using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // Public endpoint (no login needed)
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("This is a public endpoint — no authentication needed.");
        }

        // Protected endpoint (any logged-in user)
        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok("This is a protected endpoint — valid JWT token required.");
        }

        // Admin-only endpoint
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("Hello Admin — you have access to this endpoint.");
        }

        // User-only endpoint
        [Authorize(Roles = "User,Admin")]
        [HttpGet("user-admin-only")]
        public IActionResult UserOnly()
        {
            return Ok("Hello User — you have access to this endpoint.");
        }
    }
}
