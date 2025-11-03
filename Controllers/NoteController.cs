using Microsoft.AspNetCore.Mvc;
using RealTimeTaskManager.Models;
using RealTimeTaskManager.Services;

namespace RealTimeTaskManager.Controllers
{
    /// <summary>
    /// Controller for managing notes associated with tasks in the Real-Time Task Manager application
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly INoteService _noteService;

        public NoteController(ILogger<NoteController> logger, INoteService noteService)
        {
            _logger = logger;
            _noteService = noteService;
        }

        /// <summary>
        /// Retrieves all notes associated with a specific task
        /// </summary>
        /// <param name="id">The unique identifier of the task</param>
        /// <returns>A list of notes for the specified task</returns>
        /// <response code="200">Returns the list of notes for the task</response>
        /// <response code="400">If there was an error retrieving the notes</response>
        [HttpGet("task/{id}")]
        [ProducesResponseType(typeof(List<NoteResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<NoteResponse>>> GetNotesByTaskId(Guid id)
        {
            _logger.LogInformation($"Get notes by task id: {id}");

            try
            {
                var notes = await _noteService.GetNotesByTaskIdAsync(id);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notes by task id");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific note by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the note</param>
        /// <returns>The note with the specified ID</returns>
        /// <response code="200">Returns the requested note</response>
        /// <response code="404">If the note is not found</response>
        /// <response code="400">If there was an error retrieving the note</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NoteResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NoteResponse>> GetNoteById(Guid id)
        {
            _logger.LogInformation($"Get note by id: {id}");

            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                if (note == null)
                    return NotFound();

                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting note");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new note associated with a task
        /// </summary>
        /// <param name="noteRequest">The note creation request containing note details</param>
        /// <returns>The newly created note</returns>
        /// <response code="201">Returns the newly created note</response>
        /// <response code="400">If the request is invalid or null</response>
        [HttpPost]
        [ProducesResponseType(typeof(NoteResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NoteResponse>> CreateNote([FromBody] CreateNoteRequest noteRequest)
        {
            _logger.LogInformation("Creating a new note");

            if (noteRequest == null)
                return BadRequest("Note request cannot be null");

            var result = await _noteService.CreateNoteAsync(noteRequest);
            return CreatedAtAction(nameof(GetNoteById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing note with new information
        /// </summary>
        /// <param name="id">The unique identifier of the note to update</param>
        /// <param name="noteRequest">The note update request containing new note details</param>
        /// <returns>The updated note</returns>
        /// <response code="201">Returns the updated note</response>
        /// <response code="400">If the request is invalid or the note ID is empty</response>
        /// <response code="404">If the note is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NoteResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NoteResponse>> UpdateNoteByID(Guid id, [FromBody] UpdateNoteRequest noteRequest)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid note ID");

            if (noteRequest == null)
                return BadRequest("Note request cannot be null");

            var result = await _noteService.UpdateNoteAsync(id, noteRequest);
            if (result == null)
                return NotFound();

            return CreatedAtAction(nameof(GetNoteById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Deletes a note from the system
        /// </summary>
        /// <param name="id">The unique identifier of the note to delete</param>
        /// <returns>Success status of the deletion operation</returns>
        /// <response code="200">If the note was successfully deleted</response>
        /// <response code="400">If the note ID is invalid</response>
        /// <response code="404">If the note is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> DeleteNote(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid note ID");

            var isDeleted = await _noteService.DeleteNoteAsync(id);

            if (!isDeleted)
                return NotFound();

            return Ok();
        }
    }
}
