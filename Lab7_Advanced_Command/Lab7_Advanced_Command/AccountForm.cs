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
    public partial class AccountForm : Form
    {
        public AccountForm()
        {
            InitializeComponent();
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {
            Load_Account();
        }
        private void Load_Account()
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection sqlconnection = new SqlConnection(connectionString);
            sqlconnection.Open();
            // viết câu truy vấn
            String sql = "SELECT* FROM Account";
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter(sql, sqlconnection);
            dap.Fill(ds);
            // gắn dữ liệu  lên datagrid
            dgvAccount.DataSource = ds.Tables[0];
            dgvAccount.Refresh();
        }
        private void ResetForm()
        {
            txtAcountName.Text = "";
            txtPassword.Text = "";
            txtFullname.Text = "";
            txtEmail.Text = "";
            txtTell.Text = "";
            btnUpdate.Enabled = false;
         
        }
        private void dgvAccount_Click(object sender, EventArgs e)
        {
            int index = dgvAccount.CurrentRow.Index;
            if (index != null)
            {
                txtAcountName.Text = dgvAccount.Rows[index].Cells["AccountName"].Value.ToString();
                txtPassword.Text = dgvAccount.Rows[index].Cells["Password"].Value.ToString();
                txtFullname.Text = dgvAccount.Rows[index].Cells["FullName"].Value.ToString();
                txtEmail.Text = dgvAccount.Rows[index].Cells["Email"].Value.ToString();
                txtTell.Text = dgvAccount.Rows[index].Cells["Tell"].Value.ToString();

                btnUpdate.Enabled = true;               
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection conn = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "EXECUTE Update_Account @AccountName,@Password,@Fullname,@Email,@Tell,@DateCreated";

            cmd.Parameters.Add("@AccountName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 200);
            cmd.Parameters.Add("@Fullname", SqlDbType.NVarChar, 1000);
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 1000);
            cmd.Parameters.Add("@Tell", SqlDbType.NVarChar, 200);
            cmd.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime);

            cmd.Parameters["@AccountName"].Value = txtAcountName.Text;
            cmd.Parameters["@Password"].Value = txtPassword.Text;
            cmd.Parameters["@Fullname"].Value = txtFullname.Text;
            cmd.Parameters["@Email"].Value = txtEmail.Text;
            cmd.Parameters["@Tell"].Value = txtTell.Text;
            cmd.Parameters["@DateCreated"].Value = DateTime.Now.ToShortDateString();
            conn.Open();

            int numOfRows = cmd.ExecuteNonQuery();

            if (numOfRows == 1)
            {
                Load_Account();
                ResetForm();
                MessageBox.Show("Cap nhat tai khoan thanh cong");
            }
            else
            {
                MessageBox.Show("Lỗi", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }

            conn.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection conn = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "EXECUTE Insert_Account @AccountName,@Password,@Fullname,@Email,@Tell,@DateCreated";

            cmd.Parameters.Add("@AccountName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 200);
            cmd.Parameters.Add("@Fullname", SqlDbType.NVarChar, 1000);
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 1000);
            cmd.Parameters.Add("@Tell", SqlDbType.NVarChar, 200);
            cmd.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime);

            cmd.Parameters["@AccountName"].Value = txtAcountName.Text;
            cmd.Parameters["@Password"].Value = txtPassword.Text;
            cmd.Parameters["@Fullname"].Value = txtFullname.Text;
            cmd.Parameters["@Email"].Value = txtEmail.Text;
            cmd.Parameters["@Tell"].Value = txtTell.Text;
            cmd.Parameters["@DateCreated"].Value = DateTime.Now.ToShortDateString();

            conn.Open();

            int numOfRows = cmd.ExecuteNonQuery();

            if (numOfRows == 1)
            {
                Load_Account();
                ResetForm();
                MessageBox.Show("Them tai khoan thanh cong");
            }
            else
            {
                MessageBox.Show("Loi", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }

            conn.Close();
        }
    }
    
}
