using LocationsAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocationsAPI;
using Microsoft.AspNetCore.Authorization;

namespace LocationsAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly LocationContext _dbContext;

        public LocationsController(LocationContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            if (_dbContext.Locations == null)
            {
                return NotFound();
            }

            return await _dbContext.Locations.ToListAsync();
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            if (_dbContext.Locations == null)
            {
                return NotFound();
            }

            var location = await _dbContext.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        [HttpGet("pagination/{page}")]
        public async Task<IActionResult> GetLocationsPagination(int page)
        {
            int locationsPerPage = 3;
            int totalLocations = await _dbContext.Locations.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalLocations / locationsPerPage);

            if (page > totalPages)
            {
                return NotFound();
            }

            var locations = await _dbContext.Locations
                .Skip((page - 1) * locationsPerPage)
                .Take(locationsPerPage)
                .ToListAsync();

            return Ok(new { locations, totalPages });
        }


        // POST: api/Locations
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
        }

        // PUT: api/Locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(location).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            if (_dbContext.Locations == null)
            {
                return NotFound();
            }

            var location = await _dbContext.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _dbContext.Locations.Remove(location);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationExists(long id)
        {
            return (_dbContext.Locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}