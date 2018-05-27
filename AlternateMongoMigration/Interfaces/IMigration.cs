namespace AlternateMongoMigration.Interfaces
{
    public interface IMigration
    {
        string Name { get; }

        void Up();
        void Down();
    }
}
