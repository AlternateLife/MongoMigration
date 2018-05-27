using System.Collections.Generic;

namespace AlternateMongoMigration.Interfaces
{
    public interface IMigrationManager
    {
        IEnumerable<IMigration> GetMigrations();

        IEnumerable<IMigration> GetUnappliedMigrations();

        IEnumerable<IMigration> GetAppliedMigrations();
    }
}
