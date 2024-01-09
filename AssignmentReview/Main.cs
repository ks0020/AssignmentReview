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
using static AssignmentReview.Login;

namespace AssignmentReview
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            label1.Click += DepartmentMove;
            label2.Click += Refresh;
            label3.Click += EmployeeAdd;
            label4.Click += ModifyAdd;
            label5.Click += Login;
            label6.Click += Delete;
            CloseBtn.Click += Close;

            // 패스워드 화면 블러처리하기
            EmployeeTable.CellFormatting += PasswordBlind;
            // 선택된 열 값 저장
            EmployeeTable.CellClick += EmployeeTableCellSave;

            this.Load += MainLoad;
            LoginLogout();
        }
        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";
        
        // 패스워드 블러처리하기
        private void PasswordBlind(object sender, DataGridViewCellFormattingEventArgs e)
        // CellFormatting이벤트 : DataGridView 컨트롤에서 셀의 데이터를 표시블라인드하기 전에 발생하는이벤트
            // 즉, 해당 셀의 데이터가 그리드에 표시되기전에 발생하므로, 데이터를 변겨하거나 특정한 서식으로 표시하고자 할때 사용한다.
        // DataGridViewCellFormattingEventArgs : DataGridView의 CellFormatting이벤트에 사용되는 이벤트 인수.
        // 셀에 표시될 데이터의 서식화, 변환 또는 가공을 수행할 때 유용하다. 
        {
            if (e.ColumnIndex >= 0 && EmployeeTable.Columns[e.ColumnIndex].Name == "비밀번호")
            {
                if (e.Value != null)
                {
                    string PBlind= new string('*', 8); // 비밀번호를 무조건 8자리 '*'로 블러처리

                    e.Value = PBlind;
                }
            }
        }

        // 조회 결과 가져오기
        private void MainLoad(object sender, EventArgs e)
        {

            SqlDataAdapter dataAdapter;
            DataSet dataSet;

            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                try
                {
                    connection.Open();
                    // Open() : 메서드를 호출하면 DB 연결이 열리고, 이후 sql쿼리를 실행하거나 데이터를 가져오는 등의 작업을 수행할 수 있음.
                    // Open() : 메서드를 호출하기 전에는 DB 연결이 닫혀 있으므로, DB 작업을 수행하기 전에 연결을 열어야 함. 연결이 성공하면 DB 작업을 수행하고, 작업이 완료된 후에는 SqlConnection을 닫아 리소스를 효율적으로 관리해야 함.

                    // SQL 쿼리 작성 및 데이터 가져오기
                    string query = "SELECT * FROM dbo.employee"; // employee에서 모든 열 select
                    dataAdapter = new SqlDataAdapter(query, connection); // DB에서 데이터를 가져오기위해 초기화
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet); // SqlDataAdapter를 사용하여 DB에서 데이터를 가져와 DataSet에 채우는 역할
                    // Fill() = SqlDataAdapter를 통해 DB로부터 데이터를 가져와 DataSet의 하나 이상의 DataTable에 채우는데 사용됨. 

                    EmployeeTable.AutoGenerateColumns = true; // DataGridView의 컬럼 자동 생성
                    EmployeeTable.DataSource = dataSet.Tables[0]; // 데이터 그리드 뷰에 데이터 바인딩

                }
                catch (Exception ex)
                {
                    MessageBox.Show("연결 실패: " + ex.Message);
                }
            }
        }

        // 선택된 열 초기화
        private int selectedRowIndex = 0;

        // 선택된 열 값 저장
        private void EmployeeTableCellSave(object sender, DataGridViewCellEventArgs e)
        // 해당 메서드는 DataGridView 컨트롤의 셀이 클릭될 때마다 호출이됨. DataGridViewCellEventArgs를 통해 클릭된 셀의 정보를 가져옴.
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // 선택한 셀이 실제 행과 열 내에 있는 지 확인. RowIndex와 ColumnIndex가 모두 0 이상이어야함 실행이 됨.
            {
                selectedRowIndex = e.RowIndex; // 선택한 행의 인덱스 저장
            }
        }

        
        // 부서로 이동
        private void DepartmentMove(object sender, EventArgs e)
        {
            Department department = new Department();
            department.Show();
        }
        
        // 조회(새로고침)
        private void Refresh(object sneder, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM dbo.employee"; // DGV를 새로고침할 거기 때문에 쿼리를 새롭게 불러오면된다.
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataSet dataSet = new DataSet();

                    dataAdapter.Fill(dataSet);
                    EmployeeTable.DataSource = dataSet.Tables[0]; // DataGridView에 데이터 바인딩
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("새로고침 실패: " + ex.Message); ;
            }
        }

        // 사원추가
        private void EmployeeAdd(object sender, EventArgs e)
        {
            EmployeeAdd EAdd = new EmployeeAdd();
            EAdd.Show();
        }
        // 사원수정
        private void ModifyAdd(object sender, EventArgs e)
        {
            if (selectedRowIndex >= 0)
            {
                // DataGridView에서 선택한 행의 각 셀의 데이터를 가져옴
                string departmentCode = EmployeeTable.Rows[selectedRowIndex].Cells["부서코드"].Value?.ToString();
                string departmentName = EmployeeTable.Rows[selectedRowIndex].Cells["부서명"].Value?.ToString();
                string employeeCode = EmployeeTable.Rows[selectedRowIndex].Cells["사원코드"].Value?.ToString();
                string employeeName = EmployeeTable.Rows[selectedRowIndex].Cells["사원명"].Value?.ToString();
                string position = EmployeeTable.Rows[selectedRowIndex].Cells["직위"].Value?.ToString();
                string employmentType = EmployeeTable.Rows[selectedRowIndex].Cells["고용형태"].Value?.ToString();
                string phoneNumber = EmployeeTable.Rows[selectedRowIndex].Cells["휴대전화"].Value?.ToString();
                string email = EmployeeTable.Rows[selectedRowIndex].Cells["이메일"].Value?.ToString();
                string messengerID = EmployeeTable.Rows[selectedRowIndex].Cells["메신저ID"].Value?.ToString();

                // EmployeeModify 폼의 콤보 박스와 텍스트 상자에 데이터 할당
                EmployeeModify employeeModifyForm = new EmployeeModify(departmentCode, departmentName, employeeCode, employeeName, position, employmentType, phoneNumber, email, messengerID);
                employeeModifyForm.Show();
            }
            else
            {
                MessageBox.Show("수정할 사원을 선택해주세요.");
            }
        }
        
        // 로그인
        private void Login(object sender, EventArgs e)
        {
            if (Session.IsLoggedIn)
            {
                string welcomeMessage = $"안녕히 가세요, {Session.UserName} 님!";
                MessageBox.Show(welcomeMessage, "로그아웃", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 이미 로그인된 상태에서 버튼 클릭 시 로그아웃 처리
                /*MessageBox.Show( "로그아웃 되었습니다.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                Session.Logout();
                Application.Restart();
            }
            else
            {
                
                // 로그인 폼을 띄워 로그인 처리
                Login loginForm = new Login();
                loginForm.ShowDialog();

                // 로그인 폼에서 로그인 성공하면 세션에 정보가 업데이트되므로 다시 버튼 라벨 업데이트
                LoginLogout();
            }
        }

        // 로그인 정보 라벨 텍스트 변경하기
        private void LoginLogout()
        {
            if (Session.IsLoggedIn)
            {
                label5.Text = "로그아웃";
            }
            else
            {
                label5.Text = "로그인 정보";
            }
        }

        // 사원 삭제
        private void Delete(object sender, EventArgs e)
        {
            string selectedCode = "";
            string selectedName = "";
            if(selectedRowIndex >= 0) // 선택된 행이 있는지 확인
            {
                selectedCode = EmployeeTable.Rows[selectedRowIndex].Cells["사원코드"].Value?.ToString();
                selectedName = EmployeeTable.Rows[selectedRowIndex].Cells["사원명"].Value?.ToString();

            }
            EmployeeDelete EDlete = new EmployeeDelete(selectedCode, selectedName);
            EDlete.Show();
        }

        public void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
