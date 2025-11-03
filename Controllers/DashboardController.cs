using Microsoft.AspNetCore.Mvc;
using RealTimeTaskManager.Models;
using RealTimeTaskManager.Services;

namespace RealTimeTaskManager.Controllers
{
    /// <summary>
    /// Controller for providing dashboard data and activity information in the Real-Time Task Manager application
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IActivityService _activityService;

        public DashboardController(ILogger<DashboardController> logger, IActivityService activityService)
        {
            _logger = logger;
            _activityService = activityService;
        }

        /// <summary>
        /// Retrieves comprehensive dashboard data including statistics and metrics
        /// </summary>
        /// <returns>Dashboard data containing task statistics and system metrics</returns>
        /// <response code="200">Returns the dashboard data with current statistics</response>
        /// <response code="400">If there was an error retrieving the dashboard data</response>
        [HttpGet]
        [ProducesResponseType(typeof(DashboardResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DashboardResponse>> GetDashboardData()
        {
            _logger.LogInformation("Getting dashboard data");
            try
            {
                var dashboardData = await _activityService.GetDashboardDataAsync();
                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves recent activities from the system with a specified limit
        /// </summary>
        /// <param name="count">The number of recent activities to retrieve (max 100, default 20)</param>
        /// <returns>A list of recent activities</returns>
        /// <response code="200">Returns the list of recent activities</response>
        /// <response code="400">If there was an error retrieving the activities</response>
        [HttpGet("{count}")]
        [ProducesResponseType(typeof(List<ActivityResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<ActivityResponse>>> GetRecentActivities(int count)
        {
            _logger.LogInformation("Getting recent activities");
            try
            {
                var _count = count > 0 && count < 100 ? Math.Min(count, 100) : 20;
                var activities = await _activityService.GetRecentActivitiesAsync(_count);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent activities");
                return BadRequest(ex.Message);
            }
        }
    }
}
