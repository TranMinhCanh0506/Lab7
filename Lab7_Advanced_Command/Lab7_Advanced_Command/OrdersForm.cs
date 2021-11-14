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
    public partial class OrdersForm : Form
    {
        public OrdersForm()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection conn = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "EXECUTE Bills_View @date";

            cmd.Parameters.Add("@date", SqlDbType.SmallDateTime);
            cmd.Parameters["@date"].Value = dtpHDNgay.Value.ToShortDateString();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Open();      
            adapter.Fill(dt);
            cmd.CommandText = "Select SUM(Amount) from Bills where CheckoutDate = @date";
            var doanhThu = cmd.ExecuteScalar();
            lbDoanhthu.Text = doanhThu.ToString();

            conn.Close();

            dgvBills.DataSource = dt;
            dgvBills.Columns[0].ReadOnly = true;


        }

        private void dgvBills_DoubleClick(object sender, EventArgs e)
        {
            var billID = dgvBills.SelectedRows[0].Cells[0].Value.ToString();

            OrderDetailsForm frm = new OrderDetailsForm();
            frm.Load(billID);
            frm.ShowDialog(this);
        }
    }
}
