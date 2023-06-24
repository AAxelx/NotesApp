using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using NotesApp.DAL.DataAccess.Configuration;
using NotesApp.DAL.DataAccess.Models.Abstractions;
using NotesApp.DAL.DataAccess.Repositories.Abstractions;

namespace NotesApp.DAL.DataAccess.Repositories
{
	public class MongoRepository : IMongoRepository
	{
        private readonly IMongoCollection<IDocument> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<IDocument>(GetCollectionName(typeof(IDocument)));
        }

        public virtual async Task<IDocument> FindAsync(Expression<Func<IDocument, bool>> filterExpression)
        {
            var result = await _collection.Find(filterExpression).FirstOrDefaultAsync();

            return result;
        }

        public virtual async Task<IDocument> FindTaskListByUserIdAsync(ObjectId userId)
        {
            var filter = Builders<ITaskList>.Filter.Or(
                    Builders<ITaskList>.Filter.Eq(t => t.OwnerId, userId),
                    Builders<ITaskList>.Filter.ElemMatch(t => t.SharedWith,
                        Builders<ObjectId>.Filter.Eq(i => i, userId)));

            var filterDocument = filter.ToBsonDocument();
            var filterExpression = new BsonDocumentFilterDefinition<IDocument>(filterDocument);

            var result = await _collection.Find(filterExpression).SingleOrDefaultAsync();
            return result;
        }

        public virtual async Task InsertAsync(IDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public virtual async Task UpdateOneAsync(IDocument document)
        {
            var filter = Builders<IDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var filter = Builders<IDocument>.Filter.Eq(doc => doc.Id, id);
            var result = await _collection.FindOneAndDeleteAsync(filter);
        }

        public virtual IQueryable<IDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        private protected string GetCollectionName(Type documentType)
        {
            var attribute = documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute), true
            ).FirstOrDefault() as BsonCollectionAttribute;

            return attribute?.CollectionName ?? null;
        }

        public virtual IEnumerable<IDocument> FilterBy(
            Expression<Func<IDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }
    }
}

