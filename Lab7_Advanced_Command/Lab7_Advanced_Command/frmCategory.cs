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

namespace Lab7_Advanced_Command
{
    public partial class frmCategory : Form
    {
        public frmCategory()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection sqlconnection = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand sqlcommand = sqlconnection.CreateCommand();
            sqlcommand.CommandText = "INSERT INTO Category (Name, [Type])" +
                "VALUES (N'" + txtName.Text + "' , " + txtType.Text + ")";
            sqlconnection.Open();
            int numOfRowsEffeted = sqlcommand.ExecuteNonQuery();
            sqlconnection.Close();
            if (numOfRowsEffeted == 1)
            {
                MessageBox.Show(" Thêm nhóm món ăn thành công");              
                txtName.Text = "";
                txtType.Text = "";
            }
            else
            {
                MessageBox.Show(" Đã có lỗi xảy ra . Vui lòng thử lại");
            }
        }

       
    }
}
