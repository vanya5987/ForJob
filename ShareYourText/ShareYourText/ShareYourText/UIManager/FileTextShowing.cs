using ShareYourText.Database;
using ShareYourText.Interfaces.GoogleServices;
using ShareYourText.Interfaces.Services;
using ShareYourText.Interfaces.UIManager;
using ShareYourText.Interfaces.UserInteraction;

namespace ShareYourText.UIManager
{
    public sealed class FileTextShowing : IShowFileText
    {
        private readonly IHashFinder _hashFinder;
        private readonly IUserInputGetter _userInput;

        public FileTextShowing(IHashFinder hashFinder, IUserInputGetter userInput)
        {
            _hashFinder = hashFinder ?? throw new ArgumentNullException(nameof(_hashFinder));
            _userInput = userInput ?? throw new ArgumentNullException(nameof(_userInput));
        }

        public async Task ShowFileTextAsync(TextBox textBox, IDriveExtractFile extarctText, IDriveTextShowing textShowing)
        {
            await using (LinkDbContext context = new LinkDbContext())
            {
                string userLink = await _userInput.GetUserInputLinkAsync(_hashFinder);

                var linkEntity = context.BaseLinks
                            .SingleOrDefault(entity => entity.LinkFormat == userLink);

                if (linkEntity == null)
                    throw new Exception("Ссылка отсутствует или была удалена...");

                string userFileId = extarctText.ExtractFileId(linkEntity.LinkFormat);

                textBox.Text = await textShowing.ShowTextAsync(userFileId);
            }
        }
    }
}

