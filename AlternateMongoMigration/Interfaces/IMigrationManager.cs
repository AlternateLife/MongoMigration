using System.Collections.Generic;
using System.Reflection;
using MongoDB.Driver;

namespace AlternateMongoMigration.Interfaces
{
    public interface IMigrationManager
    {
        string MigrationDatabaseName { get; set; }

        string MigrationDatabaseCollection { get; set; }

        int Batch { get; }

        void AddDatabase(IMongoDatabase database, string databaseName);

        IMongoDatabase GetDatabase(string databaseName);

        void LoadMigrations();

        void LoadMigration(IEnumerable<Assembly> assemblies);

        IEnumerable<IMigration> ApplyMigrations();

        IEnumerable<IMigration> GetMigrations();

        IEnumerable<IMigration> GetUnappliedMigrations();

        IEnumerable<IMigration> GetAppliedMigrations();

        bool IsMigrationApplied(string name);

        bool IsMigrationApplied(IMigration migration);
    }
}
