using System.Linq.Expressions;
using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Repositories.Abstractions
{
	public interface IMongoRepository<T>
        where T : IDocument
    {
        IQueryable<T> AsQueryable();

        Task<T> GetByIdAsync(ObjectId id);

        Task<IEnumerable<T>> GetAllTaskListsByUserIdAsync(ObjectId userId);

        Task CreateAsync(T document);

        Task UpdateOneAsync(T document);

        Task DeleteByIdAsync(ObjectId id);
    }
}

