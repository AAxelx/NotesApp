using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Configuration.Abstractions;
using NotesApp.DAL.DataAccess.Models.Abstractions;
using NotesApp.DAL.DataAccess.Repositories.Abstractions;

namespace NotesApp.DAL.DataAccess.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T>
        where T : IDocument
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<T>> GetAll(
            int pageNumber,
            int pageSize,
            FilterDefinition<T> filterDefinition,
            SortDefinition<T> sortDefinition,
            ProjectionDefinition<T, BsonDocument> projectionDefinition)
        {
            var aggregate = await _collection.Aggregate()
                .Match(filterDefinition)
                .Sort(sortDefinition)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .Project(projectionDefinition)
                .ToListAsync();

            var result = new List<T>();
            foreach (var bsonDocument in aggregate)
            {
                var document = BsonSerializer.Deserialize<T>(bsonDocument);
                result.Add(document);
            }

            return result;
        }

        public virtual async Task<T> CreateAsync(T document)
        {
            await _collection.InsertOneAsync(document);

            return document;
        }

        public virtual async Task<bool> UpdateOneAsync(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);

            var result = await _collection.ReplaceOneAsync(filter, document);

            return Convert.ToBoolean(result.ModifiedCount);
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            var result = await _collection.DeleteOneAsync(filter);

            return Convert.ToBoolean(result.DeletedCount);
        }

        private protected string GetCollectionName(Type documentType)
        {
            var attribute = documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute), true
            ).FirstOrDefault() as BsonCollectionAttribute;

            return attribute?.CollectionName ?? null;
        }
    }
}
