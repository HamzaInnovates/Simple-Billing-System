using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Simple_Billing_System
{
    public partial class View : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.SimpleBillingCon);
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        public View()
        {
            InitializeComponent();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void View_Load(object sender, EventArgs e)
        {// shows all data from headertbl
            textBox1.Hide();
            con.Open();
            da = new SqlDataAdapter("select * from TblHeadData order by BillNo", con);
            con.Close();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            Totalbillsales();
            textBox1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        { //view bills that were generated between two dates (user can set the date for which dates he want to see details of the bill
            con.Open();
            da = new SqlDataAdapter("select*from TblHeadData where BillDate between '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "'And'" + dateTimePicker2.Value.ToString("MM/dd/yyyy") + "'order by BillNo  ", con);
            DataSet ds = new DataSet();
            da.Fill(ds,"TblHeadData");
            dataGridView1.DataSource = ds.Tables["TblHeadData"];
            con.Close();
            Totalbillsales();

        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // when cell clicked update form will be displayed in which we can add, del , updateBill
            UpdateBill ub = new UpdateBill();
            // textboxes fill be filled by this code 
            ub.txtBillNo.Text = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            ub.txtBillTotal.Text = this.dataGridView1.CurrentRow.Cells[2].Value.ToString();
            ub.txtDisAmount.Text = this.dataGridView1.CurrentRow.Cells[3].Value.ToString();
            ub.txtNetPay.Text = this.dataGridView1.CurrentRow.Cells[4].Value.ToString();
            ub.Show();
            // hides tge current form
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {// to delete bills
            if (textBox1.Text=="")
            {
                MessageBox.Show("Please Select a Bill To delete");
            }
            else
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd = new SqlCommand("Delete from TblHeadData where BillNo='"+textBox1.Text+"'", con);
                cmd.ExecuteNonQuery();
                cmd1 = new SqlCommand("Delete from TblRowData where BillNo = '" + textBox1.Text + "'", con);
               
                cmd1.ExecuteNonQuery();
                con.Close();
                fillgrid();
                MessageBox.Show("Bill Deleted");

                textBox1.Text = "";
                Totalbillsales();
            }
        }
        void fillgrid()
        { // to fill grid rows according to bills 
            con.Open();
            da = new SqlDataAdapter("select * from TblHeadData order by BillNo", con);
            con.Close();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            Totalbillsales();
        } 
        //method to calculate aa of the sales 
        void Totalbillsales()
        {
            txtTotalBills.Text = dataGridView1.Rows.Count.ToString();
               double sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                
               sum +=Convert.ToDouble( dataGridView1.Rows[i].Cells[4].Value);
            }
            txtTotalSales.Text = sum.ToString();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        { // to load Bill No is txtbox
            DataGridViewRow dr = dataGridView1.SelectedRows[0];
            textBox1.Text = dr.Cells[0].Value.ToString();
        }
    }
}
