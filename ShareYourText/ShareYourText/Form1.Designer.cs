namespace ShareYourText
{
    partial class ShareText
    {

        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            ListViewItem listViewItem1 = new ListViewItem("");
            ListViewItem listViewItem2 = new ListViewItem("");
            ListViewItem listViewItem3 = new ListViewItem("");
            ListViewItem listViewItem4 = new ListViewItem("");
            ListViewItem listViewItem5 = new ListViewItem("");
            ListViewItem listViewItem6 = new ListViewItem("");
            ListViewItem listViewItem7 = new ListViewItem("");
            ListViewItem listViewItem8 = new ListViewItem("");
            ListViewItem listViewItem9 = new ListViewItem("");
            ListViewItem listViewItem10 = new ListViewItem("");
            TopLinksList = new ListView();
            Link = new ColumnHeader();
            ExpirationDate = new ColumnHeader();
            PopularityPoint = new ColumnHeader();
            UserFileName = new TextBox();
            UserFileText = new TextBox();
            CreateFile = new Button();
            UserFileTextLabel = new Label();
            UserFileNameLabel = new Label();
            UserLinkLabel = new Label();
            TopLinksListLabel = new Label();
            UserLink = new LinkLabel();
            ShowTop = new Button();
            Evaluator = new Label();
            UserInputLink = new TextBox();
            Like = new Button();
            Dislike = new Button();
            UserInformer = new Label();
            TextViever = new TextBox();
            VievText = new Button();
            SuspendLayout();
            // 
            // TopLinksList
            // 
            TopLinksList.Columns.AddRange(new ColumnHeader[] { Link, ExpirationDate, PopularityPoint });
            TopLinksList.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8, listViewItem9, listViewItem10 });
            TopLinksList.Location = new Point(402, 165);
            TopLinksList.Name = "TopLinksList";
            TopLinksList.Size = new Size(404, 269);
            TopLinksList.TabIndex = 0;
            TopLinksList.UseCompatibleStateImageBehavior = false;
            TopLinksList.View = View.Details;
            TopLinksList.MouseClick += ListVievClicked;
            // 
            // Link
            // 
            Link.Text = "Самые популярные ";
            Link.Width = 170;
            // 
            // ExpirationDate
            // 
            ExpirationDate.Text = "Дата действия";
            ExpirationDate.Width = 150;
            // 
            // PopularityPoint
            // 
            PopularityPoint.Text = "Популярность";
            // 
            // UserFileName
            // 
            UserFileName.Location = new Point(39, 81);
            UserFileName.Name = "UserFileName";
            UserFileName.Size = new Size(362, 27);
            UserFileName.TabIndex = 11;
            // 
            // UserFileText
            // 
            UserFileText.Location = new Point(34, 165);
            UserFileText.Multiline = true;
            UserFileText.Name = "UserFileText";
            UserFileText.Size = new Size(362, 269);
            UserFileText.TabIndex = 12;
            // 
            // CreateFile
            // 
            CreateFile.Location = new Point(140, 456);
            CreateFile.Name = "CreateFile";
            CreateFile.Size = new Size(166, 31);
            CreateFile.TabIndex = 13;
            CreateFile.Text = "Создать файл";
            CreateFile.UseVisualStyleBackColor = true;
            CreateFile.Click += CreateFile_Click;
            // 
            // UserFileTextLabel
            // 
            UserFileTextLabel.AutoSize = true;
            UserFileTextLabel.Location = new Point(34, 142);
            UserFileTextLabel.Name = "UserFileTextLabel";
            UserFileTextLabel.Size = new Size(110, 20);
            UserFileTextLabel.TabIndex = 14;
            UserFileTextLabel.Text = "Введите текст :";
            // 
            // UserFileNameLabel
            // 
            UserFileNameLabel.AutoSize = true;
            UserFileNameLabel.Location = new Point(34, 58);
            UserFileNameLabel.Name = "UserFileNameLabel";
            UserFileNameLabel.Size = new Size(189, 20);
            UserFileNameLabel.TabIndex = 15;
            UserFileNameLabel.Text = "Введите название файла :";
            // 
            // UserLinkLabel
            // 
            UserLinkLabel.AutoSize = true;
            UserLinkLabel.Location = new Point(453, 58);
            UserLinkLabel.Name = "UserLinkLabel";
            UserLinkLabel.Size = new Size(105, 20);
            UserLinkLabel.TabIndex = 16;
            UserLinkLabel.Text = "Ваша ссылка :";
            // 
            // TopLinksListLabel
            // 
            TopLinksListLabel.AutoSize = true;
            TopLinksListLabel.Location = new Point(511, 142);
            TopLinksListLabel.Name = "TopLinksListLabel";
            TopLinksListLabel.Size = new Size(205, 20);
            TopLinksListLabel.TabIndex = 17;
            TopLinksListLabel.Text = "Топ 10 популярных ссылок :";
            // 
            // UserLink
            // 
            UserLink.Location = new Point(453, 81);
            UserLink.Name = "UserLink";
            UserLink.Size = new Size(353, 25);
            UserLink.TabIndex = 18;
            UserLink.TabStop = true;
            UserLink.Text = "Ваша ссылка появиться после создания файла...\r\n";
            UserLink.LinkClicked += UserLinkClicked;
            // 
            // ShowTop
            // 
            ShowTop.Location = new Point(482, 456);
            ShowTop.Name = "ShowTop";
            ShowTop.Size = new Size(253, 29);
            ShowTop.TabIndex = 19;
            ShowTop.Text = "Показать топ";
            ShowTop.UseVisualStyleBackColor = true;
            ShowTop.Click += ShowTopClicked;
            // 
            // Evaluator
            // 
            Evaluator.AutoSize = true;
            Evaluator.Location = new Point(39, 532);
            Evaluator.Name = "Evaluator";
            Evaluator.Size = new Size(127, 20);
            Evaluator.TabIndex = 20;
            Evaluator.Text = "Оценить ссылку :";
            // 
            // UserInputLink
            // 
            UserInputLink.Enabled = false;
            UserInputLink.Location = new Point(172, 532);
            UserInputLink.Name = "UserInputLink";
            UserInputLink.Size = new Size(364, 27);
            UserInputLink.TabIndex = 21;
            // 
            // Like
            // 
            Like.Enabled = false;
            Like.Location = new Point(542, 530);
            Like.Name = "Like";
            Like.Size = new Size(66, 29);
            Like.TabIndex = 22;
            Like.Text = "Лайк";
            Like.UseVisualStyleBackColor = true;
            Like.Click += LikeClicked;
            // 
            // Dislike
            // 
            Dislike.Enabled = false;
            Dislike.Location = new Point(611, 530);
            Dislike.Name = "Dislike";
            Dislike.Size = new Size(71, 29);
            Dislike.TabIndex = 23;
            Dislike.Text = "Диз";
            Dislike.UseVisualStyleBackColor = true;
            Dislike.Click += DislikeClicked;
            // 
            // UserInformer
            // 
            UserInformer.AutoSize = true;
            UserInformer.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            UserInformer.Location = new Point(172, 512);
            UserInformer.Name = "UserInformer";
            UserInformer.Size = new Size(344, 17);
            UserInformer.TabIndex = 24;
            UserInformer.Text = "Оценка ссылок станет доступна после создания файла...";
            // 
            // TextViever
            // 
            TextViever.Enabled = false;
            TextViever.ForeColor = Color.White;
            TextViever.Location = new Point(825, 58);
            TextViever.Multiline = true;
            TextViever.Name = "TextViever";
            TextViever.Size = new Size(422, 444);
            TextViever.TabIndex = 25;
            // 
            // VievText
            // 
            VievText.Location = new Point(932, 528);
            VievText.Name = "VievText";
            VievText.Size = new Size(223, 29);
            VievText.TabIndex = 26;
            VievText.Text = "Показать текст по ссылке";
            VievText.UseVisualStyleBackColor = true;
            VievText.Click += VievTextClicked;
            // 
            // ShareText
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1259, 571);
            Controls.Add(VievText);
            Controls.Add(TextViever);
            Controls.Add(UserInformer);
            Controls.Add(Dislike);
            Controls.Add(Like);
            Controls.Add(UserInputLink);
            Controls.Add(Evaluator);
            Controls.Add(ShowTop);
            Controls.Add(UserLink);
            Controls.Add(TopLinksListLabel);
            Controls.Add(UserLinkLabel);
            Controls.Add(UserFileNameLabel);
            Controls.Add(UserFileTextLabel);
            Controls.Add(CreateFile);
            Controls.Add(UserFileText);
            Controls.Add(UserFileName);
            Controls.Add(TopLinksList);
            Name = "ShareText";
            Text = "ShareYourText";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label UserFileTextLabel;
        private Label UserFileNameLabel;
        private Label UserLinkLabel;
        private Label TopLinksListLabel;
        public ListView TopLinksList;
        public TextBox UserFileName;
        public TextBox UserFileText;
        public Button CreateFile;
        public LinkLabel UserLink;
        private Button ShowTop;
        public ColumnHeader Link;
        public ColumnHeader ExpirationDate;
        public ColumnHeader PopularityPoint;
        private Label Evaluator;
        public TextBox UserInputLink;
        public Button Like;
        public Button Dislike;
        private Label UserInformer;
        private TextBox TextViever;
        private Button VievText;
    }
}
