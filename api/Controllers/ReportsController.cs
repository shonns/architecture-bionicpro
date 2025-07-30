using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "ProtheticOnly")] // Используем политику
    public class ReportsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // Если токен невалиден или нет роли, сюда не попадём — будет 401
            var rng = new Random();
            var reports = new List<ReportDto>();
            for (int i = 1; i <= 5; i++)
            {
                reports.Add(new ReportDto
                {
                    Id = i,
                    Title = $"Report {i}",
                    CreatedAt = DateTime.UtcNow.AddDays(-rng.Next(0, 30)),
                    Value = rng.Next(100, 1000)
                });
            }
            return Ok(reports);
        }
    }

    public class ReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Value { get; set; }
    }



[ApiController]
[Route("debug")]
[AllowAnonymous]
public class DebugController : ControllerBase
{
    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Ok("User not authenticated");

        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }

    [HttpGet("check-role")]
    [Authorize(Roles = "prothetic_user")]
    public IActionResult CheckRole()
    {
        return Ok(new { hasRole = true });
    }
}

}
