using MongoDB.Bson;
using MongoDB.Driver;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Repositories.Abstractions
{
	public interface IMongoRepository<T>
        where T : IDocument
    {
        Task<T> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAll(
            int pageNumber,
            int pageSize,
            FilterDefinition<T> filterDefinition,
            SortDefinition<T> sortDefinition,
            ProjectionDefinition<T, BsonDocument> projectionDefinition);

        Task<T> CreateAsync(T document);

        Task<bool> UpdateOneAsync(T document);

        Task<bool> DeleteByIdAsync(string id);

        
    }
}

