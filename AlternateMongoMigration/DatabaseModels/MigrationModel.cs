using MongoDB.Bson.Serialization.Attributes;

namespace AlternateMongoMigration.DatabaseModels
{
    public class MigrationModel
    {
        [BsonElement("migration")]
        public string Migration { get; set; }

        [BsonElement("batch")]
        public int Batch { get; set; }

        [BsonConstructor]
        public MigrationModel(string migration, int batch)
        {
            Migration = migration;
            Batch = batch;
        }
    }
}
