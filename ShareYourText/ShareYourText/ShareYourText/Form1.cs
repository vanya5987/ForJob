using Google.Apis.Drive.v3;
using ShareYourText.GoogleServices;
using ShareYourText.LinkManager;
using ShareYourText.Services;
using System.Diagnostics;
using ShareYourText.Controllers;
using ShareYourText.LinkChanges;
using ShareYourText.UIManager;
using ShareYourText.Database;
using ShareYourText.UserInteraction;
using ShareYourText.Interfaces.UIManager;
using ShareYourText.Interfaces.LinkManger;
using ShareYourText.Interfaces.Database;
using ShareYourText.Interfaces.Services;
using ShareYourText.Interfaces.GoogleServices;
using ShareYourText.Interfaces.Controllers;
using ShareYourText.Interfaces.UserInteraction;

namespace ShareYourText
{
    public partial class ShareText : Form
    {
        private readonly IGetDriveService _getDriveService = new DriveServiceGetter();
        private readonly IShowUI _ui = new UI();
        private readonly IHashFinder _hashFinder = new HashFinder();
        private readonly IDatabaseInitializator _databaseInitializator = new DatabaseInitializator();
        private readonly ILinkAdder _linkAdder = new LinkAdder();
        private readonly ILinkRemover _linkRemover = new LinkRemover();
        private readonly IUserInputGetter _userInputGetter = new UserInputGetter();

        private IShowFileText _showFileText;
        private DriveService? _driveService;
        private IDriveTextSaver? _saveText;
        private IDriveTextShowing? _showText;
        private IDriveExtractFile? _extractFile;
        private IPostController _postController;
        private IGenerateLink? _generateLink;
        private ILinkRepositoryManager? _linkRepositoryManager;
        private ICreateLink? _baseLink;
        private ICreateLink? _hashLink;
        private LinkGetter? _linkGetter;

        public ShareText()
        {
            InitializeComponent();
        }

        private async void InitializeProgram()
        {
            _driveService = await _getDriveService.InitializeDriveServiceAsync();
            _saveText = new GoogleDriveUpload(_driveService);
            _showText = new GoogleDriveUpload(_driveService);
            _extractFile = new GoogleDriveUpload(_driveService);

            string id = _saveText.SaveText(UserFileName.Text, $"{UserFileText.Text}");

            _generateLink = new LinkGenerator($"https://drive.google.com/file/d/{id}/view");
            _baseLink = new LinkCreator(_generateLink);
            _hashLink = new LinkCreator(_generateLink);

            _postController = new PostController(_hashFinder, _userInputGetter);
            _showFileText = new FileTextShowing(_hashFinder, _userInputGetter);
            await _databaseInitializator.InitializeDatabaseAsync();

            _linkGetter = new LinkGetter(_baseLink, _hashLink);
            _linkRepositoryManager = new LinkRepositoryManager(_linkAdder, _linkRemover);

            await _linkRepositoryManager.AddLinkAsync();
            await _linkRepositoryManager.RemoveLinkAsync();

            ShowLink();
            UIStateController();
        }

        private void CreateFile_Click(object sender, EventArgs e)
        {
            InitializeProgram();
        }

        private void ShowLink()
        {
            UserLink.Text = LinkGetter.GetBaseLink();
            UserInformer.Text = "Теперь вы можете оценить любую доступную ссылку =)";
        }

        private void UIStateController()
        {
            UserInputLink.Enabled = true;
            Like.Enabled = true;
            Dislike.Enabled = true;
        }

        private void UserLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(UserLink.Text)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(processInfo);
        }

        private void ShowTopClicked(object sender, EventArgs e)
        {
            _ui.ShowPopularityLinksAsync(TopLinksList);
        }

        private void LikeClicked(object sender, EventArgs e)
        {
            _postController.LikePostAsync();
        }

        private void DislikeClicked(object sender, EventArgs e)
        {
            _postController.DislikePostAsync();
        }

        private void ListVievClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListViewItem item = TopLinksList.GetItemAt(e.X, e.Y) ?? throw new Exception("Ссылки не найдены...");

                if (item != null)
                {
                    Clipboard.SetText(item.Text);
                    MessageBox.Show($"Скопировано в буфер обмена: {item.Text}");
                }
            }
        }

        private void VievTextClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserInputLink.Text))
                throw new ArgumentNullException("Введите ссылку в поле для ссылки...");

            _showFileText.ShowFileTextAsync(TextViever, _extractFile, _showText);
        }
    }
}
