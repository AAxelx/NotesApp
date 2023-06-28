using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models.Abstractions;
using NotesApp.Services.Models;

namespace NotesApp.Services.Services.Abstractions
{
	public interface ITaskListService
    {
        Task<ServiceResult> GetByIdAsync(ObjectId listId, ObjectId userId);

        Task<ServiceResult> GetAllByUserIdAsync(ObjectId userId);

        Task<ServiceResult> CreateAsync(IDocument taskList);

        Task<ServiceResult> UpdateAsync(IDocument taskList, ObjectId userId);

        Task<ServiceResult> DeleteAsync(ObjectId listId, ObjectId userId);

        Task<ServiceResult> GetUserAccessListAsync(ObjectId listId, ObjectId userId);

        Task<ServiceResult> AddUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId newUserAcessId);

        Task<ServiceResult> RemoveUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId oldUserAcessId);
    }
}

