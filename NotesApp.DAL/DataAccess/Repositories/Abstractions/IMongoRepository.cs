using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Repositories.Abstractions
{
	public interface IMongoRepository<T>
        where T : IDocument
    {
        Task<T> GetByIdAsync(ObjectId? id);

        Task<IEnumerable<T>> GetAllAsync(FilterDefinition<T> filterDefinition = null);

        Task<IEnumerable<T>> GetAllAsync(List<BsonDocument> pipeline);//revome mb

        Task<T> CreateAsync(T document);

        Task<bool> UpdateOneAsync(T document);

        Task<bool> DeleteByIdAsync(ObjectId? id);
    }
}

