using System.Collections.Generic;
using System.Reflection;
using MongoDB.Driver;

namespace AlternateMongoMigration.Interfaces
{
    public interface IMigrationManager
    {
        string MigrationDatabaseName { get; set; }

        string MigrationDatabaseCollection { get; set; }

        void AddClientForDatabase(MongoClient client, string databaseName);

        MongoClient GetClientForDatabase(string databaseName);

        void LoadMigrations();

        void LoadMigration(IEnumerable<Assembly> assemblies);

        IEnumerable<IMigration> GetMigrations();

        IEnumerable<IMigration> GetUnappliedMigrations();

        IEnumerable<IMigration> GetAppliedMigrations();

        bool IsMigrationApplied(string name);

        bool IsMigrationApplied(IMigration migration);
    }
}
