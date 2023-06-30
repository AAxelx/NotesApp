using AutoMapper;
using NotesApp.Api.Models.Requests.TaskList;
using NotesApp.Api.Models.TaskList;
using NotesApp.DAL.DataAccess.Models;

namespace NotesApp.Api.Helpers.AutoMapperProfiles
{
	public class TaskListProfile : Profile
	{
		public TaskListProfile()
		{
            CreateMap<TaskListDto, TaskList>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

            CreateMap<CreateTaskListDto, TaskListDto>();

            CreateMap<UpdateTaskListDto, TaskListDto>();
        }
	}
}