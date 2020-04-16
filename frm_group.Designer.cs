namespace Poseidon
{
    partial class frm_group
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_group));
            this.skToolMenu = new CCWin.SkinControl.SkinToolStrip();
            this.toolFont = new System.Windows.Forms.ToolStripButton();
            this.toolImgFile = new System.Windows.Forms.ToolStripButton();
            this.toolfile = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripSplitButton();
            this.rtxt_send = new CCWin.SkinControl.SkinChatRichTextBox();
            this.rtxt_message = new CCWin.SkinControl.SkinChatRichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.clb_member = new CCWin.SkinControl.ChatListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.mnu_strip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_quit_group = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_invite_group = new System.Windows.Forms.ToolStripMenuItem();
            this.pgb_download = new System.Windows.Forms.ProgressBar();
            this.pgb_upload = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.skToolMenu.SuspendLayout();
            this.mnu_strip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skToolMenu
            // 
            this.skToolMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skToolMenu.Arrow = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.skToolMenu.AutoSize = false;
            this.skToolMenu.Back = System.Drawing.Color.White;
            this.skToolMenu.BackColor = System.Drawing.Color.Transparent;
            this.skToolMenu.BackRadius = 4;
            this.skToolMenu.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.skToolMenu.Base = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skToolMenu.BaseFore = System.Drawing.Color.Black;
            this.skToolMenu.BaseForeAnamorphosis = false;
            this.skToolMenu.BaseForeAnamorphosisBorder = 4;
            this.skToolMenu.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skToolMenu.BaseForeOffset = new System.Drawing.Point(0, 0);
            this.skToolMenu.BaseHoverFore = System.Drawing.Color.Black;
            this.skToolMenu.BaseItemAnamorphosis = true;
            this.skToolMenu.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.skToolMenu.BaseItemBorderShow = true;
            this.skToolMenu.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skToolMenu.BaseItemDown")));
            this.skToolMenu.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skToolMenu.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skToolMenu.BaseItemMouse")));
            this.skToolMenu.BaseItemNorml = null;
            this.skToolMenu.BaseItemPressed = System.Drawing.Color.Transparent;
            this.skToolMenu.BaseItemRadius = 2;
            this.skToolMenu.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skToolMenu.BaseItemSplitter = System.Drawing.Color.Transparent;
            this.skToolMenu.BindTabControl = null;
            this.skToolMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.skToolMenu.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skToolMenu.Fore = System.Drawing.Color.Black;
            this.skToolMenu.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.skToolMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.skToolMenu.HoverFore = System.Drawing.Color.White;
            this.skToolMenu.ItemAnamorphosis = false;
            this.skToolMenu.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skToolMenu.ItemBorderShow = false;
            this.skToolMenu.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skToolMenu.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skToolMenu.ItemRadius = 3;
            this.skToolMenu.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFont,
            this.toolImgFile,
            this.toolfile,
            this.toolStripDropDownButton4});
            this.skToolMenu.Location = new System.Drawing.Point(0, 463);
            this.skToolMenu.Name = "skToolMenu";
            this.skToolMenu.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skToolMenu.Size = new System.Drawing.Size(456, 22);
            this.skToolMenu.SkinAllColor = true;
            this.skToolMenu.TabIndex = 125;
            this.skToolMenu.Text = "skinToolStrip1";
            this.skToolMenu.TitleAnamorphosis = false;
            this.skToolMenu.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skToolMenu.TitleRadius = 4;
            this.skToolMenu.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolFont
            // 
            this.toolFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFont.Image = ((System.Drawing.Image)(resources.GetObject("toolFont.Image")));
            this.toolFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFont.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
            this.toolFont.Name = "toolFont";
            this.toolFont.Size = new System.Drawing.Size(23, 19);
            this.toolFont.Text = "toolStripButton1";
            this.toolFont.ToolTipText = "字体选择工具栏";
            this.toolFont.Click += new System.EventHandler(this.toolFont_Click);
            // 
            // toolImgFile
            // 
            this.toolImgFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolImgFile.Image = ((System.Drawing.Image)(resources.GetObject("toolImgFile.Image")));
            this.toolImgFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolImgFile.Name = "toolImgFile";
            this.toolImgFile.Size = new System.Drawing.Size(23, 19);
            this.toolImgFile.Text = "toolStripButton7";
            this.toolImgFile.ToolTipText = "发送图片";
            this.toolImgFile.Click += new System.EventHandler(this.toolImgFile_Click);
            // 
            // toolfile
            // 
            this.toolfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolfile.Image = ((System.Drawing.Image)(resources.GetObject("toolfile.Image")));
            this.toolfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolfile.Name = "toolfile";
            this.toolfile.Size = new System.Drawing.Size(23, 19);
            this.toolfile.Text = "发送文件";
            this.toolfile.Click += new System.EventHandler(this.toolfile_Click);
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton4.AutoSize = false;
            this.toolStripDropDownButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripDropDownButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Margin = new System.Windows.Forms.Padding(0, 1, 5, 2);
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(90, 24);
            this.toolStripDropDownButton4.Text = "消息记录";
            this.toolStripDropDownButton4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripDropDownButton4.ToolTipText = "显示消息记录";
            this.toolStripDropDownButton4.ButtonClick += new System.EventHandler(this.toolStripDropDownButton4_ButtonClick);
            // 
            // rtxt_send
            // 
            this.rtxt_send.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxt_send.BackColor = System.Drawing.Color.White;
            this.rtxt_send.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxt_send.Location = new System.Drawing.Point(0, 488);
            this.rtxt_send.Name = "rtxt_send";
            this.rtxt_send.SelectControl = null;
            this.rtxt_send.SelectControlIndex = 0;
            this.rtxt_send.SelectControlPoint = new System.Drawing.Point(0, 0);
            this.rtxt_send.Size = new System.Drawing.Size(456, 103);
            this.rtxt_send.TabIndex = 124;
            this.rtxt_send.Text = "";
            // 
            // rtxt_message
            // 
            this.rtxt_message.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxt_message.BackColor = System.Drawing.Color.White;
            this.rtxt_message.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxt_message.DetectUrls = false;
            this.rtxt_message.Location = new System.Drawing.Point(0, 0);
            this.rtxt_message.Name = "rtxt_message";
            this.rtxt_message.ReadOnly = true;
            this.rtxt_message.SelectControl = null;
            this.rtxt_message.SelectControlIndex = 0;
            this.rtxt_message.SelectControlPoint = new System.Drawing.Point(0, 0);
            this.rtxt_message.Size = new System.Drawing.Size(456, 460);
            this.rtxt_message.TabIndex = 123;
            this.rtxt_message.Text = "";
            this.rtxt_message.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtxt_message_LinkClicked);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(344, 594);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 28);
            this.button2.TabIndex = 126;
            this.button2.Text = "发送消息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // clb_member
            // 
            this.clb_member.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clb_member.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.clb_member.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clb_member.ForeColor = System.Drawing.Color.Black;
            this.clb_member.FriendsMobile = false;
            this.clb_member.ListSubItemMenu = null;
            this.clb_member.Location = new System.Drawing.Point(456, 0);
            this.clb_member.Margin = new System.Windows.Forms.Padding(0);
            this.clb_member.Name = "clb_member";
            this.clb_member.SelectSubItem = null;
            this.clb_member.Size = new System.Drawing.Size(232, 625);
            this.clb_member.SubItemMenu = null;
            this.clb_member.TabIndex = 144;
            this.clb_member.VipFontColor = System.Drawing.Color.Empty;
            this.clb_member.UpSubItem += new CCWin.SkinControl.ChatListBox.ChatListClickEventHandler(this.clb_member_UpSubItem);
            this.clb_member.DoubleClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.clb_member_DoubleClickSubItem);
            this.clb_member.MouseUp += new System.Windows.Forms.MouseEventHandler(this.clb_member_MouseUp);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // mnu_strip1
            // 
            this.mnu_strip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_quit_group,
            this.mnu_invite_group});
            this.mnu_strip1.Name = "mnu_strip";
            this.mnu_strip1.Size = new System.Drawing.Size(149, 48);
            // 
            // mnu_quit_group
            // 
            this.mnu_quit_group.Name = "mnu_quit_group";
            this.mnu_quit_group.Size = new System.Drawing.Size(148, 22);
            this.mnu_quit_group.Text = "移出群聊";
            this.mnu_quit_group.Click += new System.EventHandler(this.mnu_quit_group_Click);
            // 
            // mnu_invite_group
            // 
            this.mnu_invite_group.Name = "mnu_invite_group";
            this.mnu_invite_group.Size = new System.Drawing.Size(148, 22);
            this.mnu_invite_group.Text = "邀请好友加入";
            this.mnu_invite_group.Click += new System.EventHandler(this.mnu_invite_group_Click);
            // 
            // pgb_download
            // 
            this.pgb_download.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgb_download.Location = new System.Drawing.Point(12, 597);
            this.pgb_download.Name = "pgb_download";
            this.pgb_download.Size = new System.Drawing.Size(150, 10);
            this.pgb_download.TabIndex = 147;
            this.pgb_download.Visible = false;
            // 
            // pgb_upload
            // 
            this.pgb_upload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgb_upload.Location = new System.Drawing.Point(12, 612);
            this.pgb_upload.Name = "pgb_upload";
            this.pgb_upload.Size = new System.Drawing.Size(150, 10);
            this.pgb_upload.TabIndex = 148;
            this.pgb_upload.Visible = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(233, 594);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 28);
            this.button1.TabIndex = 149;
            this.button1.Text = "关闭";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "mnu_strip";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem2.Text = "邀请好友加入";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // frm_group
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(687, 625);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pgb_upload);
            this.Controls.Add(this.pgb_download);
            this.Controls.Add(this.clb_member);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.skToolMenu);
            this.Controls.Add(this.rtxt_send);
            this.Controls.Add(this.rtxt_message);
            this.Name = "frm_group";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_group";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_group_FormClosed);
            this.Load += new System.EventHandler(this.frm_group_Load);
            this.skToolMenu.ResumeLayout(false);
            this.skToolMenu.PerformLayout();
            this.mnu_strip1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinToolStrip skToolMenu;
        private System.Windows.Forms.ToolStripButton toolImgFile;
        private System.Windows.Forms.ToolStripButton toolfile;
        private System.Windows.Forms.ToolStripSplitButton toolStripDropDownButton4;
        public CCWin.SkinControl.SkinChatRichTextBox rtxt_send;
        public CCWin.SkinControl.SkinChatRichTextBox rtxt_message;
        private System.Windows.Forms.Button button2;
        private CCWin.SkinControl.ChatListBox clb_member;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ContextMenuStrip mnu_strip1;
        private System.Windows.Forms.ToolStripMenuItem mnu_quit_group;
        private System.Windows.Forms.ToolStripButton toolFont;
        private System.Windows.Forms.ProgressBar pgb_download;
        private System.Windows.Forms.ProgressBar pgb_upload;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem mnu_invite_group;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}