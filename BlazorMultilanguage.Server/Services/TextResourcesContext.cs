using BlazorMultilanguage.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorMultilanguage.Server.Services
{
    public class TextResourcesContext : DbContext
    {
        public TextResourcesContext() {
        }

        public TextResourcesContext(DbContextOptions<TextResourcesContext> options)
            : base(options) {
        }

        public virtual DbSet<TextResource> TextResources { get; set; }
    }
}
