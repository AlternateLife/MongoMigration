using System;
using System.Collections.Generic;
using System.Reflection;
using AlternateMongoMigration.Exceptions;
using MongoDB.Driver;

namespace AlternateMongoMigration.Interfaces
{
    public interface IMigrationManager
    {
        /// <summary>
        /// Name of the migration database
        /// </summary>
        string MigrationDatabaseName { get; set; }

        /// <summary>
        /// Name of the migration collection in the migration database
        /// </summary>
        string MigrationDatabaseCollection { get; set; }

        /// <summary>
        /// Number of the current migration batch
        /// </summary>
        int Batch { get; }

        /// <summary>
        /// Add a database to the manager by it's connection string and reference it by it's name.
        /// </summary>
        /// <param name="connectionString">MongoDB connection string to connect to the database</param>
        /// <param name="databaseName">Name of the database</param>
        /// <exception cref="ArgumentNullException">Thrown if connectionString is or databaseName is null or empty</exception>
        void AddDatabaseConnection(string connectionString, string databaseName);

        /// <summary>
        /// Add a database to the manager and reference it by it's name.
        /// </summary>
        /// <param name="database">MongoDB database</param>
        /// <param name="databaseName">Name of the database</param>
        /// <exception cref="ArgumentNullException">Thrown if database is null or databaseName is null or empty</exception>
        void AddDatabase(IMongoDatabase database, string databaseName);

        /// <summary>
        /// Get a MongoDB database by its name
        /// </summary>
        /// <param name="databaseName">Name of the database</param>
        /// <returns>MongoDB database for the given name or null if no database was found</returns>
        /// <exception cref="ArgumentNullException">Thrown if databaseName is null or empty</exception>
        IMongoDatabase GetDatabase(string databaseName);

        /// <summary>
        /// Load all migrations found in the current app domain.
        /// </summary>
        void LoadMigrations();

        /// <summary>
        /// Load all migrations found in the assemblies.
        /// </summary>
        /// <param name="assemblies">Collection of assemblies to search in</param>
        void LoadMigrations(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Apply all migration which haven't been applied yet.
        /// </summary>
        /// <returns>Collection of applied migrations</returns>
        /// <exception cref="MigrationErrorException">Thrown if a migration could not be applied</exception>
        IEnumerable<IMigration> ApplyMigrations();

        /// <summary>
        /// Get all loaded migrations.
        ///
        /// All migrations found in LoadMigrations methods applied or not.
        /// </summary>
        /// <returns>Collection of all migrations</returns>
        IEnumerable<IMigration> GetMigrations();

        /// <summary>
        /// Get all unapplied migrations.
        /// </summary>
        /// <returns>Collection of unapplied migrations</returns>
        IEnumerable<IMigration> GetUnappliedMigrations();

        /// <summary>
        /// Get all applied migrations.
        /// </summary>
        /// <returns>Collection of applied migrations</returns>
        IEnumerable<IMigration> GetAppliedMigrations();

        /// <summary>
        /// Check if migration is applied.
        /// </summary>
        /// <param name="name">Name of the migration</param>
        /// <returns>True if migration was applied at any time, otherwise false</returns>
        bool IsMigrationApplied(string name);

        /// <summary>
        /// Check if migration is applied.
        /// </summary>
        /// <param name="migration">Migration to check</param>
        /// <returns>True if the migration was applied at any time, otherwise false</returns>
        bool IsMigrationApplied(IMigration migration);
    }
}
