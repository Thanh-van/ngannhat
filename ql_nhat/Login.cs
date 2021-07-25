using ql_nhat.DAO;
using ql_nhat.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ql_nhat
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public static String manv = null;
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txt_pass.UseSystemPasswordChar = true;
            }
            else
                txt_pass.UseSystemPasswordChar = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String s = "exec login_app @tk , @mk";
            DataTable dt = DataProvider.Instance.ExecuteQuery(s, new object[] { txt_name.Text.Trim(), txt_pass.Text.Trim() });
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (txt_name.Text == dt.Rows[i].ItemArray[1].ToString())
                    {
                        if (txt_pass.Text == dt.Rows[i].ItemArray[2].ToString())
                        {
                            Form1 f = new Form1();
                            f.Manv = dt.Rows[i].ItemArray[3].ToString();
                            manv= dt.Rows[i].ItemArray[3].ToString();
                            this.Hide();
                            f.ShowDialog();

                        }
                        else
                        {
                            sms.Show.Error("Sai Mật Khẩu");
                        }
                    }
                    else
                    {
                        sms.Show.Error("Sai Tài Khoản");
                    }
                }
            }
            else
            {
                sms.Show.Error("Đăng nhập thất bại");
            }
        }
        private void check_load()
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txt_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_pass_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
