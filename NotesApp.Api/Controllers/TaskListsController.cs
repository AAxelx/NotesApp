using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Api.Models.Requests.TaskList;
using NotesApp.Api.Models.Responses.TaskList;
using NotesApp.Api.Models.TaskList;
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
        public async Task<IActionResult> GetById(string taskListId, string userId)
        {
            var result = await _taskListService.GetByIdAsync(taskListId, userId);

            return MapResponse(result, _mapper.Map<TaskList, TaskListDto>);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _taskListService.GetAll(userId, pageNumber, pageSize);

            return MapResponse(result, _mapper.Map<List<TaskListLiteDto>>);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskListRequest requestModel, string userId)
        {
            var taskList = _mapper.Map<TaskList>(requestModel);
            taskList.OwnerId = userId;
            taskList.LastUpdatedAt = DateTime.UtcNow;

            var result = await _taskListService.CreateAsync(taskList);

            return MapResponse(result, _mapper.Map<TaskList, TaskListDto>);
        }

        [HttpPut("{taskListId}")]
        public async Task<IActionResult> Update([FromBody] TaskListRequest requestModel, string userId, string taskListId)
        {
            var taskList = _mapper.Map<TaskList>(requestModel);
            taskList.OwnerId = userId;
            taskList.Id = taskListId;
            taskList.LastUpdatedAt = DateTime.UtcNow;

            var result = await _taskListService.UpdateAsync(taskList, userId);

            return MapResponse(result, _mapper.Map<TaskList, TaskListDto>);
        }

        [HttpDelete("{taskListId}")]
        public async Task<IActionResult> Delete(string taskListId, string userId)
        {
            var result = await _taskListService.DeleteAsync(taskListId, userId);

            return MapResponse(result);
        }

        [HttpGet("{taskListId}/accesses")]
        public async Task<IActionResult> GetAccesses(string taskListId, string userId)
        {
            var result = await _taskListService.GetUserAccessListAsync(taskListId, userId);

            return MapResponse(result, _mapper.Map<List<string>>);
        }

        [HttpPut("{taskListId}/accesses/{newUserAccessId}")]
        public async Task<IActionResult> AddAccess(string taskListId, string userId, string newUserAccessId)
        {
            var result = await _taskListService.AddUserAccessAsync(taskListId, userId, newUserAccessId);

            return MapResponse(result, _mapper.Map<TaskList, TaskListDto>);
        }

        [HttpDelete("{taskListId}/accesses/{userIdForRemove}")]
        public async Task<IActionResult> RemoveAccess(string taskListId, string userId, string userIdForRemove)
        {
            var result = await _taskListService.RemoveUserAccessAsync(taskListId, userId, userIdForRemove);

            return MapResponse(result, _mapper.Map<TaskList, TaskListDto>);
        }
    }
}

