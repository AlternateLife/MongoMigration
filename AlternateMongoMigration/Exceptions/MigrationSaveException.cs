using System;

namespace AlternateMongoMigration.Exceptions
{
    public class MigrationSaveException : Exception
    {
        public string MigrationName { get; }

        public MigrationSaveException(string migrationName, Exception innerException) : base($"Error saving migration {migrationName}", innerException)
        {
            MigrationName = migrationName;
        }
    }
}
