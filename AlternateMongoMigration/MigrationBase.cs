using System;
using AlternateMongoMigration.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AlternateMongoMigration
{
    public abstract class MigrationBase : IMigration
    {
        /// <inheritdoc cref="IMigration"/>
        public string Name => GetType().Name;

        private IMigrationManager _migrationManager;

        /// <inheritdoc cref="IMigration"/>
        public abstract void Up();

        /// <inheritdoc cref="IMigration"/>
        public abstract void Down();

        /// <inheritdoc cref="IMigration"/>
        public virtual void Setup(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

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

        /// <summary>
        /// Create a new collection in the database if this collection is not already existing.
        ///
        /// If the collection already exists, nothing will be changed.
        /// </summary>
        /// <param name="database">Database to create the collection in</param>
        /// <param name="collectionName">Name of the collection</param>
        protected void CreateCollectionIfNotExisting(IMongoDatabase database, string collectionName)
        {
            var collections = database.ListCollections(new ListCollectionsOptions() { Filter = new BsonDocument("name", collectionName) });
            if (collections.Any())
            {
                return;
            }

            database.CreateCollection(collectionName);
        }
    }
}
