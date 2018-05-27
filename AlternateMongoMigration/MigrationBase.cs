using AlternateMongoMigration.Interfaces;

namespace AlternateMongoMigration
{
    public abstract class MigrationBase : IMigration
    {
        public string Name => GetType().Name;

        protected readonly MigrationManager _migrationManager;

        protected MigrationBase(MigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public abstract void Up();
        public abstract void Down();
    }
}
