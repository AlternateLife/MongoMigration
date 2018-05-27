using System.Collections.Generic;
using AlternateMongoMigration.Interfaces;

namespace AlternateMongoMigration
{
    public class MigrationManager : IMigrationManager
    {
        private readonly List<IMigration> _migrations;

        public MigrationManager()
        {
            _migrations = new List<IMigration>();
        }

        public IEnumerable<IMigration> GetMigrations()
        {
            return _migrations;
        }

        public IEnumerable<IMigration> GetUnappliedMigrations()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IMigration> GetAppliedMigrations()
        {
            throw new System.NotImplementedException();
        }
    }
}
