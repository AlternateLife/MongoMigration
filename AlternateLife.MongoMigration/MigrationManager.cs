﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlternateLife.MongoMigration.DatabaseModels;
using AlternateLife.MongoMigration.Exceptions;
using AlternateLife.MongoMigration.Interfaces;
using MongoDB.Driver;

namespace AlternateLife.MongoMigration
{
    public class MigrationManager : IMigrationManager
    {
        private readonly List<IMigration> _migrations;
        private readonly Dictionary<string, IMongoDatabase> _databases;

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

        public int Batch { get; private set; }

        public MigrationManager()
        {
            _migrations = new List<IMigration>();
            _databases = new Dictionary<string, IMongoDatabase>();
        }

        public void AddDatabaseConnection(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            var client = new MongoClient(connectionString);

            _databases[databaseName] = client.GetDatabase(databaseName);
        }

        public void AddDatabase(IMongoDatabase database, string databaseName)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            _databases[databaseName] = database;
        }

        public IMongoDatabase GetDatabase(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            return _databases.ContainsKey(databaseName) ? _databases[databaseName] : null;
        }

        public void LoadMigrations()
        {
            LoadMigrations(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void LoadMigrations(IEnumerable<Assembly> assemblies)
        {
            // get all migration type infos
            var migrationTypes = new List<TypeInfo>();

            foreach (var assembly in assemblies)
            {
                migrationTypes.AddRange(assembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IMigration)) && t.IsAbstract == false).OrderBy(m => m.Name));
            }

            foreach (var typeInfo in migrationTypes)
            {
                var migration = (IMigration) Activator.CreateInstance(typeInfo.AsType());
                migration.Setup(this);

                _migrations.Add(migration);
            }
        }

        public IEnumerable<IMigration> ApplyMigrations()
        {
            var appliedMigrations = new List<IMigration>();

            foreach (var migration in GetUnappliedMigrations())
            {
                try
                {
                    migration.Up();
                }
                catch (Exception innerException)
                {
                    throw new MigrationErrorException(migration.Name, appliedMigrations, innerException);
                }

                // add migration entry
                try
                {
                    GetMigrationCollection().InsertOne(new MigrationModel(migration.Name, Batch));
                }
                catch (MongoException innerException)
                {
                    throw new MigrationSaveException(migration.Name, innerException);
                }

                appliedMigrations.Add(migration);
            }

            return appliedMigrations;
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

            if (_databases.ContainsKey(MigrationDatabaseName) == false)
            {
                throw new MissingFieldException("Migration database client is missing");
            }

            var database = _databases[MigrationDatabaseName];

            _migrationCollection = database.GetCollection<MigrationModel>(MigrationDatabaseCollection);
            if (_migrationCollection == null)
            {
                throw new MigrationCollectionNotFoundException();
            }

            // update batch number
            var latestMigration = _migrationCollection.Find(m => true).SortByDescending(m => m.Batch).FirstOrDefault();
            if (latestMigration != null)
            {
                Batch = latestMigration.Batch + 1;
            }
            else
            {
                Batch = 0;
            }

            return _migrationCollection;
        }
    }
}
