using AlternateMongoMigration.Interfaces;
using MongoDB.Driver;

namespace AlternateMongoMigration
{
    public abstract class MigrationBase : IMigration
    {
        public string Name => GetType().Name;

        private readonly MigrationManager _migrationManager;

        protected MigrationBase(MigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public abstract void Up();
        public abstract void Down();

        protected IMongoDatabase GetDatabase(string database)
        {
            return _migrationManager.GetDatabase(database);
        }
    }
}
