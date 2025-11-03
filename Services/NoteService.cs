using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealTimeTaskManager.Data;
using RealTimeTaskManager.Entities;
using RealTimeTaskManager.Enums;
using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<NoteService> _logger;
        private readonly IActivityService _activityService;
        private readonly ITaskManagerHubService _hubService;

        public NoteService(ApplicationDbContext dbContext, IMapper mapper, ILogger<NoteService> logger, IActivityService activityService, ITaskManagerHubService hubService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _activityService = activityService;
            _hubService = hubService;
        }

        public async Task<NoteResponse> CreateNoteAsync(CreateNoteRequest request)
        {
            // Verify task exists and get task title
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);
            if (task == null)
                throw new ArgumentException($"Task with ID {request.TaskId} does not exist");

            var noteEntity = _mapper.Map<NoteEntity>(request);

            // Set properties that AutoMapper ignores
            noteEntity.Id = Guid.NewGuid();
            noteEntity.CreatedAt = DateTime.UtcNow;

            _dbContext.Notes.Add(noteEntity);
            await _dbContext.SaveChangesAsync();

            // Record activity
            await _activityService.RecordActivityAsync(
                ActivityActionEnum.NoteCreated,
                EntityTypeEnum.Note,
                noteEntity.Id,
                task.Title, // Use task title for context
                $"Note added: {(request.Content.Length > 50 ? request.Content.Substring(0, 50) : request.Content)}" // Truncate long content
            );

            var noteResponse = _mapper.Map<NoteResponse>(noteEntity);

            // Send SignalR notification
            var noteAddedMessage = new NoteAddedMessage
            {
                Id = noteEntity.Id,
                TaskId = noteEntity.TaskId,
                Content = noteEntity.Content,
                CreatedAt = noteEntity.CreatedAt
            };
            await _hubService.NotifyNoteAddedAsync(noteAddedMessage);

            _logger.LogInformation($"Note created for task: {task.Title}");

            return noteResponse;
        }

        public async Task<List<NoteResponse>> GetNotesByTaskIdAsync(Guid taskId)
        {
            var notes = await _dbContext.Notes
                .Where(n => n.TaskId == taskId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<NoteResponse>>(notes);
        }

        public async Task<NoteResponse?> GetNoteByIdAsync(Guid id)
        {
            var note = await _dbContext.Notes.FindAsync(id);
            return note == null ? null : _mapper.Map<NoteResponse>(note);
        }

        public async Task<NoteResponse> UpdateNoteAsync(Guid id, UpdateNoteRequest request)
        {
            var noteEntity = await _dbContext.Notes
                .Include(n => n.Task)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (noteEntity == null)
                throw new ArgumentException($"Note with ID {id} not found");

            noteEntity.Content = request.Content;
            await _dbContext.SaveChangesAsync();

            // Record activity
            await _activityService.RecordActivityAsync(
                ActivityActionEnum.NoteUpdated,
                EntityTypeEnum.Note,
                noteEntity.Id,
                noteEntity.Task?.Title ?? "Unknown Task",
                $"Note added: {(request.Content.Length > 50 ? request.Content.Substring(0, 50) : request.Content)}" // Truncate long content
            );

            var noteResponse = _mapper.Map<NoteResponse>(noteEntity);

            // Send SignalR notification
            var noteUpdatedMessage = new NoteUpdatedMessage
            {
                Id = noteEntity.Id,
                TaskId = noteEntity.TaskId,
                Content = noteEntity.Content,
                UpdatedAt = DateTime.UtcNow
            };
            await _hubService.NotifyNoteUpdatedAsync(noteUpdatedMessage);

            _logger.LogInformation($"Note updated: {noteEntity.Id}");

            return noteResponse;
        }

        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            var noteEntity = await _dbContext.Notes
                .Include(n => n.Task)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (noteEntity == null)
                return false;

            // Record activity BEFORE deletion
            await _activityService.RecordActivityAsync(
                ActivityActionEnum.NoteDeleted,
                EntityTypeEnum.Note,
                noteEntity.Id,
                noteEntity.Task?.Title ?? "Unknown Task"
            );

            // Send SignalR notification BEFORE deletion
            var noteDeletedMessage = new NoteDeletedMessage
            {
                Id = noteEntity.Id,
                TaskId = noteEntity.TaskId
            };

            _dbContext.Notes.Remove(noteEntity);
            await _dbContext.SaveChangesAsync();

            await _hubService.NotifyNoteDeletedAsync(noteDeletedMessage);

            _logger.LogInformation($"Note deleted: {id}");

            return true;
        }
    }
}
