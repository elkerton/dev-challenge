using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desafio.Umbler.Entities;
using DnsClient;
using DnsClient.Protocol;
using Whois.NET;

namespace Desafio.Umbler.Services
{
    public class DomainService
    {
        private Domain _domain;
        public async Task<Domain> SearchDomain(string domainName)
        {
            try
            {
                var response = await WhoisClient.QueryAsync(domainName);
                var whois = response.Raw;
                var lookup = new LookupClient();
                var result = await lookup.QueryAsync(domainName, QueryType.A);
                var resultNS = await lookup.QueryAsync(domainName, QueryType.NS);
                var record = result.Answers.ARecords().FirstOrDefault();
                var address = record?.Address;
                var ip = address?.ToString();
                var NSRecords = resultNS.Answers.NsRecords().ToList();
                var ttl = record?.TimeToLive ?? 0;
                var nsList = GetNsList(NSRecords);
                var hostResponse = await WhoisClient.QueryAsync(ip);
                var hostedAt = hostResponse.OrganizationName;

                _domain = new Domain(domainName, ip, nsList, ttl, whois, hostedAt);

                return _domain;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private string GetNsList(List<NsRecord> nsRecords)
        {
            var nsRecordUmounted = string.Empty;
            var lastItem = nsRecords.Last();

            nsRecords.ForEach(nr => {
                nsRecordUmounted += nr.NSDName == lastItem.NSDName ? nr.NSDName : nr.NSDName + ";";
            });

            return nsRecordUmounted;
        }
    }
}
