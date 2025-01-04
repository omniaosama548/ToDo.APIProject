using AutoMapper;
using ToDo.API.DTOs;
using ToDo.API.Models;

namespace ToDo.API.Mapping
{
    public class TaskProfile:Profile
    {
        public TaskProfile()
        {
            CreateMap<DoTask, TaskDTO>()
                .ForMember(d => d.User, O => O.MapFrom(S => S.User.Username));
            CreateMap<DoTask, ReturnedTaskFromUpdateDTO>()
               .ForMember(d => d.User, O => O.MapFrom(S => S.User.Username));
        }
    }
}
