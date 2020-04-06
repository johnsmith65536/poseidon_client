namespace Poseidon
{
    partial class frm_main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnu_add_friend = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_stats = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_online = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_offline = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_create_group = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_join_group = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_strip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_del_friend = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ChatListBox = new CCWin.SkinControl.ChatListBox();
            this.sqLiteCommand1 = new System.Data.SQLite.SQLiteCommand();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.mnu_strip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_dissolve_group = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_strip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_quit_group = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.mnu_strip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.mnu_strip1.SuspendLayout();
            this.mnu_strip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_add_friend,
            this.mnu_stats,
            this.mnu_create_group,
            this.mnu_join_group});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(269, 25);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnu_add_friend
            // 
            this.mnu_add_friend.Name = "mnu_add_friend";
            this.mnu_add_friend.Size = new System.Drawing.Size(68, 21);
            this.mnu_add_friend.Text = "添加好友";
            this.mnu_add_friend.Click += new System.EventHandler(this.mnu_add_friend_Click);
            // 
            // mnu_stats
            // 
            this.mnu_stats.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_online,
            this.mnu_offline});
            this.mnu_stats.Name = "mnu_stats";
            this.mnu_stats.Size = new System.Drawing.Size(44, 21);
            this.mnu_stats.Text = "状态";
            // 
            // mnu_online
            // 
            this.mnu_online.Name = "mnu_online";
            this.mnu_online.Size = new System.Drawing.Size(100, 22);
            this.mnu_online.Text = "在线";
            this.mnu_online.Click += new System.EventHandler(this.mnu_online_Click);
            // 
            // mnu_offline
            // 
            this.mnu_offline.Name = "mnu_offline";
            this.mnu_offline.Size = new System.Drawing.Size(100, 22);
            this.mnu_offline.Text = "离线";
            this.mnu_offline.Click += new System.EventHandler(this.mnu_offline_Click);
            // 
            // mnu_create_group
            // 
            this.mnu_create_group.Name = "mnu_create_group";
            this.mnu_create_group.Size = new System.Drawing.Size(56, 21);
            this.mnu_create_group.Text = "创建群";
            this.mnu_create_group.Click += new System.EventHandler(this.mnu_create_group_Click);
            // 
            // mnu_join_group
            // 
            this.mnu_join_group.Name = "mnu_join_group";
            this.mnu_join_group.Size = new System.Drawing.Size(56, 21);
            this.mnu_join_group.Text = "加入群";
            this.mnu_join_group.Click += new System.EventHandler(this.mnu_join_group_Click);
            // 
            // mnu_strip
            // 
            this.mnu_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_del_friend});
            this.mnu_strip.Name = "mnu_strip";
            this.mnu_strip.Size = new System.Drawing.Size(125, 26);
            // 
            // mnu_del_friend
            // 
            this.mnu_del_friend.Name = "mnu_del_friend";
            this.mnu_del_friend.Size = new System.Drawing.Size(124, 22);
            this.mnu_del_friend.Text = "删除好友";
            this.mnu_del_friend.Click += new System.EventHandler(this.mnu_del_friend_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 477);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(269, 22);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(254, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ChatListBox
            // 
            this.ChatListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ChatListBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChatListBox.ForeColor = System.Drawing.Color.Black;
            this.ChatListBox.FriendsMobile = false;
            this.ChatListBox.ListSubItemMenu = null;
            this.ChatListBox.Location = new System.Drawing.Point(0, 28);
            this.ChatListBox.Name = "ChatListBox";
            this.ChatListBox.SelectSubItem = null;
            this.ChatListBox.Size = new System.Drawing.Size(271, 450);
            this.ChatListBox.SubItemMenu = null;
            this.ChatListBox.TabIndex = 21;
            this.ChatListBox.Text = "chatListBox1";
            this.ChatListBox.UpSubItem += new CCWin.SkinControl.ChatListBox.ChatListClickEventHandler(this.ChatListBox_UpSubItem);
            this.ChatListBox.DoubleClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.ChatListBox_DoubleClickSubItem);
            // 
            // sqLiteCommand1
            // 
            this.sqLiteCommand1.CommandText = null;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // timer2
            // 
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // mnu_strip1
            // 
            this.mnu_strip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_dissolve_group});
            this.mnu_strip1.Name = "mnu_strip";
            this.mnu_strip1.Size = new System.Drawing.Size(125, 26);
            // 
            // mnu_dissolve_group
            // 
            this.mnu_dissolve_group.Name = "mnu_dissolve_group";
            this.mnu_dissolve_group.Size = new System.Drawing.Size(124, 22);
            this.mnu_dissolve_group.Text = "解散群聊";
            this.mnu_dissolve_group.Click += new System.EventHandler(this.mnu_dissolve_group_Click);
            // 
            // mnu_strip2
            // 
            this.mnu_strip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_quit_group});
            this.mnu_strip2.Name = "mnu_strip";
            this.mnu_strip2.Size = new System.Drawing.Size(125, 26);
            // 
            // mnu_quit_group
            // 
            this.mnu_quit_group.Name = "mnu_quit_group";
            this.mnu_quit_group.Size = new System.Drawing.Size(124, 22);
            this.mnu_quit_group.Text = "退出群聊";
            this.mnu_quit_group.Click += new System.EventHandler(this.mnu_quit_group_Click);
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 499);
            this.Controls.Add(this.ChatListBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frm_main";
            this.Text = "frm_main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_main_FormClosed);
            this.Load += new System.EventHandler(this.frm_main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mnu_strip.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mnu_strip1.ResumeLayout(false);
            this.mnu_strip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem mnu_add_friend;
        private System.Windows.Forms.ContextMenuStrip mnu_strip;
        private System.Windows.Forms.ToolStripMenuItem mnu_del_friend;
        private System.Windows.Forms.ToolStripMenuItem mnu_stats;
        public System.Windows.Forms.ToolStripMenuItem mnu_online;
        public System.Windows.Forms.ToolStripMenuItem mnu_offline;
        public System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Data.SQLite.SQLiteCommand sqLiteCommand1;
        public CCWin.SkinControl.ChatListBox ChatListBox;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.ToolStripMenuItem mnu_create_group;
        private System.Windows.Forms.ToolStripMenuItem mnu_join_group;
        private System.Windows.Forms.ContextMenuStrip mnu_strip1;
        private System.Windows.Forms.ToolStripMenuItem mnu_dissolve_group;
        private System.Windows.Forms.ContextMenuStrip mnu_strip2;
        private System.Windows.Forms.ToolStripMenuItem mnu_quit_group;
    }
}