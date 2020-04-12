namespace Poseidon
{
    partial class frm_invite_group
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
            this.button1 = new System.Windows.Forms.Button();
            this.clb_friend = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(140, 180);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "邀请好友";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clb_friend
            // 
            this.clb_friend.FormattingEnabled = true;
            this.clb_friend.Location = new System.Drawing.Point(53, 43);
            this.clb_friend.Name = "clb_friend";
            this.clb_friend.Size = new System.Drawing.Size(239, 68);
            this.clb_friend.TabIndex = 6;
            // 
            // frm_invite_group
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 224);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.clb_friend);
            this.Name = "frm_invite_group";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "邀请好友加入群聊";
            this.Load += new System.EventHandler(this.frm_invite_group_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox clb_friend;
    }
}