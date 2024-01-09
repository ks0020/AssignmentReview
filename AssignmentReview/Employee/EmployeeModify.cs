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
    public partial class EmployeeModify : Form
    {
        public EmployeeModify(string departmentCode, string departmentName, string employeeCode, string employeeName, string position, string employmentType, string phoneNumber, string email, string messengerID)
        {
            InitializeComponent();

            // 부서 코드 콤보 박스와 다른 필드들에 데이터 할당
            CodeCombo.Text = departmentCode;

            DepartmentNameText.Text = departmentName;
            EmployeeCodeText.Text = employeeCode;
            EmployeeNameText.Text = employeeName;
            PositionText.Text = position;
            TypeText.Text = employmentType;
            ContactText.Text = phoneNumber;
            EmailText.Text = email;
            MessengerText.Text = messengerID;

            // 부서 코드 콤보박스에 데이터 바인딩
            BindDepartmentCodes();

            // 부서 코드 콤보박스 선택 변경 이벤트 핸들러 등록
            CodeCombo.SelectedIndexChanged += DepartmentNameSelect;


            ModifyBtn.Click += Modify;
            CloseBtn.Click += Close;
        }
        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";

        // 콤보 박스에 부서코드 적용
        private void BindDepartmentCodes()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 부서코드 FROM department";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    // SqlDataReader : ADO.NET에서 제공하는 클래스.
                    // DB로부터 데이터를 읽어오는데 사용한다. DB에서 데이터를 읽어오는데 효율적이며, 데이터를 한 번에 읽어올 수 있다. 연결된 데이터 소스에서 데이터를 순차적으로 읽어오면서, 읽은 데이터를 바로 처리할 수 있도록 해준다.
                    // ExecuteReader : SqlCommand 객체를 사용하여 DB에서 데이터를 읽어오는 메서드. SELECT 쿼리를 실행하고 그 결과를 가져오는데 사용한다.
                    // 쿼리를 실행하고 DB에서 데이터를 가져오며, 가져온 데이터에 대한 읽기 전용 스트림을 제공. 이 스트림을 통해 데이터를 읽으면서 DB연결이 계속 유지된다.

                    while (reader.Read())
                    {
                        string departmentCode = reader["부서코드"].ToString();
                        CodeCombo.Items.Add(departmentCode);
                    }
                    // reader.Read() 메서드는 다음행으로 이동하며, 해당 행에 데이터가 있을 경우 true를 반환. 
                    // while 루프는 DB에서 한 행씩 데이터를 읽어오면서 반복된다.
                    // 각 행에서 SqlDataReader를 사용해서 데이터를 읽을 때는 열의 이름이나 인덱스를 통해 해당 열의 값을 가져올 수 있다. 
                    // 가져온 부서 코드 값을 CodeCombe에 추가한다.
                    // Items.Add() 메서드를 사용해서 추가한다.
                        // Items.Add() : .NETFramework의 컨트롤에서 사용되는 메서드.
                        // 주로 ComboBox나 ListBox와 같은 컨트롤에서 아이템을 추가하거나 목록을 만들 때 사용된다. 

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류: " + ex.Message);
                }
            }
        }

        // 선택된 부서 코드로 해당 부서명 가져오기
        private string GetDepartmentName(string selectedCode)
        {
            string departmentName = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 부서명 FROM department WHERE 부서코드 = @DepartmentCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentCode", selectedCode);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        departmentName = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류: " + ex.Message);
                }
            }

            return departmentName;
        }

        // 부서 코드 콤보박스 선택 변경 이벤트 핸들러
        private void DepartmentNameSelect(object sender, EventArgs e)
        {
            string selectedCode = CodeCombo.SelectedItem.ToString();
            string departmentName = GetDepartmentName(selectedCode);
            DepartmentNameText.Text = departmentName;
        }

        // 이메일 형식이 맞는지 체크
        private bool EmailCheck(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                // var : C#의 키워드 중 하나로, 변수를 선언할 때 사용한다. 컴파일러가 변수의 데이터 형식을 추론하여 할당된 값을 기반으로 결정하도록 한다.
                // System.Net.Mail.MailAddress  = .NET Framework에서 제공하는 클래스 중 하나로, 이메일 주소를 나타내고 관리하는 데 사용한다.
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // 수정하기
        private void Modify(object sender, EventArgs e)
        {
            // 필수 정보 확인
            if (string.IsNullOrEmpty(CodeCombo.Text) || string.IsNullOrEmpty(EmployeeCodeText.Text) || string.IsNullOrEmpty(EmployeeNameText.Text))
            {
                MessageBox.Show("필수 정보를 기입해주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 이메일 주소 형식 확인 단, 필수값은 아니므로 없다면 지나간다.
            if (EmailText.Text != "")
            {
                if (!EmailCheck(EmailText.Text))
                {
                    MessageBox.Show("올바른 이메일 주소 형식이 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE dbo.employee SET 부서코드 = @DepartmentCode, 부서명 = @DepartmentName, 사원명 = @EmployeeName, 직위 = @Position, 고용형태 = @Type, 휴대전화 = @Contact, 이메일 = @Email, 메신저ID = @MessengerId, 메모 = @Memo WHERE 사원코드 = @EmployeeCode";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DepartmentCode", CodeCombo.Text);
                    command.Parameters.AddWithValue("@DepartmentName", DepartmentNameText.Text);
                    command.Parameters.AddWithValue("@EmployeeCode", EmployeeCodeText.Text);
                    command.Parameters.AddWithValue("@EmployeeName", EmployeeNameText.Text);
                    command.Parameters.AddWithValue("@Position", PositionText.Text);
                    command.Parameters.AddWithValue("@Type", TypeText.Text);
                    command.Parameters.AddWithValue("@Contact", ContactText.Text);
                    command.Parameters.AddWithValue("@Email", EmailText.Text);
                    command.Parameters.AddWithValue("@MessengerId", MessengerText.Text);
                    command.Parameters.AddWithValue("@Memo", MemoText.Text);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("성공적으로 수정이 되었습니다!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("세부정보를 저장하지 못했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러발생: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Close(object sender, EventArgs e)
        {
            // 폼 닫기
            this.Close();
        }
    }
}
