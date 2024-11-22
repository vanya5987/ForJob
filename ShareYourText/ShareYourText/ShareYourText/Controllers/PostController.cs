using ShareYourText.Database;
using ShareYourText.Interfaces.Controllers;
using ShareYourText.Interfaces.Services;
using ShareYourText.Interfaces.UserInteraction;

namespace ShareYourText.Controllers
{
    public sealed class PostController : IPostController
    {
        private readonly IHashFinder _hashFinder;
        private readonly IUserInputGetter _userInput;

        public PostController(IHashFinder hashFinder, IUserInputGetter userInput)
        {
            _hashFinder = hashFinder ?? throw new ArgumentNullException(nameof(_hashFinder));
            _userInput = userInput ?? throw new ArgumentNullException(nameof(_userInput));
        }

        public async Task LikePostAsync()
        {
            await AddPointAsync(+1);
        }

        public async Task DislikePostAsync()
        {
            await AddPointAsync(-1);
        }

        private async Task AddPointAsync(int point)
        {
            await using (LinkDbContext context = new LinkDbContext())
            {
                string userLink = await _userInput.GetUserInputLinkAsync(_hashFinder);

                var linkEntity = context.BaseLinks
                   .SingleOrDefault(entity => entity.LinkFormat == userLink);

                if (linkEntity == null)
                    throw new Exception("Ссылка отсутствует или была удалена...");

                linkEntity.PopularityPoint += point;
                await context.SaveChangesAsync();
            }
        }
    }
}

