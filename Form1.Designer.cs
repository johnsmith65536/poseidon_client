namespace Poseidon
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txt_user_id = new System.Windows.Forms.TextBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.txt_status = new System.Windows.Forms.TextBox();
            this.txt_send = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.txt_id_recv = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.txt_friend = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.txt_add_friend_id = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.txt_friend_list = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(695, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "创建用户";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(241, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "登录";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txt_user_id
            // 
            this.txt_user_id.Location = new System.Drawing.Point(78, 58);
            this.txt_user_id.Name = "txt_user_id";
            this.txt_user_id.Size = new System.Drawing.Size(100, 21);
            this.txt_user_id.TabIndex = 2;
            this.txt_user_id.Text = "1709";
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(78, 102);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(100, 21);
            this.txt_password.TabIndex = 3;
            this.txt_password.Text = "john";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(695, 102);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "注销";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txt_status
            // 
            this.txt_status.Location = new System.Drawing.Point(78, 172);
            this.txt_status.Multiline = true;
            this.txt_status.Name = "txt_status";
            this.txt_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_status.Size = new System.Drawing.Size(406, 126);
            this.txt_status.TabIndex = 5;
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(78, 339);
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(248, 21);
            this.txt_send.TabIndex = 6;
            this.txt_send.Text = "john";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(509, 339);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "发送";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txt_id_recv
            // 
            this.txt_id_recv.Location = new System.Drawing.Point(384, 339);
            this.txt_id_recv.Name = "txt_id_recv";
            this.txt_id_recv.Size = new System.Drawing.Size(100, 21);
            this.txt_id_recv.TabIndex = 8;
            this.txt_id_recv.Text = "1709";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(595, 58);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "添加好友";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // txt_friend
            // 
            this.txt_friend.Location = new System.Drawing.Point(467, 58);
            this.txt_friend.Name = "txt_friend";
            this.txt_friend.Size = new System.Drawing.Size(100, 21);
            this.txt_friend.TabIndex = 10;
            this.txt_friend.Text = "1709";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(595, 102);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "接受好友";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(595, 142);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 12;
            this.button7.Text = "拒绝好友";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // txt_add_friend_id
            // 
            this.txt_add_friend_id.Location = new System.Drawing.Point(467, 102);
            this.txt_add_friend_id.Name = "txt_add_friend_id";
            this.txt_add_friend_id.Size = new System.Drawing.Size(100, 21);
            this.txt_add_friend_id.TabIndex = 13;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(695, 142);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(93, 23);
            this.button8.TabIndex = 14;
            this.button8.Text = "获取好友列表";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // txt_friend_list
            // 
            this.txt_friend_list.Location = new System.Drawing.Point(594, 172);
            this.txt_friend_list.Multiline = true;
            this.txt_friend_list.Name = "txt_friend_list";
            this.txt_friend_list.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_friend_list.Size = new System.Drawing.Size(194, 126);
            this.txt_friend_list.TabIndex = 15;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(594, 324);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(194, 88);
            this.listBox1.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.txt_friend_list);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.txt_add_friend_id);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.txt_friend);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.txt_id_recv);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.txt_status);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.txt_user_id);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txt_user_id;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txt_status;
        private System.Windows.Forms.TextBox txt_send;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txt_id_recv;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox txt_friend;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox txt_add_friend_id;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox txt_friend_list;
        private System.Windows.Forms.ListBox listBox1;
    }
}

