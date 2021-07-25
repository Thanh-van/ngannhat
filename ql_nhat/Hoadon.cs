using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ql_nhat.DAO;
using ql_nhat.DTO;
namespace ql_nhat
{
    public partial class Hoadon : Form
    {
        public Hoadon()
        {
            InitializeComponent();
        }

        private void printPreviewControl1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void create(DateTimePicker date, String fomat)
        {
            date.Format = DateTimePickerFormat.Custom;
            date.CustomFormat = fomat;
            //"dd-MM-yyyy";
            date.MaxDate = DateTime.Now;
        }

        private void Hoadon_Load(object sender, EventArgs e)
        {
            create(txt_ngay, "dd-MM-yyyy");
            create(txt_thang, "MM-yyyy");
            dataGridView1.DataSource = DataProvider.Instance.ExecuteQuery("select id as 'ID',id_khach as 'Khách',id_nv as 'Nhân Viên',ngay as 'Ngày', tongtien as 'Tổng Tiền',tienno as 'Tiền Nợ' from tb_hoadonban");
            BanHang_tt_no();
        }
        private void BanHang_tt_no()
        {
            bh_tt.Text= "$"+Xuly.Instance.sub_colum("select sum(tongtien) from tb_hoadonban");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            switch (key)
            {
                case 1:
                    {

                        break;
                    }
                case 2:
                    {
                        break;
                    }
                case 3:
                    {
                        break;
                    }
            }
        }
        int key;
        // 1 Nhap Hang
        // 2 Ban Hang
        // 3 Khach No
        #region SetKey
        private void nhap_hang(object sender, EventArgs e)
        {
            key = 1;
        }

        private void Mua_hang(object sender, EventArgs e)
        {
            key = 2;
        }

        private void khach_no(object sender, EventArgs e)
        {
            key = 3;
        }
        #endregion

        private void txt_thang_ValueChanged(object sender, EventArgs e)
        {
            String thang = txt_thang.Text.Remove(2, 5);
            String nam = txt_thang.Text.Remove(0, 3);
            dataGridView1.DataSource = DataProvider.Instance.ExecuteQuery("select id as 'ID',id_khach as 'Khách',id_nv as 'Nhân Viên',ngay as 'Ngày', tongtien as 'Tổng Tiền',tienno as 'Tiền Nợ' from tb_hoadonban where year(ngay) = '"+nam+"' and month(ngay)= '"+thang+"' ");
        }

        private void txt_ngay_ValueChanged(object sender, EventArgs e)
        {
            String nam = txt_ngay.Text.Remove(0,6);
            String thang = txt_ngay.Text.Remove(0,3);
            thang = thang.Remove(2,5);
            String day = txt_ngay.Text.Remove(2, 8);
            //sms.Show.Info(thang);
            dataGridView1.DataSource = DataProvider.Instance.ExecuteQuery("select id as 'ID',id_khach as 'Khách',id_nv as 'Nhân Viên',ngay as 'Ngày', tongtien as 'Tổng Tiền',tienno as 'Tiền Nợ' from tb_hoadonban where year(ngay) = '" + nam + "' and month(ngay)= '" + thang + "' and DAY(ngay)= '" + day + "' ");
        }

        private void Nhannv_Click(object sender, EventArgs e)
        {

        }
    }
}
