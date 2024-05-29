using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Billing_System
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Products P = new Products();
            P.Show();
        }

        private void newBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            NewBill nb = new NewBill();
            nb.Show();
           
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void viewBillsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View v = new View();
            v.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            login lg = new login();
            this.Hide();
            lg.Show();
            //lg.label1.Hide();
            //lg.label4.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
