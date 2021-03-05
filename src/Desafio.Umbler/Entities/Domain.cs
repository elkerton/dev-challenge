using DnsClient;
using DnsClient.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Desafio.Umbler.Entities
{
    public class Domain
    {
        public Domain()
        {

        }

        public Domain(string domainName)
        {
            if (string.IsNullOrEmpty(domainName)) throw new ArgumentException("Domínio Nulo ou Vazio");
            if(!isValid(domainName)) throw new ArgumentException("Domínio Inválido");

            Name = domainName;
            UpdatedAt = DateTime.Now;   
        }

        public Domain SetARecords(IDnsQueryResponse result)
        {
            if (result == null) throw new ArgumentException("ARecord é nulo");

            var record = result.Answers.ARecords().FirstOrDefault();
            var address = record?.Address;
            var ip = address?.ToString();
            Ip = ip;
            Ttl = record?.TimeToLive ?? 0;

            return this;
        }

        public Domain SetNsRecords(IDnsQueryResponse resultNs)
        {
            if (resultNs == null) throw new ArgumentException("NsRecord é Nulo");

            var NSRecords = resultNs.Answers.NsRecords().ToList();
            NsList = this.SetNsList(NSRecords);

            return this;
        }

        public Domain SetWhois(string whois)
        {
            if (string.IsNullOrEmpty(whois)) throw new ArgumentException("WhoIs é Inválido");

            WhoIs = whois;

            return this;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string WhoIs { get; set; }
        public int Ttl { get; set; }
        public string HostedAt { get; set; }
        public string NsList { get; set; }

        public List<string> GetNsList()
        {
            return this.NsList.Split(';').ToList();
        }

        private string SetNsList(List<NsRecord> nsRecords)
        {
            var nsRecordUmounted = string.Empty;
            var lastItem = nsRecords.Last();

            nsRecords.ForEach(nr => {
                nsRecordUmounted += nr.NSDName == lastItem.NSDName ? nr.NSDName : nr.NSDName + ";";
            });

            return nsRecordUmounted;
        }

        public static bool isValid(string domain)
        {
            var regex = new Regex(@"^[a-zA-Z0-9][a-zA-Z0-9-]{1,61}[a-zA-Z0-9]\.(([a-zA-Z]{2,})|([a-zA-Z]{2,}\.[a-zA-Z]{2,2}))$");

            return regex.Match(domain).Success;
        }
    }
}
