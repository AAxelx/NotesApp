using AutoMapper;
using NotesApp.Api.Models.Requests.TaskList;
using NotesApp.Api.Models.Responses.TaskList;
using NotesApp.Api.Models.TaskList;
using NotesApp.DAL.DataAccess.Models;

namespace NotesApp.Api.Helpers.AutoMapperProfiles
{
	public class TaskListProfile : Profile
	{
		public TaskListProfile()
		{
			CreateMap<TaskListDto, TaskListResponse>();

			CreateMap<TaskListDto, TaskListsResponse>();

			CreateMap<TaskListDto, TaskListAccessesResponse>();

            CreateMap<TaskListRequest, TaskListDto>();
        }
	}
}