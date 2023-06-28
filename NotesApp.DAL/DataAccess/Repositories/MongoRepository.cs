using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using NotesApp.DAL.DataAccess.Configuration;
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

        public virtual async Task<T> GetByIdAsync(ObjectId? id)
        {
            var filterDocument = Builders<T>.Filter.Eq(doc => doc.Id, id).ToBsonDocument();

            var filterExpression = new BsonDocumentFilterDefinition<T>(filterDocument);
            var result = await _collection.Find(filterExpression).FirstOrDefaultAsync();

            return result;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(FilterDefinition<T> filterDefinition = null)
        {
            var filterDocument = filterDefinition.ToBsonDocument();
            var filterExpression = new BsonDocumentFilterDefinition<T>(filterDocument);

            var result = await _collection.FindAsync(filterExpression);

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(List<BsonDocument> pipeline)
        {
            var aggregation = await _collection.AggregateAsync<T>(pipeline);

            return await aggregation.ToListAsync();
        }

        public virtual async Task<T> CreateAsync(T document)
        {
            await _collection.InsertOneAsync(document);

            return document;
        }

        public virtual async Task<bool> UpdateOneAsync(T document) 
        {
            var updateDefinition = Builders<T>.Update.Set(doc => doc, document);
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);

            var result = await _collection.UpdateOneAsync(filter, updateDefinition);

            return Convert.ToBoolean(result.ModifiedCount);
        }

        public async Task<bool> DeleteByIdAsync(ObjectId? id)
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

