using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NotesApp.Api.Models.Requests.TaskList;
using NotesApp.DAL.DataAccess.Models;
using NotesApp.Services.Services.Abstractions;

namespace NotesApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/{userId}/[controller]")]
    public class TaskListsController : BaseController
    {
        private readonly ITaskListService _taskListService;
        private readonly IMapper _mapper;

        public TaskListsController(ITaskListService taskListService, IMapper mapper) : base(mapper)
        {
            _taskListService = taskListService;
            _mapper = mapper;
        }

        [HttpGet("{taskListId}")]
        public async Task<IActionResult> GetById(ObjectId taskListId, ObjectId userId)
        {
            var result = await _taskListService.GetByIdAsync(taskListId, userId);

            return MapResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(ObjectId userId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _taskListService.GetAllByUserIdAsync(userId, pageNumber, pageSize);

            return MapResponse(result, _mapper.Map<IEnumerable<TaskListLiteDto>>);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskListDto requestModel)
        {
            var taskList = _mapper.Map<TaskListDto>(requestModel);
            var result = await _taskListService.CreateAsync(taskList);

            return MapResponse(result);
        }

        [HttpPut("{taskListId}")]
        public async Task<IActionResult> Update([FromBody] UpdateTaskListDto requestModel, ObjectId userId, ObjectId taskListId)              
        {
            var taskList = _mapper.Map<TaskListDto>(requestModel);
            taskList.Id = taskListId;
            var result = await _taskListService.UpdateAsync(taskList, userId);

            return MapResponse(result);
        }

        [HttpDelete("{taskListId}")]
        public async Task<IActionResult> Delete(ObjectId taskListId, ObjectId userId)
        {
            var result = await _taskListService.DeleteAsync(taskListId, userId);

            return MapResponse(result);
        }

        [HttpGet("{taskListId}/accesses")]
        public async Task<IActionResult> GetAccesses(ObjectId taskListId, ObjectId userId)
        {
            var result = await _taskListService.GetUserAccessListAsync(taskListId, userId);

            return MapResponse(result);
        }

        [HttpPut("{taskListId}/accesses/{newUserAccessId}")]
        public async Task<IActionResult> AddAccess(ObjectId taskListId, ObjectId userId, ObjectId newUserAccessId)
        {
            var result = await _taskListService.AddUserAccessAsync(taskListId, userId, newUserAccessId);

            return MapResponse(result);
        }

        [HttpPut("{taskListId}/accesses/{userIdForRemove}")]
        public async Task<IActionResult> RemoveAccess(ObjectId taskListId, ObjectId userId, ObjectId userIdForRemove)
        {
            var result = await _taskListService.RemoveUserAccessAsync(taskListId, userId, userIdForRemove);

            return MapResponse(result);
        }
        
    }
}

