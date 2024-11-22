using ShareYourText.Interfaces.Database;

namespace ShareYourText.Database
{
    public sealed class DatabaseInitializator : IDatabaseInitializator
    {
        public async Task InitializeDatabaseAsync()
        {
            await using (LinkDbContext context = new LinkDbContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }
    }
}

