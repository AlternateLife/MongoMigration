using System;
using System.Collections.Generic;
using AlternateLife.MongoMigration.Interfaces;

namespace AlternateLife.MongoMigration.Exceptions
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
