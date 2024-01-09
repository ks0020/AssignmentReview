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
    public partial class DepartmentAdd : Form
    {
        public DepartmentAdd()
        {
            InitializeComponent();

            AddBtn.Click += Add;
            CloseBtn.Click += Close;
        }
        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";

        public void Add(object sender, EventArgs e)
        {
            // 윈폼에 있는 각 부서 코드와 부서 명을 받아 변수에 저장
            string departmentCode = CodeText.Text;
            string departmentName = NameText.Text;


            if (string.IsNullOrEmpty(departmentCode) || string.IsNullOrEmpty(departmentName)) // IsNullOrEmpty() 메서드는 주어진 문자열이 null이거나 비어있는지 여부를 확인하는 데 사용
            {
                MessageBox.Show("필수 정보를 기입해주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO dbo.department (부서코드, 부서명, 메모) VALUES (@DepartmentCode, @DepartmentName, @Memo)";
                    SqlCommand command = new SqlCommand(query, connection);
                    //SqlCommand 객체를 생성. SqlCommand 객체는 SQL 쿼리를 실행할 준비를 하고, query에 지정된 SQL 문장을 나타내며, 이를 connection에 지정된 DB 연결에 대해 실행할 것
                    command.Parameters.AddWithValue("@DepartmentCode", departmentCode);
                    // Parameters: SqlCommand의 매개변수 컬렉션을 나타내는 속성. 이 컬렉션은 SQL 쿼리에 매개변수를 추가하는 데 사용.
                    // .AddWithValue("@DepartmentCode", departmentCode): SQL 쿼리에 매개변수를 추가하는 메서드 중 하나.
                    // @DepartmentCode: SQL 쿼리에서 사용할 매개변수 이름. 일반적으로 SQL 쿼리에서 @ 기호를 사용하여 매개변수를 지정.
                    // departmentCode: 해당 매개변수에 지정된 값. 매개변수의 실제 값을 대체하여 SQL 쿼리를 실행할 때 사용됩니다.
                    command.Parameters.AddWithValue("@DepartmentName", departmentName);
                    command.Parameters.AddWithValue("@Memo", MemoText.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    // .ExecuteNonQuery(): SqlCommand 객체를 사용하여 SQL 쿼리를 실행하고, 영향을 받은 행의 수를 반환. 이 메서드는 SELECT 쿼리가 아닌 INSERT, UPDATE, DELETE 등의 데이터 변경 쿼리를 실행할 때 사용된다.
                    // int rowsAffected = command.ExecuteNonQuery();: ExecuteNonQuery() 메서드의 반환값을 rowsAffected라는 정수형 변수에 저장.

                    if (rowsAffected > 0)
                    // DB 작업을 수행한 후, 즉시 그 작업에 의해 한 행 이상이 DB에서 변경되었다면 아래의 코드 블록을 실행. 이를 통해 DB 작업이 성공적으로 실행되어, 적어도 한 개 이상의 레코드가 영향을 받았는지 확인할 수 있다.
                    {
                        MessageBox.Show("성공적으로 추가가 되었습니다!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("세부정보를 저장하지 못했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex) 
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show("부서코드가 중복되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
