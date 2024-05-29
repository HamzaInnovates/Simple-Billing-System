using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Simple_Billing_System
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void login_Load(object sender, EventArgs e)
        {
            //label4.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string pass = textBox2.Text;
            if (username == "Admin" && pass == "@Hamza123")
            {
                this.Hide();
                Main s = new Main();
                s.Show();
            }
            else
            {
                MessageBox.Show("Invalid Username/Password", "Login Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk);
            }
        }
    }
}
