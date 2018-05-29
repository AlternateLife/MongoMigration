using System;

namespace AlternateMongoMigration.Exceptions
{
    public class MigrationSaveException : MigrationException
    {
        public MigrationSaveException(string migrationName, Exception innerException = null) : base(migrationName, $"Error saving migration {migrationName}", innerException)
        {
            MigrationName = migrationName;
        }
    }
}
