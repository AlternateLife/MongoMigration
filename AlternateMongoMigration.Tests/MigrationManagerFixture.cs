using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace AlternateMongoMigration.Tests
{
    [TestFixture]
    public class MigrationManagerFixture
    {
        [Test]
        public void DefaultProperties()
        {
            var migrationManager = new MigrationManager();

            Assert.AreEqual(migrationManager.MigrationDatabaseName, "general");
            Assert.AreEqual(migrationManager.MigrationDatabaseCollection, "migration");
            Assert.AreEqual(migrationManager.Batch, 0);
        }

        [Test]
        public void ConnectWithDefaultConfigurations()
        {
            var migrationManager = new MigrationManager();

            // add general database
            var generalDatabase = new Mock<IMongoDatabase>();

            migrationManager.AddDatabase(generalDatabase.Object, "general");

            Assert.AreEqual(migrationManager.GetDatabase("general"), generalDatabase.Object);
        }
    }
}
