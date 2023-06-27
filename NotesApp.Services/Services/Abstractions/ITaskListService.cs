using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models.Abstractions;

namespace NotesApp.Services.Services.Abstractions
{
	public interface ITaskListService
    {
        Task<ITaskList> Get(ObjectId listId);

        Task<IEnumerable<ITaskList>> GetAll(ObjectId listId);

        Task<IEnumerable<ITaskList>> GetAllByUserId(ObjectId listId);

        Task<ITaskList> Create(ITaskList taskList);

        Task<ITaskList> Update(ITaskList taskList);

        Task Delete(ObjectId listId);

        Task<ITaskList> GetUserAccessList(ObjectId listId);

        Task<ITaskList> AddUserAccess(ObjectId listId, ObjectId userId);

        Task<ITaskList> RemoveUserAccess(ObjectId listId, ObjectId userId);
    }
}

