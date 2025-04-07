using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Data;
using System.Collections.Generic;

namespace DuoTesting.Helper
{
    public abstract class TestBase
    {
        protected DatabaseConnection DbConnection { get; private set; } = null!;

        [TestInitialize]
        public void BaseSetup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DbConnection", "Server=DESKTOP-BVGO48P\\SQLEXPRESS;Database=new-league;Trusted_Connection=True;TrustServerCertificate=True;" }
                })
                .Build();

            DbConnection = new DatabaseConnection(config);
        }
    }
}
