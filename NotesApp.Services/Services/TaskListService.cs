
using MongoDB.Bson;
using MongoDB.Driver;
using NotesApp.DAL.DataAccess.Models;
using NotesApp.DAL.DataAccess.Models.Abstractions;
using NotesApp.DAL.DataAccess.Repositories.Abstractions;
using NotesApp.Services.Models.Enums;
using NotesApp.Services.Models;
using NotesApp.Services.Services.Abstractions;

namespace NotesApp.Services.Services
{
	public class TaskListService : ITaskListService
    {
        private readonly IMongoRepository<IDocument> _repository;

		public TaskListService(IMongoRepository<IDocument> repository)
		{
            _repository = repository;
		}

        public async Task<ServiceResult> GetByIdAsync(ObjectId listId, ObjectId userId)
        {
            var taskList = (TaskList)await _repository.GetByIdAsync(listId);

            if (taskList != null)
            {
                if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
                    return new ServiceValueResult<IDocument>(ErrorType.Ok, taskList);
                else
                    return new ServiceResult(ErrorType.Forbidden);
            }

            return new ServiceResult(ErrorType.BadRequest);
        }

        public async Task<ServiceResult> GetAllByUserIdAsync(ObjectId userId)
        {
            var filter = Builders<TaskList>.Filter.Or(
                Builders<TaskList>.Filter.Eq(t => t.OwnerId, userId),
                Builders<TaskList>.Filter.ElemMatch(t => t.SharedAccessUserIds, Builders<ObjectId>.Filter.Eq(i => i, userId)));

            var sortStage = new BsonDocument("$sort", new BsonDocument("LastUpdatedAt", -1));
            var pipeline = new List<BsonDocument> { filter.ToBsonDocument(), sortStage };

            var result = await _repository.GetAllAsync(pipeline);

            return new ServiceValueResult<IEnumerable<IDocument>>(ErrorType.Ok, result);
        }



        public async Task<ServiceResult> CreateAsync(IDocument taskList)
        {
            var result = await _repository.CreateAsync(taskList);

            var createdList = await _repository.GetByIdAsync(result.Id);

            if (createdList.Id != null)
                return new ServiceValueResult<IDocument>(ErrorType.Ok, createdList);

            return new ServiceResult(ErrorType.InternalServerError);
        }

        public async Task<ServiceResult> UpdateAsync(IDocument newTaskList, ObjectId userId)
        {
            var taskList = (TaskList)await _repository.GetByIdAsync(newTaskList.Id);

            if(taskList != null)
            {
                if(taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
                {
                    var success = await _repository.UpdateOneAsync(newTaskList);

                    if(success)
                        return new ServiceValueResult<IDocument>(ErrorType.Ok, newTaskList);
                    else
                        return new ServiceResult(ErrorType.InternalServerError);
                }

                return new ServiceResult(ErrorType.Forbidden);
            }

            return new ServiceResult(ErrorType.BadRequest);
        }
        
        public async Task<ServiceResult> DeleteAsync(ObjectId listId, ObjectId userId)
        {
            var taskList = (TaskList)await _repository.GetByIdAsync(listId);

            if(taskList != null)
            {
                if (taskList.OwnerId == userId)
                {
                    var success = await _repository.DeleteByIdAsync(listId);

                    if (success)
                        return new ServiceResult(ErrorType.NoContent);
                    else
                        return new ServiceResult(ErrorType.InternalServerError);
                }

                return new ServiceResult(ErrorType.Forbidden);
            }

            return new ServiceResult(ErrorType.BadRequest);
        }

        public async Task<ServiceResult> AddUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId newUserAcessId)
        {
            var taskList = (TaskList)await _repository.GetByIdAsync(listId);

            if(taskList != null)
            {
                if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
                {
                    taskList.SharedAccessUserIds.Add(newUserAcessId);
                    var success = await _repository.UpdateOneAsync(taskList);

                    if (success)
                        return new ServiceValueResult<IDocument>(ErrorType.Ok, taskList);
                    else
                        return new ServiceResult(ErrorType.InternalServerError);
                }

                return new ServiceResult(ErrorType.Forbidden);
            }

            return new ServiceResult(ErrorType.BadRequest);
        }

        public async Task<ServiceResult> GetUserAccessListAsync(ObjectId listId, ObjectId userId)
        {
            var taskList = (TaskList)await _repository.GetByIdAsync(listId);

            if (taskList != null)
            {
                if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
                {
                    return new ServiceValueResult<IEnumerable<ObjectId>>(ErrorType.Ok, taskList.SharedAccessUserIds);
                }

                return new ServiceResult(ErrorType.Forbidden);
            }

            return new ServiceResult(ErrorType.BadRequest);
        }

        public async Task<ServiceResult> RemoveUserAccessAsync(ObjectId listId, ObjectId userId, ObjectId oldUserAcessId)
        {
            var taskList = (TaskList)await _repository.GetByIdAsync(listId);

            if (taskList != null)
            {
                if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
                {
                    taskList.SharedAccessUserIds.Remove(oldUserAcessId);
                    var success = await _repository.UpdateOneAsync(taskList);

                    if (success)
                        return new ServiceValueResult<IDocument>(ErrorType.Ok, taskList);
                    else
                        return new ServiceResult(ErrorType.InternalServerError);
                }

                return new ServiceResult(ErrorType.Forbidden);
            }

            return new ServiceResult(ErrorType.BadRequest);
        }
    }
}

