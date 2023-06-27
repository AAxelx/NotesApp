﻿using System;
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

        public virtual async Task<T> GetByIdAsync(ObjectId id)
        {
            var filterDocument = Builders<T>.Filter.Eq(doc => doc.Id, id).ToBsonDocument();

            var filterExpression = new BsonDocumentFilterDefinition<T>(filterDocument);
            var result = await _collection.Find(filterExpression).FirstOrDefaultAsync();

            return result;
        }

        public virtual async Task<IEnumerable<T>> GetAllTaskListsByUserIdAsync(ObjectId userId)
        {
            var filterDocument = Builders<ITaskList>.Filter.Or(
                    Builders<ITaskList>.Filter.Eq(t => t.OwnerId, userId),
                    Builders<ITaskList>.Filter.ElemMatch(t => t.SharedAccessUserIds,
                        Builders<ObjectId>.Filter.Eq(i => i, userId))).ToBsonDocument();
            
            var filterExpression = new BsonDocumentFilterDefinition<T>(filterDocument);
            var result = await _collection.FindAsync(filterExpression); // get list
            return await result.ToListAsync();
        }

        public virtual async Task CreateAsync(T document)
        {

            await _collection.InsertOneAsync(document);
        }

        public virtual async Task UpdateOneAsync(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            var result = await _collection.FindOneAndDeleteAsync(filter);
        }

        public virtual IQueryable<T> AsQueryable()
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

        public virtual IEnumerable<T> FilterBy(
            Expression<Func<T, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }
    }
}

