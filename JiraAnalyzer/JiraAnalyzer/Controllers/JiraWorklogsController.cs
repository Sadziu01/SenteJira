using JiraAnalyzer.Data;
using JiraAnalyzer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JiraAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraWorklogsController : ControllerBase
    {
        private readonly DataContext _context;

        public JiraWorklogsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var worklogs = await _context.JiraWorklogs.Take(100).ToListAsync();

                Console.WriteLine("Odpowiedź: " + JsonConvert.SerializeObject(worklogs));
                return Ok(worklogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Wystąpił błąd serwera");
            }
        }


        [HttpGet]
        [Route("api/worklogs")]
        public async Task<IActionResult> GetWorklogs(string period, string author = null)
        {
            try
            {
                DateTime startDate;
                DateTime endDate = DateTime.Now; ;

                switch (period)
                {
                    case "year":
                        startDate = new DateTime(DateTime.Now.Year, 1, 1);
                        break;
                    case "month":
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        break;
                    case "week":
                        startDate = DateTime.Now.AddDays(-7);
                        break;
                    default:
                        return BadRequest("Nieprawidłowy okres");
                }

                var worklogsQuery = _context.JiraWorklogs
                    .Where(w => w.WorklogDate >= startDate && w.WorklogDate <= endDate);

                if (!string.IsNullOrEmpty(author))
                {
                    worklogsQuery = worklogsQuery.Where(w => w.Author == author);
                    Console.WriteLine(worklogsQuery);
                }

                var worklogs = await worklogsQuery.ToListAsync();

                if (!worklogs.Any())
                {
                    return NotFound("Brak danych dla podanego okresu i autora.");
                }

                var totalHours = worklogs.Sum(w => w.TimeSpent);
                var productiveHours = worklogs.Where(w => w.Qualification == "RP" || w.Qualification == "R").Sum(w => w.TimeSpent);
                var rpHours = worklogs.Where(w => w.Qualification == "RP").Sum(w => w.TimeSpent);
                var rHours = worklogs.Where(w => w.Qualification == "R").Sum(w => w.TimeSpent);
                var supportHours = worklogs.Where(w => w.Qualification == "HD").Sum(w => w.TimeSpent);
                var developmentHours = worklogs.Where(w => w.Qualification == "SZ").Sum(w => w.TimeSpent);
                var nonProductiveHours = worklogs.Where(w => w.Qualification == "W").Sum(w => w.TimeSpent);


                var statistics = new
                {
                    TotalHours = totalHours,
                    ProductiveHours = productiveHours,
                    RP_Hours = rpHours,
                    R_Hours = rHours,
                    SupportHours = supportHours,
                    DevelopmentHours = developmentHours,
                    NonProductiveHours = nonProductiveHours,
                    PercentageProductive = productiveHours / totalHours * 100,
                    PercentageSupport = supportHours / totalHours * 100,
                    PercentageDevelopment = developmentHours / totalHours * 100,
                    PercentageNonProductive = nonProductiveHours / totalHours * 100,
                    PercentageRP_Hours = rpHours / (rpHours + rHours) * 100,
                    PercentageR_Hours = rHours / (rpHours + rHours) * 100,
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/authors")]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                var authors = await _context.JiraWorklogs
                    .Select(w => w.Author)
                    .Distinct()
                    .ToListAsync();


                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }
    }
}
