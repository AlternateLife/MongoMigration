using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AlternateLife.MongoMigration.DatabaseModels
{
    public class MigrationModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

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
