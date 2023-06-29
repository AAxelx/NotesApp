
using MongoDB.Bson;
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

        public async Task<ServiceValueResult<TaskList>> GetByIdAsync(ObjectId listId, ObjectId userId)
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if (taskList == null)
            {
                return new ServiceValueResult<TaskList>(ErrorType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ErrorType.Forbidden);
            }

            return new ServiceValueResult<TaskList>(taskList);
        }

        public async Task<ServiceValueResult<IEnumerable<TaskList>>> GetAllByUserIdAsync(ObjectId userId)
        {
            var filter = Builders<TaskList>.Filter.Or(
                Builders<TaskList>.Filter.Eq(t => t.OwnerId, userId),
                Builders<TaskList>.Filter.ElemMatch(t => t.SharedAccessUserIds, Builders<ObjectId>.Filter.Eq(i => i, userId)));

            var sortStage = new BsonDocument("$sort", new BsonDocument("LastUpdatedAt", -1));
            var pipeline = new List<BsonDocument> { filter.ToBsonDocument(), sortStage };

            var result = (List<TaskList>)await _repository.GetAllAsync(pipeline);

            return new ServiceValueResult<IEnumerable<TaskList>>(result);
        }

        public async Task<ServiceValueResult<TaskList>> CreateAsync(TaskList taskList)
        {
            var createdTaskList = await _repository.CreateAsync(taskList);

            if (createdTaskList.Id == null)
            {
                return new ServiceValueResult<TaskList>(ErrorType.InternalServerError);
            }

            return new ServiceValueResult<TaskList>(createdTaskList);
        }

        public async Task<ServiceValueResult<TaskList>> UpdateAsync(TaskList updateTaskList, ObjectId userId)
        {
            var taskList = await _repository.GetByIdAsync(updateTaskList.Id);

            if(taskList == null)
            {
                return new ServiceValueResult<TaskList>(ErrorType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ErrorType.Forbidden);
            }

            var isSuccess = await _repository.UpdateOneAsync(updateTaskList); //асинхронное обращение к бд

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskList>(ErrorType.InternalServerError);
            }

            return new ServiceValueResult<TaskList>(updateTaskList); //fix порядок return // протестировать
        }
        
        public async Task<ServiceResult> DeleteAsync(ObjectId listId, ObjectId userId)
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if(taskList == null)
            {
                return new ServiceResult(ErrorType.BadRequest);
            }

            if (taskList.OwnerId != userId)
            {
                return new ServiceResult(ErrorType.Forbidden);
            }

            var isSuccess = await _repository.DeleteByIdAsync(listId);

            if (!isSuccess)
            {
                return new ServiceResult(ErrorType.InternalServerError);
            }

            return new ServiceResult(ErrorType.NoContent);
        }

        public async Task<ServiceValueResult<TaskList>> AddUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId newUserAccessId)
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if(taskList == null)
            {
                return new ServiceValueResult<TaskList>(ErrorType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ErrorType.Forbidden);
            }

            taskList.SharedAccessUserIds.Add(newUserAccessId);
            var isSuccess = await _repository.UpdateOneAsync(taskList);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskList>(ErrorType.InternalServerError);
            }

            return new ServiceValueResult<TaskList>(taskList);
        }

        public async Task<ServiceValueResult<List<ObjectId>>> GetUserAccessListAsync(ObjectId listId, ObjectId userId)
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if (taskList == null)
            {
                return new ServiceValueResult<List<ObjectId>>(ErrorType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<List<ObjectId>>(ErrorType.Forbidden);
            }

            return new ServiceValueResult<List<ObjectId>>(taskList.SharedAccessUserIds);
        }

        public async Task<ServiceValueResult<TaskList>> RemoveUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId userIdForRemove)
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if (taskList == null)
            {
                return new ServiceValueResult<TaskList>(ErrorType.BadRequest);
            }

            var hasAccess = IsUserHasAccess(taskList, userId);

            if (!hasAccess)
            {
                return new ServiceValueResult<TaskList>(ErrorType.Forbidden);
            }

            taskList.SharedAccessUserIds.Remove(userIdForRemove);
            var isSuccess = await _repository.UpdateOneAsync(taskList);

            if (!isSuccess)
            {
                return new ServiceValueResult<TaskList>(ErrorType.InternalServerError);
            }

            return new ServiceValueResult<TaskList>(taskList);
        }

        private bool IsUserHasAccess(TaskList taskList, ObjectId userId)
        {
            return taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId);
        }
    }
}

