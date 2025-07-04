using AutoMapper;
using TaskListManager.Data.ViewModel;

namespace TaskListManager.API.MappingConfigurations
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<TaskModel, TaskViewModel>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.Parse(src.StartDate)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus ? "CLOSED" : "PENDING"))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateTime.Parse(src.StartDate).AddDays(src.ElapsedTime)))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTime.Parse(src.StartDate).AddDays(src.AllotedTime)))
            .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.TaskStatus ? 0 : Math.Max(0, src.ElapsedTime - src.AllotedTime)))
            .ForMember(dest => dest.DaysLate, opt => opt.MapFrom(src => src.TaskStatus ? Math.Max(0, src.AllotedTime - src.ElapsedTime) : 0));

            CreateMap<CreateTaskDto, TaskModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("o")))
                .ForMember(dest => dest.ElapsedTime, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow.ToString("o")));

            CreateMap<UpdateTaskDto, TaskModel>()
                .ForMember(dest => dest.TaskName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.TaskName)))
                .ForMember(dest => dest.TaskDescription, opt => opt.Condition(src => !string.IsNullOrEmpty(src.TaskDescription)));
        }
    }
}
