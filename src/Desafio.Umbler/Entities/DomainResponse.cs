using System;
using System.Collections.Generic;

namespace Desafio.Umbler.Entities
{
    public class DomainResponse
    {
        public DomainResponse(Domain domain)
        {
            Name = domain.Name;
            Ip = domain.Ip;
            UpdatedAt = domain.UpdatedAt;
            WhoIs = domain.WhoIs;
            HostedAt = domain.HostedAt;
            NsList = domain.GetNsList();
        }

        public string Name { get; private set; }
        public string Ip { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string WhoIs { get; private set; }
        public string HostedAt { get; private set; }
        public List<string> NsList { get; private set; }
    }
}
