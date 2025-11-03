using AutoMapper;
using RealTimeTaskManager.Entities;
using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity to Response mapping
            CreateMap<TaskEntity, TaskResponse>();
            CreateMap<NoteEntity, NoteResponse>();
            CreateMap<ActivityEntity, ActivityResponse>();

            // Request to Entity mapping
            CreateMap<CreateTaskRequest, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore());

            CreateMap<UpdateTaskRequest, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore());

            CreateMap<CreateNoteRequest, NoteEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Task, opt => opt.Ignore());
        }
    }
}
