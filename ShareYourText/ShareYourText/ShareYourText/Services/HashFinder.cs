using ShareYourText.Database;
using ShareYourText.Interfaces.LinkManger;
using ShareYourText.Interfaces.Services;
using ShareYourText.Templates;

namespace ShareYourText.Services
{
    public sealed class HashFinder : IHashFinder
    {
        public async Task<string> HashLinkFinderAsync(IGenerateLink generateLink)
        {
            await using (LinkDbContext context = new LinkDbContext())
            {
                string hashLink = generateLink.GenerateSHA256Hash();

                HashLinkEntity hashLinkEntity = context.HashLinks
                    .SingleOrDefault(link => link.LinkFormat == hashLink) ?? throw new Exception("Ссылка не найдена...");

                if (hashLinkEntity != null)
                    return generateLink.GenerateBaseLink();

                return null;
            }
        }
    }
}

