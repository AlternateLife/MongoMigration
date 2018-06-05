namespace AlternateMongoMigration.Interfaces
{
    public interface IMigration
    {
        /// <summary>
        /// Name of the migration.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Process database migration.
        /// </summary>
        void Up();

        /// <summary>
        /// Revert database migration.
        /// </summary>
        void Down();

        /// <summary>
        /// Setup the migration.
        /// </summary>
        /// <param name="migrationManager">Migration manager the migration is used in</param>
        void Setup(IMigrationManager migrationManager);
    }
}
