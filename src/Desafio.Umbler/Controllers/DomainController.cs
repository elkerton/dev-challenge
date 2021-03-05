using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Desafio.Umbler.Models;
using Whois.NET;
using Microsoft.EntityFrameworkCore;
using DnsClient;
using Desafio.Umbler.Entities;

namespace Desafio.Umbler.Controllers
{
    [Route("api")]
    public class DomainController : Controller
    {
        private readonly DatabaseContext _db;
        public Domain _domain;

        public DomainController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet, Route("domain/{domainName}")]
        public async Task<IActionResult> Get(string domainName)
        { 
            try
            {
                _domain = new Domain(domainName);

                _domain = await _db.Domains.FirstOrDefaultAsync(d => d.Name == domainName);

                if (_domain == null)
                {
                    _domain = await this.SearchDomain(domainName);

                    _db.Domains.Add(_domain);
                }

                if (DateTime.Now.Subtract(_domain.UpdatedAt).TotalMinutes > _domain.Ttl)
                {
                    _domain = await this.SearchDomain(domainName);

                    _db.Domains.Update(_domain);

                }

                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {   
                throw e;
            }

            var responseBody = new
            {
                Name = _domain.Name,
                Ip = _domain.Ip,
                HostedAt = _domain.HostedAt,
                WhoIs = _domain.WhoIs,
                NsRecords = _domain.GetNsList()
            };

            return Ok(responseBody);
        }

        public async Task<Domain> SearchDomain(string domainName)
        {
            var domain = new Domain();

            try
            {
                var response = await WhoisClient.QueryAsync(domainName);

                var lookup = new LookupClient();
                var result = await lookup.QueryAsync(domainName, QueryType.A);
                var resultNS = await lookup.QueryAsync(domainName, QueryType.NS);

                domain = new Domain(domainName);

                domain.SetARecords(result);

                domain.SetNsRecords(resultNS);

                domain.SetWhois(response.Raw);

                var hostResponse = await WhoisClient.QueryAsync(domain.Ip);

                domain.HostedAt = hostResponse.OrganizationName;
                domain.UpdatedAt = DateTime.Now;

                return domain;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
