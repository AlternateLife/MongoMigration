using System;

namespace AlternateLife.MongoMigration.Exceptions
{
    public class MigrationCollectionNotFoundException : Exception
    {
        public MigrationCollectionNotFoundException() : base("Migration collection could not be found")
        {

        }
    }
}
