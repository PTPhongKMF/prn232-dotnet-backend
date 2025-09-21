using MathslideLearning.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("sales-history")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetSalesHistory()
        {
            var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var salesHistory = await _dashboardService.GetSalesHistoryForTeacherAsync(teacherId);

            return Ok(salesHistory);
        }
    }
}
