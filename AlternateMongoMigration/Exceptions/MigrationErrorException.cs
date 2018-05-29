using System;
using System.Collections.Generic;
using AlternateMongoMigration.Interfaces;

namespace AlternateMongoMigration.Exceptions
{
    public class MigrationErrorException : MigrationException
    {
        public IEnumerable<IMigration> SuccessfulMigrations { get; }

        public MigrationErrorException(
            string migrationName,
            IEnumerable<IMigration> successfulMigrations,
            Exception innerException = null
            ) : base(
            migrationName,
            $"Error applying migration {migrationName}",
            innerException)
        {
            MigrationName = migrationName;
            SuccessfulMigrations = successfulMigrations;
        }
    }
}
