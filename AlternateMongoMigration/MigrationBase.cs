using AlternateMongoMigration.Interfaces;

namespace AlternateMongoMigration
{
    public abstract class MigrationBase : IMigration
    {
        public string Name => GetType().Name;

        public MigrationBase()
        {

        }

        public abstract void Up();
        public abstract void Down();
    }
}
