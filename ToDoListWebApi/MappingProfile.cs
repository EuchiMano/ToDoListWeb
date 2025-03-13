using AutoMapper;
using ToDoListWebApi.Dtos;

namespace ToDoListWebApi;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateTaskDto, Models.Task>();
    CreateMap<UpdateTaskDto, Models.Task>();
  }
}
