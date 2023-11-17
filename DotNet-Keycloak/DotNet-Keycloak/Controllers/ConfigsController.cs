using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNet_Keycloak.Data;
using DotNet_Keycloak.Data.Model;

namespace DotNet_Keycloak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ConfigsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Configs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Config>>> GetConfig()
        {
          if (_context.Config == null)
          {
              return NotFound();
          }
            return await _context.Config.ToListAsync();
        }

        // GET: api/Configs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Config>> GetConfig(Guid id)
        {
          if (_context.Config == null)
          {
              return NotFound();
          }
            var config = await _context.Config.FindAsync(id);

            if (config == null)
            {
                return NotFound();
            }

            return config;
        }

        // PUT: api/Configs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfig(Guid id, Config config)
        {
            if (id != config.Id)
            {
                return BadRequest();
            }

            _context.Entry(config).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfigExists(id))
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

        // POST: api/Configs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Config>> PostConfig(Config config)
        {
          if (_context.Config == null)
          {
              return Problem("Entity set 'DatabaseContext.Config'  is null.");
          }
            _context.Config.Add(config);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfig", new { id = config.Id }, config);
        }

        // DELETE: api/Configs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfig(Guid id)
        {
            if (_context.Config == null)
            {
                return NotFound();
            }
            var config = await _context.Config.FindAsync(id);
            if (config == null)
            {
                return NotFound();
            }

            _context.Config.Remove(config);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfigExists(Guid id)
        {
            return (_context.Config?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
