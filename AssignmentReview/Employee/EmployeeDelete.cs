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
    public partial class EmployeeDelete : Form
    {
        private string selectedCode;
        private string selectedName;

        public EmployeeDelete(string code, string name)
        {
            InitializeComponent();

            selectedCode = code;
            selectedName = name;
            EmployeeCode.Text = "사원 코드 : " + selectedCode;
            EmployeeName.Text = "사원 명 : " + selectedName;

            DeleteBtn.Click += Delete;
            CloseBtn.Click += Close;
        }
        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";

        private void Delete(object sende, EventArgs e)
        {
            if(!string.IsNullOrEmpty(selectedCode) && !string.IsNullOrEmpty(selectedName))
            {
                string query = "DELETE FROM dbo.employee WHERE 사원코드 = @EmployeeCode AND 사원명 = @EmployeeName";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeCode", selectedCode);
                        command.Parameters.AddWithValue("@EmployeeName", selectedName);

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if(rowsAffected > 0)
                            {
                                MessageBox.Show("성공적으로 삭제되었습니다!");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("삭제할 데이터를 찾을 수 없습니다.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("삭제 중 오류 발생 : " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("삭제할 항목이 선택되지 않았습니다.");
            }
        }

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
