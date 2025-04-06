using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Data;
using Duo.Repositories;
using Duo.Models.Quizzes;
using System.Threading.Tasks;
using System.Linq;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class ExamRepositoryUT
    {
        private IExamRepository _repository = null!;
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
            _repository = new ExamRepository(_dbConnection);
        }

        [TestMethod]
        public async Task AddAndGetById_ShouldReturnSameExam()
        {
            var newExam = new Exam(0, null);

            var newId = await _repository.AddAsync(newExam);
            var examFromDb = await _repository.GetByIdAsync(newId);

            Assert.IsNotNull(examFromDb);
            Assert.AreEqual(newId, examFromDb.Id);
            Assert.IsNull(examFromDb.SectionId);
        }
    }
}
