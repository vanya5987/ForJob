using Google.Apis.Drive.v3;
using System.Diagnostics;

namespace ShareYourText
{
    public partial class ShareText : Form
    {
        private readonly IGetDriveService _getDriveService = new DriveServiceGetter();
        private readonly IShowUI _ui = new UI();
        private readonly IHashFinder _hashFinder = new HashFinder();
        private readonly IDatabaseInitializator _databaseInitializator = new DatabaseInitializator();
        private readonly IDatabaseLinkAdder _linkAdder = new DatabaseLinkAdder();
        private readonly IDatabaseLinkRemover _linkRemover = new DatabaseLinkRemover();
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

        private void InitializeProgram()
        {
            _driveService = _getDriveService.InitializeDriveService("C:\\Users\\admin\\Desktop\\credentials.json");
            _saveText = new GoogleDriveUpload(_driveService);
            _showText = new GoogleDriveUpload(_driveService);
            _extractFile = new GoogleDriveUpload(_driveService);

            string id = _saveText.SaveText(UserFileName.Text, $"{UserFileText.Text}");

            _generateLink = new LinkGenerator($"https://drive.google.com/file/d/{id}/view");
            _baseLink = new LinkCreator(_generateLink);
            _hashLink = new LinkCreator(_generateLink);

            _postController = new PostController(_hashFinder, _userInputGetter);
            _showFileText = new FileTextShowing(_hashFinder,_userInputGetter);
            _databaseInitializator.InitializeDatabase();

            _linkGetter = new LinkGetter(_baseLink, _hashLink);
            _linkRepositoryManager = new LinkRepositoryManager(_linkAdder, _linkRemover);

            _linkRepositoryManager.AddLink();
            _linkRepositoryManager.RemoveLink();

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
            _ui.ShowPopularityLinks(TopLinksList);
        }

        private void LikeClicked(object sender, EventArgs e)
        {
            _postController.LikePost();
        }

        private void DislikeClicked(object sender, EventArgs e)
        {
            _postController.DislikePost();
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

            _showFileText.ShowFileText(TextViever, _extractFile, _showText);
        }
    }
}
