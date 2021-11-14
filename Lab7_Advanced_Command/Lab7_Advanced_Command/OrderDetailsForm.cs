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
    public partial class OrderDetailsForm : Form
    {
        public OrderDetailsForm()
        {
            InitializeComponent();
        }
        public void Load(string billID)
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection conn = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "EXECUTE BillDetails_View @id";
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = billID;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            conn.Open();

            adapter.Fill(dt);
            dgvBillDetails.DataSource = dt;
            dgvBillDetails.Columns[0].ReadOnly = true;

            conn.Close();
        }
    }
}
