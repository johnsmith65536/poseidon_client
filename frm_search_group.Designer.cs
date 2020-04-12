namespace Poseidon
{
    partial class frm_search_group
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
            this.btn_search = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.mnu_strip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_add_group = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.group_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.create_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_member = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mnu_strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(260, 27);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 6;
            this.btn_search.Text = "查找";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(31, 27);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(161, 21);
            this.txt_search.TabIndex = 5;
            // 
            // mnu_strip
            // 
            this.mnu_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_add_group});
            this.mnu_strip.Name = "mnu_strip";
            this.mnu_strip.ShowImageMargin = false;
            this.mnu_strip.Size = new System.Drawing.Size(88, 26);
            // 
            // mnu_add_group
            // 
            this.mnu_add_group.Name = "mnu_add_group";
            this.mnu_add_group.Size = new System.Drawing.Size(87, 22);
            this.mnu_add_group.Text = "加入群";
            this.mnu_add_group.Click += new System.EventHandler(this.mnu_add_group_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.group_id,
            this.group_name,
            this.create_time,
            this.is_member});
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(31, 73);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(304, 171);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            // 
            // group_id
            // 
            this.group_id.Frozen = true;
            this.group_id.HeaderText = "群号";
            this.group_id.Name = "group_id";
            this.group_id.ReadOnly = true;
            this.group_id.Width = 75;
            // 
            // group_name
            // 
            this.group_name.Frozen = true;
            this.group_name.HeaderText = "群昵称";
            this.group_name.Name = "group_name";
            this.group_name.ReadOnly = true;
            this.group_name.Width = 75;
            // 
            // create_time
            // 
            this.create_time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.create_time.Frozen = true;
            this.create_time.HeaderText = "创建时间";
            this.create_time.Name = "create_time";
            this.create_time.ReadOnly = true;
            this.create_time.Width = 150;
            // 
            // is_member
            // 
            this.is_member.Frozen = true;
            this.is_member.HeaderText = "is_member";
            this.is_member.Name = "is_member";
            this.is_member.ReadOnly = true;
            this.is_member.Visible = false;
            // 
            // frm_search_group
            // 
            this.AcceptButton = this.btn_search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 265);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.txt_search);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frm_search_group";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查找群组";
            this.Load += new System.EventHandler(this.frm_search_group_Load);
            this.mnu_strip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.ContextMenuStrip mnu_strip;
        private System.Windows.Forms.ToolStripMenuItem mnu_add_group;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn create_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_member;
    }
}