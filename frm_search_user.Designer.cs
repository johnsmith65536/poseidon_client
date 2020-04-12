namespace Poseidon
{
    partial class frm_search_user
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
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.mnu_strip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_add_friend = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nick_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_online_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_friend = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mnu_strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(28, 27);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(161, 21);
            this.txt_search.TabIndex = 0;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(258, 25);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 1;
            this.btn_search.Text = "查找";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // mnu_strip
            // 
            this.mnu_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_add_friend});
            this.mnu_strip.Name = "mnu_strip";
            this.mnu_strip.ShowImageMargin = false;
            this.mnu_strip.Size = new System.Drawing.Size(100, 26);
            this.mnu_strip.Opening += new System.ComponentModel.CancelEventHandler(this.mnu_strip_Opening);
            // 
            // mnu_add_friend
            // 
            this.mnu_add_friend.Name = "mnu_add_friend";
            this.mnu_add_friend.Size = new System.Drawing.Size(99, 22);
            this.mnu_add_friend.Text = "添加好友";
            this.mnu_add_friend.Click += new System.EventHandler(this.mnu_add_friend_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.nick_name,
            this.last_online_time,
            this.is_friend});
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(28, 73);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(305, 171);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            // 
            // user_id
            // 
            this.user_id.Frozen = true;
            this.user_id.HeaderText = "用户ID";
            this.user_id.Name = "user_id";
            this.user_id.ReadOnly = true;
            this.user_id.Width = 75;
            // 
            // nick_name
            // 
            this.nick_name.Frozen = true;
            this.nick_name.HeaderText = "昵称";
            this.nick_name.Name = "nick_name";
            this.nick_name.ReadOnly = true;
            this.nick_name.Width = 75;
            // 
            // last_online_time
            // 
            this.last_online_time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.last_online_time.Frozen = true;
            this.last_online_time.HeaderText = "最后在线时间";
            this.last_online_time.Name = "last_online_time";
            this.last_online_time.ReadOnly = true;
            this.last_online_time.Width = 150;
            // 
            // is_friend
            // 
            this.is_friend.Frozen = true;
            this.is_friend.HeaderText = "is_friend";
            this.is_friend.Name = "is_friend";
            this.is_friend.ReadOnly = true;
            this.is_friend.Visible = false;
            // 
            // frm_search_user
            // 
            this.AcceptButton = this.btn_search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 271);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.txt_search);
            this.Name = "frm_search_user";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查找用户";
            this.mnu_strip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.ContextMenuStrip mnu_strip;
        private System.Windows.Forms.ToolStripMenuItem mnu_add_friend;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn nick_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_online_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_friend;
    }
}