namespace Poseidon
{
    partial class frm_msg_box
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
            this.clb_unread_msg = new CCWin.SkinControl.ChatListBox();
            this.SuspendLayout();
            // 
            // clb_unread_msg
            // 
            this.clb_unread_msg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.clb_unread_msg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clb_unread_msg.ForeColor = System.Drawing.Color.Black;
            this.clb_unread_msg.FriendsMobile = false;
            this.clb_unread_msg.ListSubItemMenu = null;
            this.clb_unread_msg.Location = new System.Drawing.Point(0, 0);
            this.clb_unread_msg.Name = "clb_unread_msg";
            this.clb_unread_msg.SelectSubItem = null;
            this.clb_unread_msg.Size = new System.Drawing.Size(247, 155);
            this.clb_unread_msg.SubItemMenu = null;
            this.clb_unread_msg.TabIndex = 23;
            this.clb_unread_msg.Text = "chatListBox1";
            this.clb_unread_msg.ClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListClickEventHandler(this.clb_unread_msg_ClickSubItem);
            // 
            // frm_msg_box
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 157);
            this.Controls.Add(this.clb_unread_msg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_msg_box";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frm_msg_box";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public CCWin.SkinControl.ChatListBox clb_unread_msg;
    }
}