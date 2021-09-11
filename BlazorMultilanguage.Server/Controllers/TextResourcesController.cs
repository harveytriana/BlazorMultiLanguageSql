using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorMultilanguage.Server.Models;
using BlazorMultilanguage.Server.Services;
using System;
using System.Diagnostics;
using IO = System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BlazorMultilanguage.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextResourcesController : ControllerBase
    {
        readonly TextResourcesContext _context;
        readonly ILogger<TextResourcesController> _logger;


        public TextResourcesController(
            TextResourcesContext context, 
            ILogger<TextResourcesController> logger) {
            _context = context;
            _logger = logger;
        }

        // GET: api/TextResources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextResource>>> GetTextResources() {
            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()} Request TextResource");
            return await _context.TextResources.ToListAsync();
        }

        // GET: api/TextResources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TextResource>> GetTextResource(string id) {
            var textResource = await _context.TextResources.FindAsync(id);

            if (textResource == null) {
                return NotFound();
            }

            return textResource;
        }

        // PUT: api/TextResources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTextResource(string id, TextResource textResource) {
            if (id != textResource.Id) {
                return BadRequest();
            }

            _context.Entry(textResource).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!TextResourceExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TextResources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TextResource>> PostTextResource(TextResource textResource) {
            _context.TextResources.Add(textResource);
            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                if (TextResourceExists(textResource.Id)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return CreatedAtAction("GetTextResource", new { id = textResource.Id }, textResource);
        }

        // DELETE: api/TextResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTextResource(string id) {
            var textResource = await _context.TextResources.FindAsync(id);
            if (textResource == null) {
                return NotFound();
            }

            _context.TextResources.Remove(textResource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TextResourceExists(string id) {
            return _context.TextResources.Any(e => e.Id == id);
        }

        [HttpGet("Import")]
        public async Task<string> GetLatest() {
            string result;
            try {

                //if (_context.TextResources.Any()) {
                //    Trace.WriteLine("The table TextResources is not empty.");
                //    return false;
                //}

                var file = @"C:\_study\Blazor\Intents\BlazorMultiLanguageSql\BlazorMultiLanguage\Resources\Languages.json";

                if (IO.File.Exists(file)) {
                    var js = IO.File.ReadAllText(file);
                    var data = JsonSerializer.Deserialize<List<TextResource>>(js);
                    foreach (var i in data) {
                        if (string.IsNullOrEmpty(i.Id) || TextResourceExists(i.Id)) {
                            continue;
                        }
                        Trace.WriteLine(i.ToString());
                        _context.TextResources.Add(i);
                        await _context.SaveChangesAsync();
                    }
                    result = "Update is done";
                } else {
                    result = "missing file";
                }
            } catch {
                result = "exception";
            }
            return result;
        }
    }
}
