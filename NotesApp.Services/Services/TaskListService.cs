
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
        private readonly IMongoRepository<TaskList> _repository;

		public TaskListService(IMongoRepository<TaskList> repository)
		{
            _repository = repository;
		}

        public async Task<ServiceValueResult<TaskList>> GetByIdAsync(string listId, string userId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if (taskList == null)
            {
                return new ServiceValueResult<TaskList>(ResponseType.NotFound);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ResponseType.Forbidden);
            }

            return new ServiceValueResult<TaskList>(taskList);
        }

        public async Task<ServiceValueResult<List<TaskList>>> GetAll(string userId, int pageNumber, int pageSize)
        {
            var filterDefinition = Builders<TaskList>.Filter.Eq(t => t.OwnerId, userId) |
                Builders<TaskList>.Filter.AnyEq(t => t.SharedAccessUserIds, userId);

            var sortDefinition = Builders<TaskList>.Sort.Descending(t => t.LastUpdatedAt);

            var projectionDefinition = Builders<TaskList>.Projection
                .Include(t => t.Id)
                .Include(t => t.Title);

            var result = await _repository.GetAll(pageNumber, pageSize, filterDefinition, sortDefinition, projectionDefinition).ConfigureAwait(false);

            return new ServiceValueResult<List<TaskList>>(result.ToList());
        }

        public async Task<ServiceValueResult<TaskList>> CreateAsync(TaskList taskList) //add user is created and owner check
        {
            var createdTaskList = await _repository.CreateAsync(taskList).ConfigureAwait(false);

            if (createdTaskList.Id == null)
            {
                return new ServiceValueResult<TaskList>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskList>(createdTaskList);
        }

        public async Task<ServiceValueResult<TaskList>> UpdateAsync(TaskList updateTaskList, string userId)
        {
            var taskList = await _repository.GetByIdAsync(updateTaskList.Id).ConfigureAwait(false);

            if(taskList == null)
            {
                return new ServiceValueResult<TaskList>(ResponseType.NotFound);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ResponseType.Forbidden);
            }

            var isSuccess = await _repository.UpdateOneAsync(updateTaskList).ConfigureAwait(false);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskList>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskList>(updateTaskList);
        }
        
        public async Task<ServiceResult> DeleteAsync(string listId, string userId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if(taskList == null)
            {
                return new ServiceResult(ResponseType.NotFound);
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

        public async Task<ServiceValueResult<TaskList>> AddUserAccessAsync(string listId, string userId, string newUserAccessId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if(taskList == null)
            {
                return new ServiceValueResult<TaskList>(ResponseType.NotFound);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ResponseType.Forbidden);
            }

            if (taskList.SharedAccessUserIds.Contains(newUserAccessId))
            {
                return new ServiceValueResult<TaskList>(ResponseType.Ok);
            }
            taskList.SharedAccessUserIds.Add(newUserAccessId);
            var isSuccess = await _repository.UpdateOneAsync(taskList).ConfigureAwait(false);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskList>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskList>(taskList);
        }

        public async Task<ServiceValueResult<List<string>>> GetUserAccessListAsync(string listId, string userId)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if (taskList == null)
            {
                return new ServiceValueResult<List<string>>(ResponseType.NotFound);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<List<string>>(ResponseType.Forbidden);
            }

            return new ServiceValueResult<List<string>>(taskList.SharedAccessUserIds);
        }

        public async Task<ServiceValueResult<TaskList>> RemoveUserAccessAsync(string listId, string userId, string userIdForRemove)
        {
            var taskList = await _repository.GetByIdAsync(listId).ConfigureAwait(false);

            if (taskList == null)
            {
                return new ServiceValueResult<TaskList>(ResponseType.NotFound);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ResponseType.Forbidden);
            }

            taskList.SharedAccessUserIds.Remove(userIdForRemove);
            var isSuccess = await _repository.UpdateOneAsync(taskList).ConfigureAwait(false);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskList>(ResponseType.BadRequest);
            }

            return new ServiceValueResult<TaskList>(taskList);
        }

        private bool IsUserHasAccess(TaskList taskList, string userId)
        {
            return taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId);
        }

        
    }
}

