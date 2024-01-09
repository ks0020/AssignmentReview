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
    public partial class DepartmentDelete : Form
    {
        // DepartmentDelete의 멤버변수
        private string selectedCode;
        private string selectedName;
        public DepartmentDelete(string code, string name)
        {
            InitializeComponent();

            DeleteBtn.Click += Delete;
            CloseBtn.Click += Close;

            // code와 name을 각각의 변수에 할당한다.
            selectedCode = code;
            selectedName = name;

            CodeLabel.Text = "부서 코드 : " + selectedCode;
            NameLabel.Text = "부서 명 : " + selectedName;
        }
        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";

        public void Delete(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedCode) && !string.IsNullOrEmpty(selectedName))
            // ISNullOrEmpty : 주어진 문자열이 null이거나 비어있는지 여부를 확인하는 데 사용
            {
                // 선택된 부서 코드와 이름을 사용하여 삭제 쿼리
                string deleteQuery = "DELETE FROM dbo.department WHERE 부서코드 = @DepartmentCode AND 부서명 = @DepartmentName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                // SqlConnection 객체를 만듬. DB 연결이 열린 후에 코드 실행이 완료되면 Dispose 메서드를 호출하여 DB 연결을 닫고 관련 리소스를 해제
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    // SqlCommand객체를 만듬. 만찬가지로, Dispose메서드를 호출하여 SqlCommand와 관련된 리소스를 해제한다.
                    // 중첩된 using문을 사용함으로써 코드가 블록을 벗어날 때 각 객체가 적절히 닫히고 리소스가 해제되도록 보장함. 
                    {
                        command.Parameters.AddWithValue("@DepartmentCode", selectedCode);
                        command.Parameters.AddWithValue("@DepartmentName", selectedName);

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            // command.ExecuteNonQuery() = SqlCommand를 실행하여 DB에서 영향을 받은 행의 수를 반환하는 메서드. sql 쿼리를 실행할 때 사용됨. 실행된 쿼리에 따라 DB    에서 영향을 받은 행의 수가 반환됨
                            // ExecuteNonQuery() 데이터 변경 쿼리를 실행하고 그 결과를 확인하는 데 사용
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("성공적으로 삭제되었습니다.!");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("삭제할 데이터를 찾을 수 없습니다.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("삭제 중 오류 발생: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("삭제할 항목이 선택되지 않았습니다.");
            }
        }

        public void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
