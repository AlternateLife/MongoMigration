using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlternateMongoMigration.Interfaces;
using MongoDB.Driver;

namespace AlternateMongoMigration
{
    public class MigrationManager : IMigrationManager
    {
        private readonly List<IMigration> _migrations;
        private readonly Dictionary<string, MongoClient> _clients;

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
                var migration = (IMigration) Activator.CreateInstance(typeInfo.AsType());

                _migrations.Add(migration);
            }
        }

        public IEnumerable<IMigration> GetMigrations()
        {
            return _migrations;
        }

        public IEnumerable<IMigration> GetUnappliedMigrations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMigration> GetAppliedMigrations()
        {
            throw new NotImplementedException();
        }

        public bool IsMigrationApplied(string name)
        {
            throw new NotImplementedException();
        }

        public bool IsMigrationApplied(IMigration migration)
        {
            return IsMigrationApplied(migration.GetType().Name);
        }
    }
}
