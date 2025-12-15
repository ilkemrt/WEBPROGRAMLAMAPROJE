using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Controllers.Api
{
    [ApiController]
    [Route("api/TrainersApi")]
    public class TrainersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainersApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trainers = await _context.Trainers
                .Include(t => t.Service)
                .Select(t => new
                {
                    t.Id,
                    FullName = t.FirstName + " " + t.LastName,
                    Service = t.Service.Name
                })
                .ToListAsync();

            return Ok(trainers);
        }


    }
}
