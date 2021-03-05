using Desafio.Umbler.Test._Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using Desafio.Umbler.Entities;
using DnsClient;
using ExpectedObjects;
using Xunit;
using Xunit.Abstractions;

namespace Desafio.Umbler.Test
{
    public class DomainTest
    {
        private readonly ITestOutputHelper _output;
        private readonly string _name;
        private readonly Faker _faker;

        public DomainTest(ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();

            _name = _faker.Internet.DomainName();
            _output.WriteLine($"Domain running on test: '{_name}'");
        }

        [Fact]
        public void Must_Create_Domain()
        {
            var domainCorrect = new
            {
                Name = _name
            };

            var domain = new Domain(domainCorrect.Name);

            domain.ToExpectedObject().ShouldMatch(domain);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Must_Name_Be_Not_Null_Or_Empty(string invalidName)
        {
            Assert.Throws<ArgumentException>(() => DomainBuilder.New().WithName(invalidName).Build());
        }

        [Fact]
        public void Must_Name_Be_Valid()
        {
            Assert.IsType<Domain>(DomainBuilder.New().WithName(_name).Build());
        }

        [Fact]
        public void Must_Name_Be_Not_Valid()
        {
            var invalidName = _faker.Name.LastName();
            Assert.Throws<ArgumentException>(() => DomainBuilder.New().WithName(invalidName).Build());
        }

        [Fact]
        public void Must_ARecords_Be_Not_Null()
        {
            var domain = new Domain(_name);

            Assert.Throws<ArgumentException>(() => domain.SetARecords(null));
        }

        [Fact]
        public void Must_NsRecords_Be_Not_Null()
        {
            var domain = new Domain(_name);

            Assert.Throws<ArgumentException>(() => domain.SetNsRecords(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Must_Whois_Be_Not_Null_OR_Empty(string whoisInvalid)
        {
            var domain = new Domain(_name);

            Assert.Throws<ArgumentException>(() => domain.SetWhois(whoisInvalid));
        }
    }
}
