using Desafio.Umbler.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Bogus;

namespace Desafio.Umbler.Test._Builders
{
    public class DomainBuilder
    {
        private static string _name;
        private static DateTime _updatedAt;
        private static string _ip;
        private static string _hostedAt;
        private static string _whoIs;
        private static int _ttl;
        private static string _nsList;

        public static  DomainBuilder New()
        {
            var faker = new Faker();

            _name = faker.Internet.DomainName();
            _updatedAt = faker.Date.Past();
            _ip = faker.Internet.Ip();
            _hostedAt = faker.Company.CompanyName();
            _whoIs = faker.Lorem.Paragraph();
            _ttl = faker.Random.Int();
            _nsList = faker.Lorem.Lines();

            return new DomainBuilder();
        }

        public DomainBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public DomainBuilder WithNsRecord(string nsList)
        {
            _nsList = nsList;
            return this;
        }

        public Domain Build()
        {
            return new Domain(_name);
        }
    }
}
