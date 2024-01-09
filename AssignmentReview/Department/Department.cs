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
    public partial class Department : Form
    {
        public Department()
        {
            InitializeComponent();

            AddBtn.Click += Add;
            ModifyBtn.Click += Modify;
            DeleteBtn.Click += Delete;
            closeBtn.Click += Close;

            
            DepartmentTable.CellClick += DepartmentGridView1_CellClick;
            this.Load += TableLoad;
        }

        string connectionString = @"Data Source=DESKTOP-80CKK65;Initial Catalog=Project001;Integrated Security=True";
        
        // 선택된 행의 인덱스 초기화 변수
        private int selectedRowIndex = 0;

        // 셀 클릭시 저장해서 띄우기
        private void DepartmentGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        // DataGridViewCellEventArgs e: DataGridView 셀 이벤트에 대한 정보를 제공. RowIndex와 ColumnIndex 등의 속성을 포함.
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                // e.RowIndex: 클릭한 셀의 행 인덱스.
                // e.ColumnIndex: 클릭한 셀의 열 인덱스.
            {
                selectedRowIndex = e.RowIndex; // 선택한 행의 인덱스 저장
            }
        }

        // DB에서 DepartmentTable 불러오기
        public void TableLoad(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter; //SqlDataAdapter 클래스는 ADO.NET 프레임워크에서 SQL DB와 상호 작용할 때 사용되는 중요한 클래스 중 하나이다.
                                        //이 클래스는 DB와 데이터를 주고받을 때 중개 역할을 수행하며, DB로부터 데이터를 가져오거나 DB에 변경된 데이터를 저장하는 데 사용된다.
            DataSet dataSet; // DataSet 클래스는 메모리 내에 데이터를 보유하는 ADO.NET 클래스이다.
                             // DB로부터 가져온 데이터를 임시로 저장하거나 애플리케이션 내에서 데이터를 조작하고 변경한 후에 DB에 다시 저장하는 데 사용됩니다.

            using (SqlConnection connection = new SqlConnection(connectionString)) //C#에서 DB에 연결할 때 사용되는 SqlConnection 클래스를 활용하는 부분.
                // using: using 키워드는 C#에서 리소스를 사용한 후 자동으로 정리 및 해제하기 위해 사용. using 블록 내에서 사용된 리소스(여기서는 SqlConnection)는 블록을 벗어날 때 자동으로 Dispose() 메서드가 호출되어 리소스를 정리한다.
                // SqlConnection connection = new SqlConnection(connectionString): SqlConnection 클래스의 인스턴스를 만든다. 이를 통해 SQL Server DB에 연결.
                // connectionString은 DB 연결에 필요한 정보를 담고 있는 문자열이다. 이 연결 문자열은 DB 서버의 주소, 인증 방법, DB 이름 등을 포함할 수 있습니다.
                // using 블록 내부에 DB 연결에 필요한 작업을 구현한다. 이 블록 내에서 DB 작업을 수행하고 연결을 열고 닫을 수 있다.
                // 예를 들어, DB에서 데이터를 가져오거나 DB에 쿼리를 실행하는 등의 작업을 수행할 수 있다.
                // using 블록을 벗어나면 Dispose() 메서드가 호출되어 SqlConnection 객체가 소멸되고 연결이 닫힌다. 이렇게 하면 연결 리소스가 자동으로 해제되어 메모리 누수를 방지한다.
                // using 문을 사용하면 개발자가 명시적으로 .Dispose() 메서드를 호출하지 않아도 리소스를 해제할 수 있으므로 코드가 더 안전하고 간결해진다. DB 연결과 같은 리소스를 다룰 때 using 문을 사용하는 것이 권장된다.
            {
                try
                {
                    connection.Open();

                    // SQL 쿼리 작성 및 데이터 가져오기
                    string query = "SELECT * FROM dbo.department";
                    dataAdapter = new SqlDataAdapter(query, connection); // SqlDataAdapter를 인스턴스화하고 설정하는 부분. 이 코드는 DB에서 데이터를 가져오기 위해 SqlDataAdapter를 사용
                    // new SqlDataAdapter(query, connection): SqlDataAdapter의 인스턴스를 생성. 생성자에는 두 개의 매개변수가 있다.
                    // query: DB에서 실행할 SQL 쿼리 또는 저장 프로시저의 문자열을 나타낸다. DB에서 가져올 데이터를 지정하는 쿼리가 여기에 포함된다.
                    // connection: SqlConnection 객체를 나타내며, DB에 연결하기 위한 연결 정보를 제공. SqlConnection은 DB연결을 나타내며, SqlDataAdapter가 해당 연결을 사용하여 데이터를 가져온다.
                    dataSet = new DataSet(); // new DataSet()은 DataSet 클래스의 새 인스턴스를 생성한다. 이 새로운 DataSet 객체는 메모리 내에 데이터를 보관하고 처리하는 데 사용될 것
                    dataAdapter.Fill(dataSet); // SqlDataAdapter를 사용하여 데이터베이스에서 데이터를 가져와서 DataSet에 채우는 역할
                    // Fill() 메서드는 SqlDataAdapter를 사용하여 DB로부터 데이터를 가져와서 DataSet에 채우는 역할. 이 메서드를 호출하면 SqlDataAdapter는 DB로부터 데이터를 가져오고, 그 결과를 DataSet에 로드하여 DataSet을 채운다.

                    DepartmentTable.AutoGenerateColumns = true; // DataGridView의 컬럼 자동 생성
                    // AutoGenerateColumns: 이 속성은 데이터 소스의 데이터를 기반으로 DataGridView 컨트롤에 자동으로 열(Column)을 생성할지 여부를 나타낸다.
                    // true로 설정하면 DataGridView 컨트롤이 데이터 소스에 있는 데이터를 기반으로 자동으로 열을 생성.
                    // false로 설정하면 데이터 소스의 데이터를 기반으로 자동으로 열을 생성하지 않으며, 개발자가 수동으로 열을 추가해야 한다.
                    DepartmentTable.DataSource = dataSet.Tables[0]; // 데이터 그리드 뷰에 데이터 바인딩
                    // dataSource: 이 속성은 DataGridView 컨트롤에 바인딩될 데이터를 나타낸다.
                    // dataSet.Tables[0]: 이 부분은 DB로부터 가져온 데이터가 담긴 DataSet의 첫 번째 테이블(0번 인덱스)을 의미한다.

                }
                catch (Exception ex)
                {
                    MessageBox.Show("연결 실패: " + ex.Message);
                }
            }
        }
        // 부서 추가
        public void Add(object sender, EventArgs e)
        {
            DepartmentAdd DAdd = new DepartmentAdd();
            DAdd.Show();
        }
        //부서 수정
        public void Modify(object sender, EventArgs e)
        {
            // 선택된 행의 데이터를 저장하기 위한 문자열변수
            string selectedCode = "";
            string selectedName = "";
            // 선택된 행의 인덱스 확인
            if (selectedRowIndex >= 0)
            {
                // 선택한 행의 데이터 가져오기
                selectedCode = DepartmentTable.Rows[selectedRowIndex].Cells["부서코드"].Value?.ToString();
                selectedName = DepartmentTable.Rows[selectedRowIndex].Cells["부서명"].Value?.ToString();
            }
            DepartmentModify DModify = new DepartmentModify(selectedCode, selectedName); // 선택된 부서 코드와 이름을 매개변수로 전달
            DModify.Show();
        }
        // 부서 삭제
        public void Delete(object sender, EventArgs e)
        {
            string selectedCode = "";
            string selectedName = "";
            if (selectedRowIndex >= 0)
            {
                selectedCode = DepartmentTable.Rows[selectedRowIndex].Cells["부서코드"].Value?.ToString();
                selectedName = DepartmentTable.Rows[selectedRowIndex].Cells["부서명"].Value?.ToString();

            }
            DepartmentDelete DDelete = new DepartmentDelete(selectedCode, selectedName);
            DDelete.Show();
        }
        // 닫기
        public void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
