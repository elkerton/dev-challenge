using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Desafio.Umbler.Models;
using Whois.NET;
using Microsoft.EntityFrameworkCore;
using DnsClient;
using Desafio.Umbler.Entities;
using Desafio.Umbler.Services;

namespace Desafio.Umbler.Controllers
{
    [Route("api")]
    public class DomainController : Controller
    {
        private readonly DatabaseContext _db;
        private Domain _domain;
        private DomainService _domainService;

        public DomainController(DatabaseContext db, DomainService domainService)
        {
            _db = db;
            _domainService = domainService;
        }

        [HttpGet, Route("domain/{domainName}")]
        public async Task<IActionResult> Get(string domainName)
        {
            try
            {
                _domain = await _db.Domains.FirstOrDefaultAsync(d => d.Name == domainName);

                if (_domain == null)
                {
                    _domain = await _domainService.SearchDomain(domainName);

                    _db.Domains.Add(_domain);
                }

                if (!_domain.IsUpdated())
                {
                    _domain = await _domainService.SearchDomain(domainName);

                    _db.Domains.Update(_domain);

                }

                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            var domainResponse = new DomainResponse(_domain);

            return Ok(domainResponse);
        }
    }
}
