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
    public partial class Form1 : Form
    {
        private DataTable foodTable;
        public Form1()
        {
            InitializeComponent();
        }
        public void LoadCategory()
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection conn = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Name FROM Category";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Open();

            adapter.Fill(dt);
            conn.Close();
            conn.Dispose();
            cbbCategory.DataSource = dt;
            cbbCategory.DisplayMember = "Name";
            cbbCategory.ValueMember = "ID";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.LoadCategory();
        }

        public void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbCategory.SelectedIndex == -1) return;
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection conn = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Food WHERE FoodCategoryID = @categoryId";
            // truyền tham số
            cmd.Parameters.Add("@categoryId", SqlDbType.Int);
            if(cbbCategory.SelectedValue is DataRowView)

            {
                DataRowView rowView = cbbCategory.SelectedValue as DataRowView;
                cmd.Parameters["@categoryId"].Value = rowView["ID"];
            }
            else
            {
                cmd.Parameters["@categoryId"].Value = cbbCategory.SelectedValue;
            }
            //tạo bộ điều khiển dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            foodTable = new DataTable();

            conn.Open();
            adapter.Fill(foodTable);
            conn.Close();
            conn.Dispose();
            dgvFoodList.DataSource = foodTable;
            lbQuantity.Text = foodTable.Rows.Count.ToString();
            lbCatName.Text = cbbCategory.Text;
        }

        private void dgvFoodList_Click(object sender, EventArgs e)
        {
            string connectionString = "server=LAPTOP-RPQ3FKVN\\SQLEXPRESS; database = Restaurant; Integrated Security = true ;";
            // tạo đối tượng kết nối
            SqlConnection conn = new SqlConnection(connectionString);
            // tạo đối tượng thực thi lệnh
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT @numSaleFood = sum (Quantity) FROM BillDetails WHERE FoodID = @foodId";
            
            // Lấy thông tin sản phẩm được chọn 
            if(dgvFoodList.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvFoodList.SelectedRows[0];
                DataRowView rowView = selectedRow.DataBoundItem as DataRowView;

                //truyền tham số
                cmd.Parameters.Add("@foodId", SqlDbType.Int);
                cmd.Parameters["@foodId"].Value = rowView["ID"];

                cmd.Parameters.Add("@numSaleFood", SqlDbType.Int);
                cmd.Parameters["@numSaleFood"].Direction = ParameterDirection.Output;

                // MỞ kết nối csdl
                conn.Open();

                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@numSaleFood"].Value.ToString();
                MessageBox.Show("Tổng số lượng món " + rowView["Name"] + " đã bán là : " + result + " " + rowView["Unit"],"Thông báo ",MessageBoxButtons.OK);
                conn.Close();
            }
            cmd.Dispose();
            conn.Dispose();
        }

        private void tsmAddFood_Click(object sender, EventArgs e)
        {
            FoodInfoForm foodForm = new FoodInfoForm();
            foodForm.FormClosed += new FormClosedEventHandler(foodForm_FormClosed);
            foodForm.Show(this);
        }

        private void foodForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            int index = cbbCategory.SelectedIndex;
            cbbCategory.SelectedIndex = -1;
            cbbCategory.SelectedIndex = index;
        }

        private void tsmUpdateFood_Click(object sender, EventArgs e)
        {
            if(dgvFoodList.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvFoodList.SelectedRows[0];
                DataRowView rowView = selectedRow.DataBoundItem as DataRowView;
                FoodInfoForm foodform = new FoodInfoForm();
                foodform.FormClosed += new FormClosedEventHandler(foodForm_FormClosed);
                foodform.Show(this);
                foodform.DisplayFoodInfo(rowView);
            }
        }

        private void txtSearchByName_TextChanged(object sender, EventArgs e)
        {
            if (foodTable == null) return;

            string filterExpression = "Name Like '%" + txtSearchByName.Text + "%'";
            string sortExpression = "Price DESC";
            DataViewRowState rowStateFilter = DataViewRowState.OriginalRows;

            DataView foodView = new DataView(foodTable, filterExpression, sortExpression, rowStateFilter);


            dgvFoodList.DataSource = foodView;
        }

        private void btnDSHoaDon_Click(object sender, EventArgs e)
        {
            OrdersForm f = new OrdersForm();
            f.ShowDialog();
        }

        private void btnDSTK_Click(object sender, EventArgs e)
        {
            AccountForm f = new AccountForm();
            f.ShowDialog();
        }

       
    }
}
