using System;
using System.Diagnostics;
using MongoDB.Bson;
using NotesApp.DAL.DataAccess.Models;
using NotesApp.DAL.DataAccess.Models.Abstractions;
using NotesApp.DAL.DataAccess.Repositories.Abstractions;
using NotesApp.Services.Services.Abstractions;

namespace NotesApp.Services.Services
{
	public class TaskListService //: ITaskListService
    {
        private readonly IMongoRepository<ITaskList> _repository;

		public TaskListService(IMongoRepository<ITaskList> repository)
		{
            _repository = repository;
		}

        public async Task<ITaskList> GetByIdAsync(ObjectId listId, ObjectId userId) //fix return
        {
            var taskList = await _repository.GetByIdAsync(listId); //fix naming

            if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
                return taskList;

            return taskList;
        }

        public async Task<IEnumerable<ITaskList>> GetAllByUserId(ObjectId userId) // fix filter
        {
            var result = await _repository.GetAllTaskListsByUserIdAsync(userId);

            return result;
        }

        public async Task<ITaskList> CreateAsync(ITaskList taskList) // check in chat gpt
        {
            await _repository.CreateAsync(taskList);

            return taskList;
        }

        public async Task<ITaskList> UpdateAsync(ITaskList taskList) // check in chat gpt
        {
            await _repository.UpdateOneAsync(taskList);
            var result = await _repository.GetByIdAsync(taskList.Id);

            return result;
        }

        public async Task<bool> DeleteAsync(ObjectId listId, ObjectId userId) // check in chat gpt
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if(taskList.OwnerId == userId)
            {
                await _repository.DeleteByIdAsync(listId);
                var result = await _repository.GetByIdAsync(listId);

                if (result == null)
                    return true;
            }

            return false;
        }

        public async Task<ITaskList> AddUserAccess(ObjectId listId, ObjectId userId, ObjectId newUserAcessId) // fix parametrs
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
            {
                taskList.SharedAccessUserIds.Add(newUserAcessId);
                await _repository.UpdateOneAsync(taskList); // need check result?

                return taskList;
            }

            return null; // fix it
        }

        public async Task<IEnumerable<ObjectId>> GetUserAccessList(ObjectId listId, ObjectId userId)
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
            {
                return taskList.SharedAccessUserIds;
            }

            return null; //fix return
        }

        public async Task<ITaskList> RemoveUserAccess(ObjectId listId, ObjectId userId, ObjectId oldUserAcessId) //fix names
        {
            var taskList = await _repository.GetByIdAsync(listId);

            if (taskList.OwnerId == userId || taskList.SharedAccessUserIds.Contains(userId))
            {
                taskList.SharedAccessUserIds.Remove(oldUserAcessId);
                await _repository.UpdateOneAsync(taskList); // need check result?

                return taskList;
            }

            return null; // fix it
        }
    }
}

