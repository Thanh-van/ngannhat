using ql_nhat.DAO;
using ql_nhat.DTO;
using System;
using System.Windows.Forms;
namespace ql_nhat
{
    public partial class Khach : Form
    {
        public Khach()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String sql = null;
            if (textBox1.Text.Trim() == null)
            {
                 sql = "select ma_k,ten_K as 'Họ Tên',sdt as 'SĐT',diachi as 'Địa Chỉ' from khach where ma_K != 'K02'";
                
                 
            }
            else
            {
                sql = "select ma_k,ten_K as 'Họ Tên',sdt as 'SĐT',diachi as 'Địa Chỉ' from khach where ma_K != 'K02' and ten_k like N'%"+textBox1.Text.Trim()+"%'";
            }
            set(sql);
            Xuly.Instance.setLabel(data_khach,label1,"van");
        }
        
        private void set(String sql)
        {
            try
            {
                data_khach.DataSource = DataProvider.Instance.ExecuteQuery(sql);
            }
            catch(Exception e)
            {
                sms.Show.Error("Lỗi load dữ liệu đại lý \n ");
            }
            
        }
        private void Khach_Load(object sender, EventArgs e)
        {
            String sql = "select ma_k,ten_K as 'Họ Tên',sdt as 'SĐT',diachi as 'Địa Chỉ' from khach where ma_K != 'K02'";
            set(sql);
            
            data_khach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            data_khach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            data_khach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            data_khach.Columns[0].Visible = false;
            Xuly.Instance.setLabel(data_khach, label1,"van");
        }

        public delegate void GETDATA(String data, String data1);
        public GETDATA mydata;

        private void bam(object sender, DataGridViewCellEventArgs e)
        {
            if (String.IsNullOrEmpty(data_khach.CurrentRow.Cells[0].Value as String)==false)
            {
                mydata(data_khach.CurrentRow.Cells[1].Value.ToString(),data_khach.CurrentRow.Cells[0].Value.ToString());
                this.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ql n = new Ql("OK");
            n.ShowDialog();
        }
    } 
}
