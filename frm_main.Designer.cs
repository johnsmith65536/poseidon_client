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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnu_add_friend = new System.Windows.Forms.ToolStripMenuItem();
            this.dgv_friend = new System.Windows.Forms.DataGridView();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_msg = new System.Windows.Forms.DataGridView();
            this.mnu_strip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_del_friend = new System.Windows.Forms.ToolStripMenuItem();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_id_send = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.create_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_friend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_msg)).BeginInit();
            this.mnu_strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_add_friend});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1178, 25);
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
            // dgv_friend
            // 
            this.dgv_friend.AllowUserToAddRows = false;
            this.dgv_friend.AllowUserToDeleteRows = false;
            this.dgv_friend.AllowUserToResizeRows = false;
            this.dgv_friend.BackgroundColor = System.Drawing.Color.White;
            this.dgv_friend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_friend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.status});
            this.dgv_friend.GridColor = System.Drawing.Color.White;
            this.dgv_friend.Location = new System.Drawing.Point(301, 28);
            this.dgv_friend.Name = "dgv_friend";
            this.dgv_friend.ReadOnly = true;
            this.dgv_friend.RowHeadersVisible = false;
            this.dgv_friend.RowTemplate.Height = 23;
            this.dgv_friend.Size = new System.Drawing.Size(167, 157);
            this.dgv_friend.TabIndex = 18;
            this.dgv_friend.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_friend_CellDoubleClick);
            this.dgv_friend.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_friend_CellMouseUp);
            // 
            // user_id
            // 
            this.user_id.Frozen = true;
            this.user_id.HeaderText = "用户Id";
            this.user_id.Name = "user_id";
            this.user_id.ReadOnly = true;
            this.user_id.Width = 75;
            // 
            // status
            // 
            this.status.Frozen = true;
            this.status.HeaderText = "状态";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 75;
            // 
            // dgv_msg
            // 
            this.dgv_msg.AllowUserToAddRows = false;
            this.dgv_msg.AllowUserToDeleteRows = false;
            this.dgv_msg.AllowUserToResizeRows = false;
            this.dgv_msg.BackgroundColor = System.Drawing.Color.White;
            this.dgv_msg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_msg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.type,
            this.user_id_send,
            this.content,
            this.create_time});
            this.dgv_msg.GridColor = System.Drawing.Color.White;
            this.dgv_msg.Location = new System.Drawing.Point(509, 28);
            this.dgv_msg.Name = "dgv_msg";
            this.dgv_msg.ReadOnly = true;
            this.dgv_msg.RowHeadersVisible = false;
            this.dgv_msg.RowTemplate.Height = 23;
            this.dgv_msg.Size = new System.Drawing.Size(481, 157);
            this.dgv_msg.TabIndex = 19;
            this.dgv_msg.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_msg_CellDoubleClick);
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
            // id
            // 
            this.id.Frozen = true;
            this.id.HeaderText = "Id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 75;
            // 
            // type
            // 
            this.type.Frozen = true;
            this.type.HeaderText = "类型";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.Width = 75;
            // 
            // user_id_send
            // 
            this.user_id_send.Frozen = true;
            this.user_id_send.HeaderText = "发送者";
            this.user_id_send.Name = "user_id_send";
            this.user_id_send.ReadOnly = true;
            this.user_id_send.Width = 75;
            // 
            // content
            // 
            this.content.Frozen = true;
            this.content.HeaderText = "内容";
            this.content.Name = "content";
            this.content.ReadOnly = true;
            this.content.Width = 200;
            // 
            // create_time
            // 
            this.create_time.Frozen = true;
            this.create_time.HeaderText = "时间";
            this.create_time.Name = "create_time";
            this.create_time.ReadOnly = true;
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 361);
            this.Controls.Add(this.dgv_msg);
            this.Controls.Add(this.dgv_friend);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frm_main";
            this.Text = "frm_main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_main_FormClosed);
            this.Load += new System.EventHandler(this.frm_main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_friend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_msg)).EndInit();
            this.mnu_strip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnu_add_friend;
        private System.Windows.Forms.DataGridView dgv_friend;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridView dgv_msg;
        private System.Windows.Forms.ContextMenuStrip mnu_strip;
        private System.Windows.Forms.ToolStripMenuItem mnu_del_friend;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id_send;
        private System.Windows.Forms.DataGridViewTextBoxColumn content;
        private System.Windows.Forms.DataGridViewTextBoxColumn create_time;
    }
}