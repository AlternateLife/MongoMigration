using System;

namespace AlternateMongoMigration.Exceptions
{
    public class MigrationCollectionNotFoundException : Exception
    {
        public MigrationCollectionNotFoundException() : base("Migration collection could not be found")
        {

        }
    }
}
