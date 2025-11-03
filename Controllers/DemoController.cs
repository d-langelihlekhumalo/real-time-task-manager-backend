using Microsoft.AspNetCore.Mvc;
using RealTimeTaskManager.Services;

namespace RealTimeTaskManager.Controllers
{
    /// <summary>
    /// Controller for managing demo data and demo-specific operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger;
        private readonly IDataSeedingService _dataSeedingService;

        public DemoController(ILogger<DemoController> logger, IDataSeedingService dataSeedingService)
        {
            _logger = logger;
            _dataSeedingService = dataSeedingService;
        }

        /// <summary>
        /// Seeds the database with demo data if it's empty
        /// </summary>
        /// <returns>Success status of the seeding operation</returns>
        /// <response code="200">If the demo data was seeded successfully</response>
        /// <response code="400">If there was an error during seeding</response>
        [HttpPost("seed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SeedDemoData()
        {
            _logger.LogInformation("Demo data seeding requested");

            try
            {
                await _dataSeedingService.SeedDemoDataAsync();
                return Ok(new { message = "Demo data seeded successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding demo data");
                return BadRequest(new { message = "Error seeding demo data", error = ex.Message });
            }
        }

        /// <summary>
        /// Resets all data in the database and seeds fresh demo data
        /// </summary>
        /// <returns>Success status of the reset operation</returns>
        /// <response code="200">If the demo data was reset successfully</response>
        /// <response code="400">If there was an error during reset</response>
        [HttpPost("reset")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetDemoData()
        {
            _logger.LogInformation("Demo data reset requested");

            try
            {
                await _dataSeedingService.ResetDemoDataAsync();
                return Ok(new { message = "Demo data reset successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting demo data");
                return BadRequest(new { message = "Error resetting demo data", error = ex.Message });
            }
        }

        /// <summary>
        /// Gets information about the current demo setup
        /// </summary>
        /// <returns>Demo information and statistics</returns>
        /// <response code="200">Returns demo information</response>
        [HttpGet("info")]
        [ProducesResponseType(200)]
        public IActionResult GetDemoInfo()
        {
            var demoInfo = new
            {
                purpose = "SignalR Real-Time Task Management Demo",
                features = new[]
                {
                    "Real-time task creation and updates",
                    "Live note collaboration",
                    "Instant dashboard statistics",
                    "Multi-client synchronization",
                    "Activity feed updates"
                },
                instructions = new[]
                {
                    "Open multiple browser tabs to see real-time updates",
                    "Create tasks and notes in one tab, watch them appear in others",
                    "Toggle task completion to see dashboard statistics update",
                    "Check the dashboard for live activity feed",
                    "Use /api/demo/reset to clear data and start fresh"
                },
                endpoints = new
                {
                    seed = "POST /api/demo/seed - Add demo data if database is empty",
                    reset = "POST /api/demo/reset - Clear all data and reseed",
                    info = "GET /api/demo/info - This information"
                }
            };

            return Ok(demoInfo);
        }
    }
}