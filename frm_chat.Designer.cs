namespace Poseidon
{
    partial class frm_chat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_chat));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pgb_upload = new System.Windows.Forms.ProgressBar();
            this.pgb_download = new System.Windows.Forms.ProgressBar();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.rtxt_message = new CCWin.SkinControl.SkinChatRichTextBox();
            this.rtxt_send = new CCWin.SkinControl.SkinChatRichTextBox();
            this.skToolMenu = new CCWin.SkinControl.SkinToolStrip();
            this.toolFont = new System.Windows.Forms.ToolStripButton();
            this.toolVibration = new System.Windows.Forms.ToolStripButton();
            this.toolImgFile = new System.Windows.Forms.ToolStripButton();
            this.toolfile = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripSplitButton();
            this.显示消息记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示比例ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.放大Ctrl滚轮ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.清屏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.消息管理器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolItemPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.录制视屏动画ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.捕捉视屏图像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.显示截图编辑工具栏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.截图时隐藏当前窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.skToolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pgb_upload
            // 
            this.pgb_upload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgb_upload.Location = new System.Drawing.Point(12, 504);
            this.pgb_upload.Name = "pgb_upload";
            this.pgb_upload.Size = new System.Drawing.Size(138, 23);
            this.pgb_upload.TabIndex = 22;
            // 
            // pgb_download
            // 
            this.pgb_download.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgb_download.Location = new System.Drawing.Point(163, 504);
            this.pgb_download.Name = "pgb_download";
            this.pgb_download.Size = new System.Drawing.Size(157, 23);
            this.pgb_download.TabIndex = 23;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(393, 504);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 29);
            this.button2.TabIndex = 26;
            this.button2.Text = "发送消息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.rtxt_message.Size = new System.Drawing.Size(476, 366);
            this.rtxt_message.TabIndex = 27;
            this.rtxt_message.Text = "";
            this.rtxt_message.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtxt_message_LinkClicked);
            // 
            // rtxt_send
            // 
            this.rtxt_send.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxt_send.BackColor = System.Drawing.Color.White;
            this.rtxt_send.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxt_send.Location = new System.Drawing.Point(0, 393);
            this.rtxt_send.Name = "rtxt_send";
            this.rtxt_send.SelectControl = null;
            this.rtxt_send.SelectControlIndex = 0;
            this.rtxt_send.SelectControlPoint = new System.Drawing.Point(0, 0);
            this.rtxt_send.Size = new System.Drawing.Size(476, 105);
            this.rtxt_send.TabIndex = 28;
            this.rtxt_send.Text = "";
            this.rtxt_send.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtxt_send_KeyDown);
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
            this.toolVibration,
            this.toolImgFile,
            this.toolStripDropDownButton4,
            this.toolfile});
            this.skToolMenu.Location = new System.Drawing.Point(0, 364);
            this.skToolMenu.Name = "skToolMenu";
            this.skToolMenu.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skToolMenu.Size = new System.Drawing.Size(476, 26);
            this.skToolMenu.SkinAllColor = true;
            this.skToolMenu.TabIndex = 122;
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
            this.toolFont.Size = new System.Drawing.Size(23, 23);
            this.toolFont.Text = "toolStripButton1";
            this.toolFont.ToolTipText = "字体选择工具栏";
            this.toolFont.Click += new System.EventHandler(this.toolFont_Click);
            // 
            // toolVibration
            // 
            this.toolVibration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolVibration.Image = ((System.Drawing.Image)(resources.GetObject("toolVibration.Image")));
            this.toolVibration.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolVibration.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolVibration.Name = "toolVibration";
            this.toolVibration.Size = new System.Drawing.Size(23, 23);
            this.toolVibration.Text = "toolStripButton4";
            this.toolVibration.ToolTipText = "向好友发送窗口抖动";
            this.toolVibration.Click += new System.EventHandler(this.toolVibration_Click);
            // 
            // toolImgFile
            // 
            this.toolImgFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolImgFile.Image = ((System.Drawing.Image)(resources.GetObject("toolImgFile.Image")));
            this.toolImgFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolImgFile.Name = "toolImgFile";
            this.toolImgFile.Size = new System.Drawing.Size(23, 23);
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
            this.toolfile.Size = new System.Drawing.Size(23, 23);
            this.toolfile.Text = "发送文件";
            this.toolfile.Click += new System.EventHandler(this.toolfile_Click);
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton4.AutoSize = false;
            this.toolStripDropDownButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示消息记录ToolStripMenuItem,
            this.显示比例ToolStripMenuItem,
            this.toolStripMenuItem5,
            this.清屏ToolStripMenuItem,
            this.消息管理器ToolStripMenuItem});
            this.toolStripDropDownButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripDropDownButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Margin = new System.Windows.Forms.Padding(0, 1, 5, 2);
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(90, 24);
            this.toolStripDropDownButton4.Text = "消息记录";
            this.toolStripDropDownButton4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripDropDownButton4.ToolTipText = "显示消息记录";
            // 
            // 显示消息记录ToolStripMenuItem
            // 
            this.显示消息记录ToolStripMenuItem.Name = "显示消息记录ToolStripMenuItem";
            this.显示消息记录ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.显示消息记录ToolStripMenuItem.Text = "显示消息记录";
            // 
            // 显示比例ToolStripMenuItem
            // 
            this.显示比例ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.放大Ctrl滚轮ToolStripMenuItem,
            this.缩小ToolStripMenuItem,
            this.toolStripMenuItem6,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12,
            this.toolStripMenuItem13,
            this.toolStripMenuItem14});
            this.显示比例ToolStripMenuItem.Name = "显示比例ToolStripMenuItem";
            this.显示比例ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.显示比例ToolStripMenuItem.Text = "显示比例";
            // 
            // 放大Ctrl滚轮ToolStripMenuItem
            // 
            this.放大Ctrl滚轮ToolStripMenuItem.Name = "放大Ctrl滚轮ToolStripMenuItem";
            this.放大Ctrl滚轮ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.放大Ctrl滚轮ToolStripMenuItem.Text = "放大 Ctrl+滚轮";
            // 
            // 缩小ToolStripMenuItem
            // 
            this.缩小ToolStripMenuItem.Name = "缩小ToolStripMenuItem";
            this.缩小ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.缩小ToolStripMenuItem.Text = "缩小";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(154, 6);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem8.Text = "400%";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem9.Text = "200%";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem10.Text = "150%";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem11.Text = "125%";
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem12.Text = "100%";
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem13.Text = "75%";
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem14.Text = "50%";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(145, 6);
            // 
            // 清屏ToolStripMenuItem
            // 
            this.清屏ToolStripMenuItem.Name = "清屏ToolStripMenuItem";
            this.清屏ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.清屏ToolStripMenuItem.Text = "清屏";
            // 
            // 消息管理器ToolStripMenuItem
            // 
            this.消息管理器ToolStripMenuItem.Name = "消息管理器ToolStripMenuItem";
            this.消息管理器ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.消息管理器ToolStripMenuItem.Text = "消息管理器";
            // 
            // ToolItemPrint
            // 
            this.ToolItemPrint.Name = "ToolItemPrint";
            this.ToolItemPrint.Size = new System.Drawing.Size(185, 22);
            this.ToolItemPrint.Text = "屏幕截图Ctrl+Alt+A";
            // 
            // 录制视屏动画ToolStripMenuItem
            // 
            this.录制视屏动画ToolStripMenuItem.Name = "录制视屏动画ToolStripMenuItem";
            this.录制视屏动画ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.录制视屏动画ToolStripMenuItem.Text = "录制视屏动画";
            // 
            // 捕捉视屏图像ToolStripMenuItem
            // 
            this.捕捉视屏图像ToolStripMenuItem.Name = "捕捉视屏图像ToolStripMenuItem";
            this.捕捉视屏图像ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.捕捉视屏图像ToolStripMenuItem.Text = "捕捉视屏图像";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
            // 
            // 显示截图编辑工具栏ToolStripMenuItem
            // 
            this.显示截图编辑工具栏ToolStripMenuItem.Name = "显示截图编辑工具栏ToolStripMenuItem";
            this.显示截图编辑工具栏ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.显示截图编辑工具栏ToolStripMenuItem.Text = "显示截图编辑工具栏";
            // 
            // 截图时隐藏当前窗口ToolStripMenuItem
            // 
            this.截图时隐藏当前窗口ToolStripMenuItem.Name = "截图时隐藏当前窗口ToolStripMenuItem";
            this.截图时隐藏当前窗口ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.截图时隐藏当前窗口ToolStripMenuItem.Text = "截图时隐藏当前窗口";
            // 
            // frm_chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 534);
            this.Controls.Add(this.skToolMenu);
            this.Controls.Add(this.rtxt_send);
            this.Controls.Add(this.rtxt_message);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pgb_download);
            this.Controls.Add(this.pgb_upload);
            this.Name = "frm_chat";
            this.Text = "frm_chat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_chat_FormClosed);
            this.Load += new System.EventHandler(this.frm_chat_Load);
            this.skToolMenu.ResumeLayout(false);
            this.skToolMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ProgressBar pgb_upload;
        private System.Windows.Forms.ProgressBar pgb_download;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button2;
        public CCWin.SkinControl.SkinChatRichTextBox rtxt_message;
        public CCWin.SkinControl.SkinChatRichTextBox rtxt_send;
        private CCWin.SkinControl.SkinToolStrip skToolMenu;
        private System.Windows.Forms.ToolStripButton toolFont;
        private System.Windows.Forms.ToolStripButton toolVibration;
        private System.Windows.Forms.ToolStripButton toolImgFile;
        private System.Windows.Forms.ToolStripMenuItem ToolItemPrint;
        private System.Windows.Forms.ToolStripMenuItem 录制视屏动画ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 捕捉视屏图像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 显示截图编辑工具栏ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 截图时隐藏当前窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripDropDownButton4;
        private System.Windows.Forms.ToolStripMenuItem 显示消息记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示比例ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 放大Ctrl滚轮ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem 清屏ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 消息管理器ToolStripMenuItem;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripButton toolfile;
    }
}