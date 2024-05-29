using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Simple_Billing_System
{
    public partial class UpdateBill : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.SimpleBillingCon);
        SqlCommand cmd;
        //global variable
        int b;
        public UpdateBill()
        {
            InitializeComponent();
        }

        private void UpdateBill_Load(object sender, EventArgs e)
        { // load bills acoording to their Bill No and shows on new form (Update Bills)
            txtDeleteUpdate.Visible = false;
            con.Open();
            cmd = new SqlCommand("Select * from TblRowData where BillNo ='" + txtBillNo.Text + "' ", con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;

            //fill product names in comobox
            cmbProducts.Items.Clear();
            con.Open();
            cmd = new SqlCommand("Select ProName from TblProducts order by ProName asc ", con);
            cmd.ExecuteNonQuery();
           dt = new DataTable();
           da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                cmbProducts.Items.Add(dr["ProName"]);
            }
            con.Close();

        }

        private void txtQty_Leave(object sender, EventArgs e)
            // when txtbox qty looses focus , then method will be called
        {
            Total_amount();
        }
        public void Total_amount()
            //method to calculate amount after entering price and quantity of product
        {
            double a1, b1, tt;
            a1 = double.Parse(txtPrice.Text);
            b1 = double.Parse(txtQty.Text);
            tt = a1 * b1;
            if (tt > 0)

            {
                txtAmount.Text = tt.ToString();
            }
        }
        void LoadSINo()
            // method to add SINo ( when row add Serial number will be incremented across each row 
        {
            int i = 1;
                foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = i;
                i++;
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
           
        {
            LoadSINo();
        }
    
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            // to add values in the cells at every cell
        {
            btnAdd.Text = "update";
            b = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[b];
            txtDeleteUpdate.Text = row.Cells[0].Value.ToString();
            cmbProducts.Text = row.Cells[1].Value.ToString();
            txtPrice.Text = row.Cells[2].Value.ToString();
            txtQty.Text = row.Cells[3].Value.ToString();
            txtAmount.Text = row.Cells[4].Value.ToString();
 
         
        }
        

        private void btnDelete_Click(object sender, EventArgs e)
            // to remove the selected row in gridview 
        {
            foreach  (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow) dataGridView1.Rows.Remove(row);
               
                LoadSINo();
            }
            MessageBox.Show("Bill Deleted");
            CLR();
            G_total();
            btnAdd.Text = "Add";
        }
        //method to calculate netpay after discount
        public void Disc()
        {
            double a2, b2, i;
            double.TryParse(txtBillTotal.Text, out a2);
            double.TryParse(txtDisAmount.Text, out b2);
            i = a2 - b2;
            if (i > 0)
            {
                txtNetPay.Text = i.ToString();
            }
        }
        void cal_add()
        {
            double sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sum +=Convert.ToDouble( dataGridView1.Rows[i].Cells[4].Value);
            }
            txtNetPay.Text = sum.ToString();
            txtBillTotal.Text = sum.ToString();
            
        }
        private void btnAdd_Click(object sender, EventArgs e)
            // if txtbox empty, add data else update that data
        {
            if (txtDeleteUpdate.Text == "")
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.Rows[1].Cells[1].Value = (i + 1).ToString();
                }
                DataTable dt = dataGridView1.DataSource as DataTable;
                DataRow r1 = dt.NewRow();
                r1[1] = cmbProducts.Text.ToString();
                r1[2] = txtPrice.Text.ToString();
                r1[3] = txtQty.Text.ToString();
                r1[4] = txtAmount.Text.ToString();
                r1[5] = txtBillNo.Text.ToString();
                cmbProducts.Focus();
                dt.Rows.Add(r1);
                cal_add();
            }
            
            else
            { // adding updated (new) values
                DataGridViewRow row = dataGridView1.Rows[b];
                row.Cells[1].Value = cmbProducts.Text;
                row.Cells[2].Value = txtPrice.Text;
                row.Cells[3].Value = txtQty.Text;
                row.Cells[4].Value = txtAmount.Text;
                row.Cells[5].Value = txtBillNo.Text;
                CLR();
                G_total();
                btnAdd.Text = "Add";
            }

            
            
        } //method to sum total netpay
        public void G_total()
        {
            double sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
            }
            txtBillTotal.Text = sum.ToString();
        }
        //method to clear txtboxes
        public void CLR()
        {
            txtPrice.Text = "";
            txtQty.Text = "";
            cmbProducts.Text = "";
            txtAmount.Text = "";
            txtDeleteUpdate.Text = "";
            txtBillTotal.Text = "";
            txtNetPay.Text = "";
            txtDisAmount.Text = "";
        }

        private void txtBillTotal_TextChanged(object sender, EventArgs e)
        {
            Disc();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //deleting old bills(row data)
            try
            {
                con.Open();
                cmd = new SqlCommand("Delete From TblRowData where BillNo= '" + txtBillNo.Text + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            // saving updated data
            try
            {
               
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {


                    SqlCommand cmd1 = new SqlCommand("Insert Into TblRowData (SINo,ProductName,Price,Qty,Amount,BillNo,Date) values ('"+dataGridView1.Rows[i].Cells[0].Value.ToString()+"','"+ dataGridView1.Rows[i].Cells[1].Value.ToString() + "','"+ dataGridView1.Rows[i].Cells[2].Value.ToString() + "','"+ dataGridView1.Rows[i].Cells[3].Value.ToString() + "','"+ dataGridView1.Rows[i].Cells[4].Value.ToString() + "','"+ dataGridView1.Rows[i].Cells[5].Value.ToString() + "','"+ dateTimePicker2.Value.ToString("MM/dd/yyyy") + "') ", con);
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();

                }
               
            }
            catch (Exception ex)
               
            {

                MessageBox.Show(ex.Message);
            }
            //update bill amount disc amount and net pay
            try
            {
                con.Open();
                cmd = new SqlCommand("Update TblHeadData set BillDate='"+dateTimePicker2.Value.ToString("MM/dd/yyyy")+"',BillAmount='"+txtBillTotal.Text+ "',DisAmount='"+txtDisAmount.Text+ "',NetPay='"+txtNetPay.Text+"'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Bill Updated!");
                CLR();
              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            this.Hide();
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        { //load product price
            con.Open();
            cmd = new SqlCommand("select * from TblProducts where ProName = '" + cmbProducts.Text + "'", con);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtPrice.Text = dr[2].ToString();
            }
            con.Close();
        }

        private void txtDisAmount_Leave(object sender, EventArgs e)
        {
            Disc();
        }
    }
}
