using Microsoft.EntityFrameworkCore;
using ShareYourText.Templates;
using ShareYourText.GetPath;

namespace ShareYourText.Database
{
    public sealed class LinkDbContext : DbContext
    {
        public DbSet<BaseLinkEntity> BaseLinks { get; set; }

        public DbSet<HashLinkEntity> HashLinks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath.GetPath("links")}");
        }
    }
}

