using System.Diagnostics;

namespace ShareYourText
{
    public partial class ShareText : Form
    {
        private readonly IDriveService _driveService = new GoogleDriveUpload("C:\\Users\\admin\\Desktop\\credentials.json");
        private readonly IShowUI _ui = new UI();

        private IGetId _getId;
        private ILinkHandler _linkHandler;
        private ICombinedLinkManager _linkManage;

        public ShareText()
        {
            InitializeComponent();
        }

        private void CreateFile_Click(object sender, EventArgs e)
        {
            string userFileName = UserFileName.Text;
            string userFileText = UserFileText.Text;
            string id = _driveService.SaveText(userFileName, $"{userFileText}");

            _getId = new FileCreater(id);
            _linkHandler = new LinkHandler(_driveService);

            _linkHandler.InitializeDatabase();

            LinkGetter getBaseLink = new LinkGetter(_getId);

            _linkManage = new LinkManager(_linkHandler);
            _linkManage.ManageLink();

            ShowLink();
            UIStateController();
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

        private void UserLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(UserLink.Text)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(processInfo);
        }

        private void ShowTop_Click(object sender, EventArgs e)
        {
            _ui.ShowPopularityLinksList(TopLinksList);
        }

        private void Like_Click(object sender, EventArgs e)
        {
            _linkHandler.LikePost();
        }

        private void Dislike_Click(object sender, EventArgs e)
        {
            _linkHandler.DislikePost();
        }

        private void ListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListViewItem item = TopLinksList.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    Clipboard.SetText(item.Text);
                    MessageBox.Show($"Скопировано в буфер обмена: {item.Text}");
                }
            }
        }

        private void VievText_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserInputLink.Text))
                throw new ArgumentNullException("Введите ссылку в поле для ссылки...");

            _linkHandler.ShowTextForLink(TextViever);
        }
    }
}
