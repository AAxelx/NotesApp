using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models;
using NotesApp.DAL.DataAccess.Models.Abstractions;
using NotesApp.Services.Models;

namespace NotesApp.Services.Services.Abstractions
{
	public interface ITaskListService
    {
        Task<ServiceValueResult<TaskList>> GetByIdAsync(ObjectId listId, ObjectId userId);

        Task<ServiceValueResult<IEnumerable<TaskList>>> GetAllByUserIdAsync(ObjectId userId);

        Task<ServiceValueResult<TaskList>> CreateAsync(TaskList taskList);

        Task<ServiceValueResult<TaskList>> UpdateAsync(TaskList taskList, ObjectId userId);

        Task<ServiceResult> DeleteAsync(ObjectId listId, ObjectId userId);

        Task<ServiceValueResult<List<ObjectId>>> GetUserAccessListAsync(ObjectId listId, ObjectId userId);

        Task<ServiceValueResult<TaskList>> AddUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId newUserAcessId);

        Task<ServiceValueResult<TaskList>> RemoveUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId oldUserAcessId);
    }
}

