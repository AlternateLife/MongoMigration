using System;
using System.Collections.Generic;
using System.Linq;
using AlternateLife.MongoMigration.Tests.SampleMigrations;
using AlternateLife.MongoMigration.DatabaseModels;
using AlternateLife.MongoMigration.Interfaces;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace AlternateLife.MongoMigration.Tests
{
    [TestFixture]
    public class MigrationManagerFixture
    {
        [Test]
        public void VerifyDefaultProperties()
        {
            var migrationManager = new MigrationManager();

            Assert.AreEqual(migrationManager.MigrationDatabaseName, "general");
            Assert.AreEqual(migrationManager.MigrationDatabaseCollection, "migration");
            Assert.AreEqual(migrationManager.Batch, 0);
        }

        [Test]
        public void AddDatabases()
        {
            var migrationManager = new MigrationManager();

            var databaseMock = new Mock<IMongoDatabase>();

            Assert.Throws<ArgumentNullException>(() => migrationManager.AddDatabase(null, "general"));
            Assert.Throws<ArgumentNullException>(() => migrationManager.AddDatabaseConnection(null, "general"));
            Assert.Throws<ArgumentNullException>(() => migrationManager.AddDatabase(databaseMock.Object, null));
            Assert.Throws<ArgumentNullException>(() => migrationManager.AddDatabase(databaseMock.Object, ""));

            Assert.DoesNotThrow(() => migrationManager.AddDatabase(databaseMock.Object, "general"));
        }

        [Test]
        public void GetDatabases()
        {
            var migrationManager = new MigrationManager();

            Assert.Throws<ArgumentNullException>(() => migrationManager.GetDatabase(null));
            Assert.Throws<ArgumentNullException>(() => migrationManager.GetDatabase(""));

            Assert.AreEqual(migrationManager.GetDatabase("unknown"), null);

            // add valid database
            var databaseMock = new Mock<IMongoDatabase>();
            migrationManager.AddDatabase(databaseMock.Object, "general");

            Assert.AreEqual(migrationManager.GetDatabase("general"), databaseMock.Object);
        }

        [Test]
        public void LoadMigrations()
        {
            var migrationManager = new MigrationManager();

            Assert.AreEqual(migrationManager.GetMigrations().Count(), 0);

            migrationManager.LoadMigrations();

            Assert.AreEqual(migrationManager.GetMigrations().Count(), 1);
            Assert.AreEqual(migrationManager.GetMigrations().First().GetType(), typeof(Migration_001));
        }
    }
}
