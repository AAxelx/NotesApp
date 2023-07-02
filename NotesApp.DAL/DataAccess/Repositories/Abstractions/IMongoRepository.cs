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

        Task<IEnumerable<TProjection>> GetAllAsync<TProjection>(
            int pageNumber,
            int pageSize,
            FilterDefinition<T> filterDefinition = null,
            ProjectionDefinition<T, TProjection> projectionDefinition = null,
            SortDefinition<T> sortDefinition = null);

        Task<T> CreateAsync(T document);

        Task<bool> UpdateOneAsync(T document);

        Task<bool> DeleteByIdAsync(ObjectId? id);
    }
}

