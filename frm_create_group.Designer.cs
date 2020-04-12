namespace Poseidon
{
    partial class frm_create_group
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
            this.txt_group_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.clb_friend = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_group_name
            // 
            this.txt_group_name.Location = new System.Drawing.Point(111, 46);
            this.txt_group_name.Name = "txt_group_name";
            this.txt_group_name.Size = new System.Drawing.Size(179, 21);
            this.txt_group_name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "群名";
            // 
            // clb_friend
            // 
            this.clb_friend.FormattingEnabled = true;
            this.clb_friend.Location = new System.Drawing.Point(51, 102);
            this.clb_friend.Name = "clb_friend";
            this.clb_friend.Size = new System.Drawing.Size(239, 68);
            this.clb_friend.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(129, 204);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "创建群组";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frm_create_group
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 251);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.clb_friend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_group_name);
            this.Name = "frm_create_group";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建群组";
            this.Load += new System.EventHandler(this.frm_create_group_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_group_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clb_friend;
        private System.Windows.Forms.Button button1;
    }
}