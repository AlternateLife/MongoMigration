using System;

namespace AlternateLife.MongoMigration.Exceptions
{
    public class MigrationException : Exception
    {
        public string MigrationName { get; protected set; }

        public MigrationException(string migrationName, Exception innerException = null) : base($"Error for migration {migrationName}", innerException)
        {
            MigrationName = migrationName;
        }

        public MigrationException(string migrationName, string description, Exception innerException = null) : base(description, innerException)
        {
            MigrationName = migrationName;
        }
    }
}
