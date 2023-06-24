using System;
using System.Diagnostics;
using System.Linq.Expressions;
using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.DAL.DataAccess.Repositories.Abstractions
{
	public interface IMongoRepository
    {
        IQueryable<IDocument> AsQueryable();

        IEnumerable<IDocument> FilterBy(Expression<Func<IDocument, bool>> filterExpression);

        Task<IDocument> FindAsync(Expression<Func<IDocument, bool>> filterExpression);

        Task<IDocument> FindTaskListByUserIdAsync(ObjectId id);

        Task InsertAsync(IDocument document);

        Task DeleteByIdAsync(ObjectId id);
    }
}

