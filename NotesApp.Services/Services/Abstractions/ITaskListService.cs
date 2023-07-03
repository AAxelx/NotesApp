using NotesApp.DAL.DataAccess.Models;
using NotesApp.Services.Models;

namespace NotesApp.Services.Services.Abstractions
{
	public interface ITaskListService
    {
        Task<ServiceValueResult<TaskListDto>> GetByIdAsync(string listId, string userId);

        Task<ServiceValueResult<List<TaskListDto>>> GetAll(string userId, int pageNumber, int pageSize);

        Task<ServiceValueResult<TaskListDto>> CreateAsync(TaskListDto taskList);

        Task<ServiceValueResult<TaskListDto>> UpdateAsync(TaskListDto taskList, string userId);

        Task<ServiceResult> DeleteAsync(string listId, string userId);

        Task<ServiceValueResult<List<string>>> GetUserAccessListAsync(string listId, string userId);

        Task<ServiceValueResult<TaskListDto>> AddUserAccessAsync(string listId, string userId, string newUserAcessId);

        Task<ServiceValueResult<TaskListDto>> RemoveUserAccessAsync(string listId, string userId, string oldUserAcessId);
    }
}

