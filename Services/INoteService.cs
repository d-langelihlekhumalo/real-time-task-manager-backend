using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public interface INoteService
    {
        Task<NoteResponse> CreateNoteAsync(CreateNoteRequest request);
        Task<List<NoteResponse>> GetNotesByTaskIdAsync(Guid taskId);
        Task<NoteResponse?> GetNoteByIdAsync(Guid id);
        Task<NoteResponse> UpdateNoteAsync(Guid id, UpdateNoteRequest request);
        Task<bool> DeleteNoteAsync(Guid id);
    }
}
