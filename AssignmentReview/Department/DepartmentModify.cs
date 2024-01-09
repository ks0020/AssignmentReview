using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssignmentReview
{
    public partial class DepartmentModify : Form
    {

        public DepartmentModify(string code, string name)
        {
            InitializeComponent();

            UpdateBtn.Click += Update;
            CloseBtn.Click += Close;

            CodeText.Text += code;
            NameText.Text += name;
            
        }
        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";

        public void Update(object sneder, EventArgs e) 
        {
            string departmentCode = CodeText.Text;
            string departmentName = NameText.Text;


            if (string.IsNullOrEmpty(departmentCode) || string.IsNullOrEmpty(departmentName))
            {
                MessageBox.Show("필수 정보를 기입해주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE dbo.department SET 부서명 = @DepartmentName, 메모 = @Memo WHERE 부서코드 = @DepartmentCode";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DepartmentCode", departmentCode);
                    command.Parameters.AddWithValue("@DepartmentName", departmentName);
                    command.Parameters.AddWithValue("@Memo", MemoText.Text);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("성공적으로 수정이 되었습니다!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("세부정보를 수정하지 못했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러발생: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Close(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
