
using MongoDB.Driver;
using NotesApp.DAL.DataAccess.Models;
using NotesApp.DAL.DataAccess.Repositories.Abstractions;
using NotesApp.Services.Models.Enums;
using NotesApp.Services.Models;
using NotesApp.Services.Services.Abstractions;

namespace NotesApp.Services.Services
{
	public class TaskListService : ITaskListService
    {
        private readonly IMongoRepository<TaskListDto> _repository;

		public TaskListService(IMongoRepository<TaskListDto> repository)
		{
            _repository = repository;
		}

        public async Task<ServiceValueResult<TaskListDto>> GetByIdAsync(string listId, string userId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if (taskList == null)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.Forbidden);
            }

            return new ServiceValueResult<TaskListDto>(taskList);
        }

        public async Task<ServiceValueResult<List<TaskListDto>>> GetAll(string userId, int pageNumber, int pageSize)
        {
            var filterDefinition = Builders<TaskListDto>.Filter.Eq(t => t.OwnerId, userId) |
                Builders<TaskListDto>.Filter.AnyEq(t => t.SharedAccessUserIds, userId);

            var sortDefinition = Builders<TaskListDto>.Sort.Descending(t => t.LastUpdatedAt);

            var projectionDefinition = Builders<TaskListDto>.Projection
                .Include(t => t.Id)
                .Include(t => t.Title);

            var result = await _repository.GetAll(pageNumber, pageSize, filterDefinition, sortDefinition, projectionDefinition).ConfigureAwait(false);

            return new ServiceValueResult<List<TaskListDto>>(result.ToList());
        }

        public async Task<ServiceValueResult<TaskListDto>> CreateAsync(TaskListDto taskList) //add user is created and owner check
        {
            var createdTaskList = await _repository.CreateAsync(taskList).ConfigureAwait(false);

            if (createdTaskList.Id == null)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskListDto>(createdTaskList);
        }

        public async Task<ServiceValueResult<TaskListDto>> UpdateAsync(TaskListDto updateTaskList, string userId)
        {
            var taskList = await _repository.GetByIdAsync(updateTaskList.Id).ConfigureAwait(false);

            if(taskList == null)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.Forbidden);
            }

            var isSuccess = await _repository.UpdateOneAsync(updateTaskList).ConfigureAwait(false);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskListDto>(updateTaskList);
        }
        
        public async Task<ServiceResult> DeleteAsync(string listId, string userId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if(taskList == null)
            {
                return new ServiceResult(ResponseType.BadRequest);
            }

            if (taskList.OwnerId != userId)
            {
                return new ServiceResult(ResponseType.Forbidden);
            }

            var isSuccess = await _repository.DeleteByIdAsync(listId).ConfigureAwait(false);

            if (!isSuccess)
            {
                return new ServiceResult(ResponseType.BadRequest);
            }

            return new ServiceResult(ResponseType.NoContent);
        }

        public async Task<ServiceValueResult<TaskListDto>> AddUserAccessAsync(string listId, string userId, string newUserAccessId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if(taskList == null)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.Forbidden);
            }

            taskList.SharedAccessUserIds.Add(newUserAccessId);
            var isSuccess = await _repository.UpdateOneAsync(taskList).ConfigureAwait(false);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskListDto>(taskList);
        }

        public async Task<ServiceValueResult<List<string>>> GetUserAccessListAsync(string listId, string userId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if (taskList == null)
            {
                return new ServiceValueResult<List<string>>(ResponseType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<List<string>>(ResponseType.Forbidden);
            }

            return new ServiceValueResult<List<string>>(taskList.SharedAccessUserIds);
        }

        public async Task<ServiceValueResult<TaskListDto>> RemoveUserAccessAsync(string listId, string userId, string userIdForRemove)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if (taskList == null)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.Forbidden);
            }

            taskList.SharedAccessUserIds.Remove(userIdForRemove);
            var isSuccess = await _repository.UpdateOneAsync(taskList).ConfigureAwait(false);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskListDto>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskListDto>(taskList);
        }

        private bool IsUserHasAccess(TaskListDto taskList, string userId)
        {
            return taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId);
        }

        
    }
}

