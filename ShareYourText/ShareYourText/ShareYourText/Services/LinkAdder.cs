using ShareYourText.Database;
using ShareYourText.Interfaces.Services;
using ShareYourText.Templates;

namespace ShareYourText.LinkChanges
{
    public sealed class LinkAdder : ILinkAdder
    {
        public async Task AddBaseLinkAsync()
        {
            await using (LinkDbContext context = new LinkDbContext())
            {
                BaseLinkEntity linkEntity = new BaseLinkEntity();

                context.BaseLinks.Add(linkEntity);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddHashLinkAsync()
        {
            await using (LinkDbContext context = new LinkDbContext())
            {
                HashLinkEntity hashLinkEntity = new HashLinkEntity();

                context.HashLinks.Add(hashLinkEntity);
                await context.SaveChangesAsync();
            }
        }
    }
}

