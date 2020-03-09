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
            this.btn_send = new System.Windows.Forms.Button();
            this.txt_send = new System.Windows.Forms.TextBox();
            this.dgv_msg = new System.Windows.Forms.DataGridView();
            this.btn_send_file = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pgb_upload = new System.Windows.Forms.ProgressBar();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.create_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.e_tag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pgb_download = new System.Windows.Forms.ProgressBar();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_msg)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(599, 400);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "发送消息";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(74, 319);
            this.txt_send.Multiline = true;
            this.txt_send.Name = "txt_send";
            this.txt_send.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_send.Size = new System.Drawing.Size(481, 66);
            this.txt_send.TabIndex = 1;
            // 
            // dgv_msg
            // 
            this.dgv_msg.AllowUserToAddRows = false;
            this.dgv_msg.AllowUserToDeleteRows = false;
            this.dgv_msg.AllowUserToResizeRows = false;
            this.dgv_msg.BackgroundColor = System.Drawing.Color.White;
            this.dgv_msg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_msg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.content,
            this.create_time,
            this.e_tag});
            this.dgv_msg.GridColor = System.Drawing.Color.White;
            this.dgv_msg.Location = new System.Drawing.Point(74, 12);
            this.dgv_msg.MultiSelect = false;
            this.dgv_msg.Name = "dgv_msg";
            this.dgv_msg.ReadOnly = true;
            this.dgv_msg.RowHeadersVisible = false;
            this.dgv_msg.RowTemplate.Height = 23;
            this.dgv_msg.Size = new System.Drawing.Size(510, 232);
            this.dgv_msg.TabIndex = 20;
            this.dgv_msg.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_msg_CellDoubleClick);
            // 
            // btn_send_file
            // 
            this.btn_send_file.Location = new System.Drawing.Point(599, 342);
            this.btn_send_file.Name = "btn_send_file";
            this.btn_send_file.Size = new System.Drawing.Size(75, 23);
            this.btn_send_file.TabIndex = 21;
            this.btn_send_file.Text = "发送文件";
            this.btn_send_file.UseVisualStyleBackColor = true;
            this.btn_send_file.Click += new System.EventHandler(this.btn_send_file_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pgb_upload
            // 
            this.pgb_upload.Location = new System.Drawing.Point(751, 173);
            this.pgb_upload.Name = "pgb_upload";
            this.pgb_upload.Size = new System.Drawing.Size(510, 23);
            this.pgb_upload.TabIndex = 22;
            // 
            // user_id
            // 
            this.user_id.Frozen = true;
            this.user_id.HeaderText = "用户Id";
            this.user_id.Name = "user_id";
            this.user_id.ReadOnly = true;
            this.user_id.Width = 75;
            // 
            // content
            // 
            this.content.Frozen = true;
            this.content.HeaderText = "内容";
            this.content.Name = "content";
            this.content.ReadOnly = true;
            this.content.Width = 250;
            // 
            // create_time
            // 
            this.create_time.Frozen = true;
            this.create_time.HeaderText = "时间";
            this.create_time.Name = "create_time";
            this.create_time.ReadOnly = true;
            // 
            // e_tag
            // 
            this.e_tag.Frozen = true;
            this.e_tag.HeaderText = "e_tag";
            this.e_tag.Name = "e_tag";
            this.e_tag.ReadOnly = true;
            // 
            // pgb_download
            // 
            this.pgb_download.Location = new System.Drawing.Point(751, 234);
            this.pgb_download.Name = "pgb_download";
            this.pgb_download.Size = new System.Drawing.Size(510, 23);
            this.pgb_download.TabIndex = 23;
            // 
            // frm_chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 456);
            this.Controls.Add(this.pgb_download);
            this.Controls.Add(this.pgb_upload);
            this.Controls.Add(this.btn_send_file);
            this.Controls.Add(this.dgv_msg);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.btn_send);
            this.Name = "frm_chat";
            this.Text = "frm_chat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_chat_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_msg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox txt_send;
        public System.Windows.Forms.DataGridView dgv_msg;
        private System.Windows.Forms.Button btn_send_file;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ProgressBar pgb_upload;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn content;
        private System.Windows.Forms.DataGridViewTextBoxColumn create_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn e_tag;
        private System.Windows.Forms.ProgressBar pgb_download;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}