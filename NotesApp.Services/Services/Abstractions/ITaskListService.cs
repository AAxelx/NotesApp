using NotesApp.DAL.DataAccess.Models;
using NotesApp.Services.Models;

namespace NotesApp.Services.Services.Abstractions
{
	public interface ITaskListService
    {
        Task<ServiceValueResult<TaskList>> GetByIdAsync(string listId, string userId);

        Task<ServiceValueResult<List<TaskList>>> GetAll(string userId, int pageNumber, int pageSize);

        Task<ServiceValueResult<TaskList>> CreateAsync(TaskList taskList);

        Task<ServiceValueResult<TaskList>> UpdateAsync(TaskList taskList, string userId);

        Task<ServiceResult> DeleteAsync(string listId, string userId);

        Task<ServiceValueResult<List<string>>> GetUserAccessListAsync(string listId, string userId);

        Task<ServiceValueResult<TaskList>> AddUserAccessAsync(string listId, string userId, string newUserAcessId);

        Task<ServiceValueResult<TaskList>> RemoveUserAccessAsync(string listId, string userId, string oldUserAcessId);
    }
}

