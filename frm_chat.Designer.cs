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
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.create_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_msg)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(684, 388);
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
            this.create_time});
            this.dgv_msg.GridColor = System.Drawing.Color.White;
            this.dgv_msg.Location = new System.Drawing.Point(612, 33);
            this.dgv_msg.Name = "dgv_msg";
            this.dgv_msg.ReadOnly = true;
            this.dgv_msg.RowHeadersVisible = false;
            this.dgv_msg.RowTemplate.Height = 23;
            this.dgv_msg.Size = new System.Drawing.Size(510, 232);
            this.dgv_msg.TabIndex = 20;
            this.dgv_msg.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_msg_CellContentClick);
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
            // 
            // create_time
            // 
            this.create_time.Frozen = true;
            this.create_time.HeaderText = "时间";
            this.create_time.Name = "create_time";
            this.create_time.ReadOnly = true;
            // 
            // frm_chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 456);
            this.Controls.Add(this.dgv_msg);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.btn_send);
            this.Name = "frm_chat";
            this.Text = "frm_chat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_chat_FormClosed);
            this.Load += new System.EventHandler(this.frm_chat_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_msg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox txt_send;
        public System.Windows.Forms.DataGridView dgv_msg;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn content;
        private System.Windows.Forms.DataGridViewTextBoxColumn create_time;
    }
}