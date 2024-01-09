using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssignmentReview
{
    public partial class EmployeeAdd : Form
    {
        public EmployeeAdd()
        {
            InitializeComponent();

            // 부서코드 콤보박스에 데이터 바인딩
            BindDepartmentCodes();
            // 부서코드 콤보박스 선택 변경 이벤트 핸들러
            CodeCombo.SelectedIndexChanged += DepartmentNameDisplay;

            AddBtn.Click += Add;
            CloseBtn.Click += Close;
        }

        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";

        // 콤보 박스 부서코드 적용
        private void BindDepartmentCodes()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 부서코드 FROM dbo.department";
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();//ExecuteReader() : SqlCommand를 실행하여 DB로부터 데이터를 읽어오는 데 사용되는 메서드.
                    // SqlDataReader를 사용하여 DB로부터 읽은 행을 하나씩 읽을 수 있음
                    while (reader.Read())
                    // DB로부터 가져온 결과 집합에 대해 반복적으로 데이터를 읽기 위해서 루프
                    {
                        string departmentCode = reader["부서코드"].ToString();
                        // reader["부서코드"].ToString() : reader객체에서 부서코드 열의 값을 가져와서 문자열로 변환
                        CodeCombo.Items.Add(departmentCode);
                        //departmentCode값을 콤보박스의 아이템 컬렉션에 추가. 
                    }
                    reader.Close(); // DataReader를 닫아서 리소스를 해제하고 DB 연결을 종료
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 : " + ex.Message);
                }
            }
        }

        // 부서코드에 해당하는 부서명을 조회하고 반환하는 메서드
        private string GetDepartmentName(string selectedCode)
        {
            string departmentName = string.Empty; // string.Empty : 빈 문자열을 나타내는 C#의 내장된 상수.
            // 이 변수는 초기화되고 나중에 코드에서 어떤 값이든 할당되기 전에 빈 문자열로 초기화 된다.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 부서명 FROM department WHERE 부서코드 = @DepartmentCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentCode", selectedCode);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    // ExecuteScalar() : DB에서 단일값, 하나의 결과만 필요한 경우에 사용된다.
                    if(result != null)
                    {
                        departmentName = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 : " + ex.Message);
                }
            }
            return departmentName;
        }

        // 가져온 부서명을 텍스트박스에 표시하기위한 메서드
        private void DepartmentNameDisplay(object sender, EventArgs e)
        {
            string selectedCode = CodeCombo.SelectedItem.ToString(); // 선택된 항목을 문자열로 반환
            string departmentName = GetDepartmentName(selectedCode); // 위에서 선언한 GetDepartmentName을 호출. GetDepartmentName의 반환값을 departmentName에 할당
            DepartmentNameText.Text = departmentName; // 부서명 텍스트박스에 표시하기
        }

        // 이메일 형식이 맞는지 체크
        private bool EmailCheck(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                // System.Net.Mail.MailAddress 클래스는 .NET Framework에서 제공하는 클래스 중 하나로, 이메일 주소를 나타내고 관리하는 데 사용
                return addr.Address == email; // 비교하여 반환
            }
            catch
            {
                return false; // 형식이 잘못됬다면 false
            }
        }

        // 아이디 중복 체크
        private bool IdCheck(string userId)
        {
            // 기존에 존재하는 아이디가 현재 입력된 아이디와 같은지 셀렉트해와야하기때문에 쿼리를 연결한다.
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM dbo.employee WHERE 로그인ID = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    // 반환된 값은int형태로 count변수에 저장이된다.
                    // 조회를 했을때 조건을 만족하는 레코드가 없다면 0이 반환된다.
                    // ExecuteScalar : ADO.NET에서 사용되는 메서드. DB에서 단일값을 반환할때 사용한다.
                    // SELECT 쿼리를 실행하고 그 결과로 단일 값을 가져올 때 사용한다. 
                    // COUNT(*)를 사용하여 특정 조건에 맞는 레코드 수를 반환하거나, SUM(), AVG() 등의 집계 함수를 사용하여 총합이나 평균값 등을 반환할때 주로 사용됨.
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("에러 발생: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        // 비밀번호 해싱 함수
        private string HashPassword(string passowrd)
        {
            using (SHA256 sha256Hash = SHA256.Create()) // 암호화를 위해 해시 알고리즘 생성
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passowrd));// 패스워드를 바이트 배열로 변환 후, 바이트 배열에 대한 해시계산.
                StringBuilder sBuilder = new StringBuilder();
                // StringBuilder = .NET 프레임워크의 클래스. 가변적인 문자열을 만들 수 있음. 초기에 빈 StringBuilder를 만들고, Append() 메서드를 황용하여 문자열을 계속해서 추가할 수 있음. 마지막에 ToString() 메서드를 활용하여 최종적인 문자열을 얻을 수 있음.
                // 해시된 바이트 배열을 문자열로 변환하기위해 객체 생성
                for (int i = 0; i < bytes.Length; i++)
                {
                    sBuilder.Append(bytes[i].ToString("x2"));
                    // sBuilder에 바이트배열의 각 요소를 16진수 문자열로 변환하여 추가하는 것. 
                    // ToString("x2") = 숫자를 16진수 문자열로 변환하는데 사용됨. 여기서, x2는 문자열 서식 지정자로, 숫자를 16진수 문자열로 변환하되, 최소한 두 자리를 사용하여 표시하라는 의미. 
                    // x = 16진수. 2 = 최소한 두자리를 표시하라는 것을 의미
                    // 큰따옴표는 서식 지정자 "x2"를 문자열로 인식하도록 하기위한것은 아니고, C#언어에서 문자열 리터럴을 표현하는데 사용. C#에서 문자열 리터럴은 큰따옴표로 둘러싸여 있어야 함. 만약 ""를 사용하지 않느다면 컴파일러는 이를 인식하지 못함. 
                }
                return sBuilder.ToString();
            }
        }
        // 저장하기
        public void Add(object sender, EventArgs e)
        {
            // 필수 정보 확인
            if(string.IsNullOrEmpty(CodeCombo.Text) || string.IsNullOrEmpty(EmployeeCodeText.Text) || string.IsNullOrEmpty(EmployeeNameText.Text) || string.IsNullOrEmpty(UserIdText.Text) || string.IsNullOrEmpty(UserPasswordText.Text))
            {
                MessageBox.Show("필수 정보를 입력 해주세요..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 이메일 주소형식 확인
            // 단, 이메일은 필수가 아니므로 비어있다면 그냥 지나친다.
            if(EmailText.Text != "")
            {
                if (string.IsNullOrEmpty(EmailText.Text) || !EmailCheck(EmailText.Text))
                {
                    MessageBox.Show("올바른 이메일 주소 형식이 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // 비밀번호 해싱
            string hashedPassword = HashPassword(UserPasswordText.Text);

            // 비밀번호 유효성 검사
            if (UserPasswordText.Text.Length < 8 || !Regex.IsMatch(UserPasswordText.Text, @"^(?=.*[a-zA-Z])(?=.*[0-9]).{8,}$"))
            // Length < 8 : 문자열의 길이가 8보다 작은지 확인
            // Regex.IsMatch(UserPasswordText.Text, @"^(?=.*[a-zA-Z])(?=.*[0-9]).{8,}$")) : 정규식을 사용해서 조건이 맞는지 확인.
            // (?=.*[a-zA-Z]) : 최소한 하나의 영문자가 포함되어야 함
            // (?=.*[0-9]) : 최소한 하나의 숫자가 포함되어야 함
            // .{8,} : 총 길이가 최소 8자 이상이어야함.
            {
                MessageBox.Show("비밀번호는 8자 이상이어야 하며, 영문과 숫자를 혼용하여야 합니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 아이디 중복 확인
            if (IdCheck(UserIdText.Text))
            {
                MessageBox.Show("이미 존재하는 아이디입니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 저장하기
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO dbo.employee (부서코드, 부서명, 사원코드, 사원명, 로그인ID, 비밀번호, 직위, 고용형태, 휴대전화, 이메일, 메신저ID, 메모) VALUES (@DepartmentCode, @DepartmentName, @EmployeeCode, @EmployeeName, @UserId, @Password, @Position, @Type, @Contact, @Email, @MessengerId, @Memo)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DepartmentCode", CodeCombo.Text);
                    command.Parameters.AddWithValue("@DepartmentName", DepartmentNameText.Text);
                    command.Parameters.AddWithValue("@EmployeeCode", EmployeeCodeText.Text);
                    command.Parameters.AddWithValue("@EmployeeName", EmployeeNameText.Text);
                    command.Parameters.AddWithValue("@UserId", UserIdText.Text);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Position", PositionText.Text);
                    command.Parameters.AddWithValue("@Type", TypeText.Text);
                    command.Parameters.AddWithValue("@Contact", ContactText.Text);
                    command.Parameters.AddWithValue("@Email", EmailText.Text);
                    command.Parameters.AddWithValue("@MessengerId", MessengerText.Text);
                    command.Parameters.AddWithValue("@MeMo", MemoText.Text);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
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
