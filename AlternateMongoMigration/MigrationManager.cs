using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlternateMongoMigration.DatabaseModels;
using AlternateMongoMigration.Interfaces;
using MongoDB.Driver;

namespace AlternateMongoMigration
{
    public class MigrationManager : IMigrationManager
    {
        private readonly List<IMigration> _migrations;
        // TODO: Replace clients with databases
        private readonly Dictionary<string, MongoClient> _clients;

        private string _migrationDatabaseName = "general";
        private string _migrationDatabaseCollection = "migration";
        private IMongoCollection<MigrationModel> _migrationCollection;

        public string MigrationDatabaseName
        {
            get => _migrationDatabaseName;
            set
            {
                _migrationDatabaseName = value;

                _migrationCollection = null;
            }
        }

        public string MigrationDatabaseCollection
        {
            get => _migrationDatabaseCollection;
            set
            {
                _migrationDatabaseCollection = value;

                _migrationCollection = null;
            }
        }

        public MigrationManager()
        {
            _migrations = new List<IMigration>();
            _clients = new Dictionary<string, MongoClient>();
        }

        public void AddClientForDatabase(MongoClient client, string databaseName)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            _clients[databaseName] = client;
        }

        public MongoClient GetClientForDatabase(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            return _clients.ContainsKey(databaseName) ? _clients[databaseName] : null;
        }

        public void LoadMigrations()
        {
            LoadMigration(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void LoadMigration(IEnumerable<Assembly> assemblies)
        {
            // get all migration type infos
            var migrationTypes = new List<TypeInfo>();

            foreach (var assembly in assemblies)
            {
                migrationTypes.AddRange(assembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IMigration))).OrderBy(m => m.Name));
            }

            foreach (var typeInfo in migrationTypes)
            {
                var migration = (IMigration) Activator.CreateInstance(typeInfo.AsType(), this);

                _migrations.Add(migration);
            }
        }

        public IEnumerable<IMigration> GetMigrations()
        {
            return _migrations;
        }

        public IEnumerable<IMigration> GetUnappliedMigrations()
        {
            return _migrations.Where(m => IsMigrationApplied(m) == false);
        }

        public IEnumerable<IMigration> GetAppliedMigrations()
        {
            return _migrations.Where(IsMigrationApplied);
        }

        public bool IsMigrationApplied(string name)
        {
            return GetMigrationCollection().Count(m => m.Migration == name) > 0;
        }

        public bool IsMigrationApplied(IMigration migration)
        {
            return IsMigrationApplied(migration.GetType().Name);
        }

        private IMongoCollection<MigrationModel> GetMigrationCollection()
        {
            if (_migrationCollection != null)
            {
                return _migrationCollection;
            }

            if (_clients.ContainsKey(MigrationDatabaseName) == false)
            {
                throw new MissingFieldException("Migration database client is missing");
            }

            var client = _clients[MigrationDatabaseName];
            var database = client.GetDatabase(MigrationDatabaseName);

            _migrationCollection = database.GetCollection<MigrationModel>(MigrationDatabaseCollection);

            return _migrationCollection;
        }
    }
}
