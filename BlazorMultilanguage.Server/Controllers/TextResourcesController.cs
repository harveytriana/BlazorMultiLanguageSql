/*
BlazorMultiLanguage :: TextResourcesController
blazorspread.net
*/
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorMultilanguage.Server.Models;
using BlazorMultilanguage.Server.Services;
using System;
using IO = System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Text;

namespace BlazorMultilanguage.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextResourcesController : ControllerBase
    {
        readonly TextResourcesContext _context;

        public TextResourcesController(TextResourcesContext context) {
            _context = context;
        }

        [HttpGet("GetCulture/{culture}")]
        public async Task<Dictionary<string, string>> GetLanguage(string culture) {
            var ls = await _context.TextResources
                .Where(x => x.Id != "")
                .Select(x => new { x.Id, Text = x.Get(culture) })
                .ToDictionaryAsync(x => x.Id, x => x.Text);
            return ls;
        }

        /// <summary>
        /// Using reflection, reads the signature and filters for those that have content
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCultures")]
        public async Task<string[]> GetCultures() {
            var f = await _context
                .TextResources
                .OrderBy(X => X.Id)
                .FirstOrDefaultAsync();
            try {
                var ls = new List<string>();
                foreach (var p in typeof(TextResource).GetProperties()) {
                    if (p.Name != "Id") {
                        if (string.IsNullOrEmpty(f.Get(p.Name))) {
                            continue;
                        }
                        ls.Add(p.Name);
                    }
                }
                return ls.ToArray();
            } catch {
                return Array.Empty<string>();
            }
        }
    }
}
