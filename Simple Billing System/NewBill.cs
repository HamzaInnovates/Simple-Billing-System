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
    public partial class NewBill : Form
    {
        //global variable a
        int row = 0;
        int c = 0;
        int i;
        int billno = 0;
        int val=0;
        string value;

        SqlConnection con = new SqlConnection(Properties.Settings.Default.SimpleBillingCon);
        SqlCommand cmd;
        SqlDataReader dr;
        public NewBill()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void NewBill_Load(object sender, EventArgs e)
        { // loads Product Name 
           
            cmbProducts.Items.Clear();
            con.Open();
            cmd = new SqlCommand("Select ProName from TblProducts order by ProName asc ", con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt); 
            // loop that checks all of the rows and loads product name in combobox
            foreach (DataRow dr in dt.Rows)
            {
                cmbProducts.Items.Add(dr["ProName"]);
            }
            con.Close();
            LoadBill();
        
            dataGridView1.Columns[5].Visible = false;
        }
        public void LoadBill()
        { 
            // method that loads BillNo and increment the Bill No evry time  when form opens (because i have created a table and not set the BillNo as identity sepecification so that i will be auto incremented)

            //Read is a function that reads data checks that is there any row? if row is there then it will return true else false 
            SqlConnection con = new SqlConnection(Properties.Settings.Default.SimpleBillingCon);
            con.Open();

            SqlCommand cmd = new SqlCommand("Select MAX(BillNo) from TblHeadData", con);
            dr = cmd.ExecuteReader();


            if (dr.Read())
            {
                value = (dr[0].ToString());
                if (value != "")
                {
                    val = Convert.ToInt32(dr[0]);
                }

                if (billno <= val)
                {
                    billno = ++val;
                    txtBillNo.Text = billno.ToString();
                }
                con.Close();
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbProducts.Text=="")
            {
                MessageBox.Show("Product Name is empty");

            }
            else if (txtPrice.Text=="")
            {
                MessageBox.Show("Product Price is empty");
            }
            
            else if (txtQty.Text=="")
            {
                MessageBox.Show("Product Quantity is empty");
            }
            else
            { //if txtbox is empty then add else update
                if (txtDeleteUpdate.Text == "")
                {
                    
                    row = dataGridView1.Rows.Count ;
                    dataGridView1.Rows.Add();
                    dataGridView1["ProductName", row].Value = cmbProducts.Text;
                    dataGridView1["Price", row].Value = txtPrice.Text;
                    dataGridView1["Qty", row].Value = txtQty.Text;
                    dataGridView1["Amount", row].Value = txtAmount.Text;
                    dataGridView1["BillNo", row].Value = txtBillNo.Text;
                    dataGridView1["Date", row].Value = dateTimePicker1.Value.ToString("MM/dd/yyyy");

                    dataGridView1.Refresh();
                    cmbProducts.Focus();
                    if (dataGridView1.Rows.Count>0)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1];
                    }
                    
                }
                else
                { //Update 
                    btnAdd.Text = "update";

                    i = Convert.ToInt32(txtDeleteUpdate.Text);
                    
                    DataGridViewRow row = dataGridView1.Rows[c];
                    row.Cells[1].Value = cmbProducts.Text;
                    row.Cells[2].Value = txtPrice.Text;
                    row.Cells[3].Value = txtQty.Text;
                    row.Cells[4].Value = txtAmount.Text;
                    btnAdd.Text = "Add";

                }
                    clear();
                    G_total();
                

            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)

        { //  it is used here to add value starting for the cell 0 and increments that value at each row
            // basically it is used here to add value to the serial number as serial number is always from the starting (1) and upto so on 
            //but if we had set this as identity spcification, it will add different values to Table
               this.dataGridView1.Rows[e.RowIndex].Cells[0].Value =(e.RowIndex+1).ToString();
        }

        // double click is an event of GridView when any of its cell is ouble clicked

               private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
      
            //e is an event argument and rowindex identifies that which row was clicked nad then adds that values to txtboxes for del,update
            i = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[i];
            cmbProducts.Text = row.Cells[1].Value.ToString();
            txtPrice.Text = row.Cells[2].Value.ToString();
            txtQty.Text = row.Cells[3].Value.ToString();
            txtAmount.Text = row.Cells[4].Value.ToString();
            txtDeleteUpdate.Text = row.Cells[0].Value.ToString();
         

            btnAdd.Text = "Update";


        }
        // method to claculate total amount
        public void Total_amount()
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
        //method to clear txtboxes and combobox
        void clear()
        {
            cmbProducts.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
            txtAmount.Text = "";
            txtDeleteUpdate.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtDeleteUpdate.Text=="")
            {
                MessageBox.Show("Please Select an Product To Delete!");
            }
       
            else {
                // checks each row in datagrid view rows and remove the selected row
                //DATAGRIDVIEW row is an class that is used to identify the rows present in datagrid
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (!row.IsNewRow)dataGridView1.Rows.Remove(row);
                }
          
                clear();
                btnAdd.Text = "Add";
            }
           
        }

        private void txtQty_Leave(object sender, EventArgs e)
        {
            Total_amount();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void txtPrice_Leave(object sender, EventArgs e)
        {
            
        }
        public void G_total()
            //  method that adds all of the rows and cell value 4 which is net pay and shows total sale amount 
        {
            double sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sum+=Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
            }
            txtBillTotal.Text = sum.ToString();
        }
        // method to calculate amount after discount
        public void disc()
        { 
            double a2, b2, i;
            double.TryParse(txtBillTotal.Text, out a2);
            double.TryParse(txtDisAmount.Text, out b2);
            i = a2 - b2;
            if(i>0)
            {
                txtNetPay.Text = i.ToString();
            }
        }

        private void txtBillTotal_TextChanged(object sender, EventArgs e)

        { // when total amount text will change , then the same amount value will be shifted to netpay txtbox (without discount )
           
            txtNetPay.Text = txtBillTotal.Text;
            
        }

        private void txtDisAmount_Leave(object sender, EventArgs e)
        {
            disc();
        }
         
        private void btnSave_Click(object sender, EventArgs e)
        { // if no rows are there nothing will be saved
           
            if (dataGridView1.Rows.Count<1)
            {
                MessageBox.Show("Please Add Atleast one Bill");
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {


                    SqlCommand cmd = new SqlCommand("Insert Into TblRowData (SINo,ProductName,Price,Qty,Amount,BillNo,Date) values ('" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "') ", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                // to save row data 
                con.Open();
                SqlCommand cmd1 = new SqlCommand("Insert into TblHeadData(BillNo,BillDate,BillAmount,DisAmount,NetPay)values('"+txtBillNo.Text+"','"+dateTimePicker1.Text+"','"+txtBillTotal.Text+"','"+txtDisAmount.Text+"','"+txtNetPay.Text+"' )",con);
                cmd1.ExecuteNonQuery(); 
               
                con.Close();
                //after saving load bill number new one
                // and clear all txtboxes
                LoadBill();
                clear();
              
                txtDisAmount.Text = "";
                txtBillTotal.Text = "";
                txtNetPay.Text = "";
                MessageBox.Show("Bill Saved!");
      
                
                
                   
                    txtBillNo.Text = billno.ToString();
                billno++;
                
                // the clear the rows in datagrid to differ between two bills 
                dataGridView1.Rows.Clear();
               



            }
                  
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // to load corresponding  product price when an item is selected in cmbo box
            con.Open();
            cmd = new SqlCommand("select * from TblProducts where ProName = '" + cmbProducts.Text + "'", con);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                // adds that value in price txtbox
                txtPrice.Text = dr[2].ToString();
            }
            con.Close();
        }
    }
}
