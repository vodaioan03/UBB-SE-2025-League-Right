using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Data;
using Duo.Repositories;
using Duo.Models;
using Duo.Models.Exercises;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class ExerciseRepositoryUT
    {
        private IExerciseRepository _repository = null!;
        private DatabaseConnection _dbConnection = null!;

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DbConnection", "Server=DESKTOP-BVGO48P\\SQLEXPRESS;Database=new-league;Trusted_Connection=True;TrustServerCertificate=true;" }
                })
                .Build();

            _dbConnection = new DatabaseConnection(config);
            _repository = new ExerciseRepository(_dbConnection);
        }
