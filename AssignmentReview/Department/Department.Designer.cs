
namespace AssignmentReview
{
    partial class Department
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.closeBtn = new System.Windows.Forms.Label();
            this.Title1 = new System.Windows.Forms.Label();
            this.DeleteBtn = new System.Windows.Forms.Label();
            this.ModifyBtn = new System.Windows.Forms.Label();
            this.AddBtn = new System.Windows.Forms.Label();
            this.DepartmentTable = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentTable)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.closeBtn);
            this.panel1.Controls.Add(this.Title1);
            this.panel1.Controls.Add(this.DeleteBtn);
            this.panel1.Controls.Add(this.ModifyBtn);
            this.panel1.Controls.Add(this.AddBtn);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 35);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::AssignmentReview.Properties.Resources.ic_person_24px;
            this.panel2.Location = new System.Drawing.Point(14, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(16, 16);
            this.panel2.TabIndex = 7;
            // 
            // closeBtn
            // 
            this.closeBtn.AutoSize = true;
            this.closeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeBtn.Location = new System.Drawing.Point(484, 6);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(42, 21);
            this.closeBtn.TabIndex = 2;
            this.closeBtn.Text = "닫기";
            // 
            // Title1
            // 
            this.Title1.AutoSize = true;
            this.Title1.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.Title1.Location = new System.Drawing.Point(36, 5);
            this.Title1.Name = "Title1";
            this.Title1.Size = new System.Drawing.Size(84, 25);
            this.Title1.TabIndex = 1;
            this.Title1.Text = "부서사원";
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.AutoSize = true;
            this.DeleteBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DeleteBtn.Location = new System.Drawing.Point(436, 6);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(42, 21);
            this.DeleteBtn.TabIndex = 3;
            this.DeleteBtn.Text = "삭제";
            // 
            // ModifyBtn
            // 
            this.ModifyBtn.AutoSize = true;
            this.ModifyBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ModifyBtn.Location = new System.Drawing.Point(388, 6);
            this.ModifyBtn.Name = "ModifyBtn";
            this.ModifyBtn.Size = new System.Drawing.Size(42, 21);
            this.ModifyBtn.TabIndex = 4;
            this.ModifyBtn.Text = "수정";
            // 
            // AddBtn
            // 
            this.AddBtn.AutoSize = true;
            this.AddBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddBtn.Location = new System.Drawing.Point(340, 6);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(42, 21);
            this.AddBtn.TabIndex = 5;
            this.AddBtn.Text = "추가";
            // 
            // DepartmentTable
            // 
            this.DepartmentTable.BackgroundColor = System.Drawing.Color.White;
            this.DepartmentTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DepartmentTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DepartmentTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DepartmentTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DepartmentTable.Location = new System.Drawing.Point(12, 53);
            this.DepartmentTable.Name = "DepartmentTable";
            this.DepartmentTable.RowTemplate.Height = 23;
            this.DepartmentTable.Size = new System.Drawing.Size(508, 276);
            this.DepartmentTable.TabIndex = 1;
            // 
            // Department
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(533, 341);
            this.Controls.Add(this.DepartmentTable);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(549, 380);
            this.MinimumSize = new System.Drawing.Size(549, 380);
            this.Name = "Department";
            this.Text = "부서";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DepartmentTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label closeBtn;
        private System.Windows.Forms.Label Title1;
        private System.Windows.Forms.Label DeleteBtn;
        private System.Windows.Forms.Label ModifyBtn;
        private System.Windows.Forms.Label AddBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView DepartmentTable;
    }
}