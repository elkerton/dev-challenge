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

        public Domain(string domainName, string ip, string nsList, int ttl, string whois, string hostedAt)
        {
            if (string.IsNullOrEmpty(domainName)) throw new ArgumentException("Domínio Nulo ou Vazio");
            if (!isValid(domainName)) throw new ArgumentException("Domínio Inválido");
            if (ip == null) throw new ArgumentException("Ip é Nulo");
            if (string.IsNullOrEmpty(whois)) throw new ArgumentException("WhoIs é Inválido");
            if (string.IsNullOrEmpty(nsList)) throw new ArgumentException("NSList é Nulo");
            if (ttl < 0) throw new ArgumentException("TTl é negativo");

            NsList = nsList;
            Ip = ip;
            Ttl = ttl;
            UpdatedAt = DateTime.Now;
            WhoIs = whois;
            HostedAt = hostedAt;
            Name = domainName;
        }

        [Key] 
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Ip { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string WhoIs { get; private set; }
        public int Ttl { get; private set; }
        public string HostedAt { get; private set; }
        public string NsList { get; private set; }

        public List<string> GetNsList()
        {
            return this.NsList.Split(';').ToList();
        }

        public bool IsUpdated()
        {
            return DateTime.Now.Subtract(UpdatedAt).TotalMinutes < Ttl;
        }

        public static bool isValid(string domain)
        {
            var regex = new Regex(
                @"^[a-zA-Z0-9][a-zA-Z0-9-]{1,61}[a-zA-Z0-9]\.(([a-zA-Z]{2,})|([a-zA-Z]{2,}\.[a-zA-Z]{2,2}))$");

            return regex.Match(domain).Success;
        }
    }
}
