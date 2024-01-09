using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssignmentReview
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            LoginBtn.Click += LoginInfo;
            CloseBtn.Click += Close;
        }
        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";

        // 비밀번호 해시
        private string HashPassword(string password)
        {
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder Sbuild = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++)
                {
                    Sbuild.Append(bytes[i].ToString("x2"));
                }
                return Sbuild.ToString();
            }
        }

        // 로그인
        private void LoginInfo(object sender, EventArgs e)
        {
            // 유저 정보 받기
            if(string.IsNullOrEmpty(UserId.Text) || string.IsNullOrEmpty(UserPassword.Text))
            {
                MessageBox.Show("아이디와 비밀번호를 기입해주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM dbo.employee WHERE 로그인ID = @UserId AND 비밀번호 = @UserPassword";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", UserId.Text);
                    command.Parameters.AddWithValue("@UserPassword", HashPassword(UserPassword.Text));
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // DB에서 조회된 결과가 적어도 한 개 이상의 행을 포함하는지를 알려준다.
                                        // HasRows = SqlDataReader객체의 속성 중 하나로, 해당 SqlDataReader가 한 개이 상의 행을 포함하고 있는지 여부이다.
                                        // SqlDataReader로 부터 데이터를 읽기 전에 호출하여, 결과 집합에 데이터가 존재하는지 여부를 확인하는데 사용한다. 
                    {
                        while (reader.Read())
                        {
                            string userId = reader["로그인ID"].ToString();
                            string userName = reader["사원명"].ToString();
                            Session.Login(userId, userName); // 로그인 성공 시 사용자 이름을 Session에 저장합니다.
                            MessageBox.Show($"로그인 성공! {userName}님 환영합니다.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("로그인 실패: 유저 정보가 일치하지 않습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러발생: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 로그인 상태 유지하기.
        public static class Session // 로그인 상태를 유지하기 위한 Session 클래스 정의
        {
            public static bool IsLoggedIn // 로그인 상태확인 true라면 로그인, false라면 로그아웃된 상태임
            { 
                get; private set; // 이 속성의 접근 제어를 정의한다.
                // get : 속성을 읽을 수 있는 접근자를 나타낸다. 외부 코드에서 속성 값을 읽을 수 있다.
                // private set : 속성을 설정할 수 있는 접근자를 나타낸다. 여기서는 private로 접근을 제한했기 때문에, 클래스 내부에서만 속성값을 변경할 수 있다.
            }
            public static string UserId // 로그인된 사용자의 상태를 유지하는 데에는 사용자 식별 정보가 필요하다
            { 
                get; private set; 
            }
            public static string UserName { get; private set; } // 사용자 이름 추가

            public static void Login(string userId, string userName) 
            {
                IsLoggedIn = true;
                UserId = userId;
                UserName = userName;
            }

            public static void Logout()
            {
                IsLoggedIn = false;
                UserId = null;
                UserName = null;
            }
        }
        

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
