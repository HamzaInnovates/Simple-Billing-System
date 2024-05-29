using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Simple_Billing_System
{
    public partial class Products : Form
    {  
        SqlConnection con = new SqlConnection(Properties.Settings.Default.SimpleBillingCon);
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            Fill();
        }

        private void btnSave_Click(object sender, EventArgs e)
        { if (txtProName.Text == "")
            {
                MessageBox.Show("Please Enter Product Name");
            }
            else
            {
                try
                {
                    // insertion in tbl products
                    con.Open();
                    cmd = new SqlCommand("Insert into TblProducts(ProName,ProPrice) VALUES('" + txtProName.Text + "','"+txtPrice.Text+"')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    Fill();
                    MessageBox.Show("Record Saved");
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        void Fill()
        { // method to fill gridview rows accordiing to data 
            con.Open();
            da = new SqlDataAdapter("select * from TblProducts order by ProName asc", con);
            con.Close();
            SqlCommandBuilder cd = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        int i;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        { //loads the values in txt boxes again to update,del
            i = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[i];
            txtProID.Text = row.Cells[0].Value.ToString();
            txtProName.Text = row.Cells[1].Value.ToString();
            txtPrice.Text = row.Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        { if (txtProID.Text == "")
            {
                MessageBox.Show("Please select to update");
            }
            else
            {
                try
                { //updation in Tbl Products
                    con.Open();
                    cmd = new SqlCommand("UPDATE TblProducts set ProName='" + txtProName.Text + "',ProPrice='"+txtPrice.Text+"' where ProID='" + txtProID.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                    Fill();
                    MessageBox.Show("Record Updated");
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        void clear()
        { // method to clear txtboxes
            txtProID.Clear();
            txtProName.Clear();
            txtPrice.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        { if (txtProID.Text == "")
            {
                MessageBox.Show("Please select product to delete");
            }
            else
            {
                try
                {
                    con.Open();
                    cmd = new SqlCommand("Delete from TblProducts where ProID='" + txtProID.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                    Fill();
                    MessageBox.Show("Record Deleted");
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
