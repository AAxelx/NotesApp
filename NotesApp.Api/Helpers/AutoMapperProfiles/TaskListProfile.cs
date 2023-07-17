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
			CreateMap<TaskList, TaskListDto>();

            CreateMap<TaskListRequest, TaskList>();

            CreateMap<List<TaskList>, List<TaskListLiteDto>>()
            .ForMember(dest => dest, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest[0].Id, opt => opt.MapFrom(src => src[0].Id))
            .ProjectTo<List<TaskListLiteDto>>(MemberList.None);
        }
	}
}