using System.Linq.Expressions;
using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Repositories.Abstractions
{
	public interface IMongoRepository<T>
        where T : IDocument
    {
        IQueryable<T> AsQueryable();

        IEnumerable<T> FilterBy(Expression<Func<T, bool>> filterExpression);

        Task<T> FindAsync(Expression<Func<T, bool>> filterExpression);

        Task<T> FindTaskListByUserIdAsync(ObjectId id);

        Task InsertAsync(T document);

        Task DeleteByIdAsync(ObjectId id);
    }
}

