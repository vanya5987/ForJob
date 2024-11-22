using Microsoft.EntityFrameworkCore;
using ShareYourText.Database;
using ShareYourText.Interfaces.Services;
using ShareYourText.Templates;

namespace ShareYourText.LinkChanges
{
    public sealed class LinkRemover : ILinkRemover
    {
        public async Task RemoveExpiredBaseLinksAsync()
        {
            await using (var context = new LinkDbContext())
            {
                DateTime now = DateTime.UtcNow;

                List<BaseLinkEntity> expiredLinks = await context.BaseLinks
                                            .Where(link => link.ExpirationDate < now)
                                            .ToListAsync();

                context.BaseLinks.RemoveRange(expiredLinks);

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveExpiredHashLinksAsync()
        {
            await using (LinkDbContext context = new LinkDbContext())
            {
                DateTime now = DateTime.UtcNow;
                List<HashLinkEntity> expiredLinks = await context.HashLinks
                                         .Where(link => link.ExpirationDate < now)
                                         .ToListAsync();

                context.HashLinks.RemoveRange(expiredLinks);
                await context.SaveChangesAsync();
            }
        }
    }
}

