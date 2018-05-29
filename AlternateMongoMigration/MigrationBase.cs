using System;
using AlternateMongoMigration.Interfaces;
using MongoDB.Driver;

namespace AlternateMongoMigration
{
    public abstract class MigrationBase : IMigration
    {
        /// <inheritdoc cref="IMigration"/>
        public string Name => GetType().Name;

        private readonly MigrationManager _migrationManager;

        protected MigrationBase(MigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        /// <inheritdoc cref="IMigration"/>
        public abstract void Up();

        /// <inheritdoc cref="IMigration"/>
        public abstract void Down();

        /// <summary>
        /// Get a mongodb database by the name.
        /// </summary>
        /// <param name="database">Name of the database</param>
        /// <returns>MongoDB database instance or null if no database found</returns>
        /// <exception cref="ArgumentNullException">Thrown if database is null or empty</exception>
        public IMongoDatabase GetDatabase(string database)
        {
            return _migrationManager.GetDatabase(database);
        }
    }
}
