using System;
using System.Collections.Generic;
using AlternateMongoMigration.Interfaces;

namespace AlternateMongoMigration.Exceptions
{
    public class MigrationErrorException : Exception
    {
        public string MigrationName { get; }
        public IEnumerable<IMigration> SuccessfulMigrations { get; }

        public MigrationErrorException(
            string migrationName,
            IEnumerable<IMigration> successfulMigrations,
            Exception innerException
            ) : base(
            $"Error applying migration {migrationName}",
            innerException)
        {
            MigrationName = migrationName;
            SuccessfulMigrations = successfulMigrations;
        }
    }
}
