using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ql_nhat.DTO;
using ql_nhat.DAO;
using System.Text.RegularExpressions;

namespace ql_nhat
{
    public partial class Ql : Form
    {
        public Ql()
        {
            InitializeComponent();
        }
        private String ql = null;
        public Ql(String Key)
        {
            InitializeComponent();
            this.ql = Key;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }


        #region LoadThemKhach
        private void load_form_Khach()
        {
            full(DataKhach);
            DataKhach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            String sql = "select ma_k, ten_k as 'Tên Khách',sdt as 'SĐT',diachi as 'Địa chỉ' from khach where id !=2";
            DataKhach.DataSource = DataProvider.Instance.ExecuteQuery(sql);
            this.DataKhach.Columns[0].Visible = false;
            arr = new string[DataKhach.RowCount];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = DataKhach.Rows[i].Cells[0].Value.ToString();

            }

        }
        #endregion
        String clear(TextBox txt)
        {
            String sent = txt.Text;
            sent = sent.Trim();
            Regex trimmer = new Regex(@"\s\s+");
            sent = trimmer.Replace(sent, " ");
            return sent;
        }
        Boolean check(TextBox txt)
        {


            if (!String.IsNullOrEmpty(clear(txt)) && clear(txt) != "")
            {
                return true;
            }
            else return false;
        }
        private Boolean check_add_edit_khach()
        {
            if (check(txt_tenkhach))
            {
                if (check(txt_sdt_khach))
                {
                    if (check(txt_diachi_khach))
                    {
                        return true;

                    }
                    else
                    {
                        txt_diachi_khach.Focus();
                        sms.Show.Error("Vui lòng thêm địa chỉ");
                    }
                }
                else
                {
                    txt_sdt_khach.Focus();
                    sms.Show.Error("Vui lòng nhập sdt");
                }
            }
            else
            {
                txt_tenkhach.Focus();
                sms.Show.Error("Vui lòng nhập tên");
            }
            return false;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (K_add.Text == "Refresh")
            {
                K_add.Text = "Thêm";
                load_Ma();
                txt_tenkhach.Text = "";
                txt_sdt_khach.Text = "";
                txt_diachi_khach.Text = "";
            }
            else
            {
                if (check_add_edit_khach())
                {
                    String key = "insert into khach(ma_k, ten_k, diachi, sdt) values('" + clear(txt_makhach) + "', N'" + clear(txt_tenkhach) + "', N'" + clear(txt_diachi_khach) + "', '" + clear(txt_sdt_khach) + "')";
                    int dem = DataProvider.Instance.ExecuteNonQuery(key);
                    if (Dialog_kq(dem, "Thêm thành công", "Thêm thất bại"))
                    {
                        load_form_Khach();
                        load_Ma();
                    }
                }
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private String[] arr = null;
        void full(DataGridView data)
        {
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            data.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            data.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void Ql_Load(object sender, EventArgs e)
        {
            
            full(datanv);
            if (ql != null)
            {
                NhanVien.Visible = false;
                load_form_Khach();
                K_delete.Enabled = false;
                K_edit.Enabled = false;

                load_Ma();
            }
            else
            {
                khach.Visible = false;
                NhanVien.Visible = true;
                load_tbnhanvien();
            }
        }
        #region load_Ma
        private void load_Ma()
        {
            if (int.Parse(create_Masp()) < 10)
            {
                txt_makhach.Text = "K0" + (int.Parse(create_Masp()) + 1).ToString();

            }
            else
            {
                txt_makhach.Text = "K" + (int.Parse(create_Masp()) + 1).ToString();
            }
        }
        #endregion 

        private String create_Masp()
        {
            String tam = null;
            if (arr.Length > 0)
            {
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    tam = arr[i].Remove(0, 2);
                }
            }
            return tam;
        }
        public void set_gt(String gt)
        {
            if (gt=="1")
            {
                radioButton1.Checked=true;
            }else
            {
                radioButton2.Checked = true;
            }
        }
        private void load_Ma_nv()
        {
            if (int.Parse(create_Masp()) < 10)
            {
                txt_manv.Text = "NV0" + (int.Parse(create_Masp()) + 1).ToString();

            }
            else
            {
                txt_manv.Text = "NV" + (int.Parse(create_Masp()) + 1).ToString();
            }
        }
        private void DataKhach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            K_add.Text = "Refresh";
            txt_makhach.Text = DataKhach.CurrentRow.Cells[0].Value.ToString();
            txt_tenkhach.Text = DataKhach.CurrentRow.Cells[1].Value.ToString();
            txt_sdt_khach.Text = DataKhach.CurrentRow.Cells[2].Value.ToString();
            txt_diachi_khach.Text = DataKhach.CurrentRow.Cells[3].Value.ToString();
        }

        private void xoa_khach(object sender, EventArgs e)
        {
            DialogResult dl = sms.Show.Dialog("Bạn có muốn xóa khách hàng" + txt_tenkhach);
            if (dl == DialogResult.Yes)
            {
                String sql = " delete from khach where ma_k = '" + clear(txt_makhach) + "'";
                int dem = DataProvider.Instance.ExecuteNonQuery(sql);
                Dialog_kq(dem, "Xóa Thành Công", "Xóa thất bại");

            }

        }
        private Boolean Dialog_kq(int check, String txt, String txt2)
        {
            if (check > 0)
            {
                sms.Show.Info(txt);
                return true;
            }
            else
            {
                sms.Show.Error(txt2);
                return false;
            }
        }

        private void sua_khach(object sender, EventArgs e)
        {
            if (check_add_edit_khach())
            {
                String key = "update khach set ten_k= N'" + clear(txt_tenkhach) + "',sdt='" + clear(txt_sdt_khach) + "',diachi= N'" + clear(txt_diachi_khach) + "' where ma_k = '" + clear(txt_makhach) + "'";
                int dem = DataProvider.Instance.ExecuteNonQuery(key);
                if (Dialog_kq(dem, "Sửa thành công", "Sửa thất bại"))
                {
                    load_form_Khach();
                    load_Ma();
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        public Boolean check_add_edit_nv()
        {
            if (check(txt_tennv))
            {
                if (check(txt_sdtnv))
                {
                    if (check(txt_tk))
                    {
                        if (check(txt_mk))
                        {
                            if (check(txt_diachi))
                            {
                                return true;
                            }
                            else
                            {
                                txt_diachi.Focus();
                                sms.Show.Error("Vui lòng nhập địa chỉ");
                            }
                        }
                        else
                        {
                            txt_mk.Focus();
                            sms.Show.Error("Vui lòng nhập mật khẩu");
                        }
                    }
                    else
                    {
                        txt_tk.Focus();
                        sms.Show.Error("Vui lòng nhập tài khoản");
                    }
                }
                else
                {
                    txt_sdtnv.Focus();
                    sms.Show.Error("Vui lòng nhập SĐT");
                }
            }else
            {
                txt_tennv.Focus();
                sms.Show.Error("Vui lòng nhập tên nhân viên");
            }
            return false;
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            if (check_add_edit_khach())
            {

            }
        }
        private void load_tbnhanvien()
        {
            nv_date.Format = DateTimePickerFormat.Custom;
            nv_date.CustomFormat = "dd-MM-yyyy";
            nv_date.MaxDate = DateTime.Now;
            String sql = "select ma_nv,ten_nv as'Tên',gt as 'Giới Tính',ns as 'Ngày Sinh',sdt as 'SĐT', diachi as 'Địa Chỉ',tk as 'Tài Khoản',mk as 'Mật Khẩu',nv_ql from tb_nv,tb_login where tb_login.login_nv = tb_nv.ma_nv";
            datanv.DataSource = DataProvider.Instance.ExecuteQuery(sql);
            datanv.Columns[0].Visible = false;
            arr = new string[datanv.RowCount];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = datanv.Rows[i].Cells[0].Value.ToString();

            }
            load_Ma_nv();

        }
        Boolean quyen(String s)
        {
            try
            {
                int.Parse(s);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        void set_quyen(String s)
        {

            try
            {
                if (quyen(s))
                {
                    txt_quyen.SelectedIndex = int.Parse(s);
                }
 
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void datanv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           nv_add.Text = "Refresh";
            txt_manv.Text = datanv.CurrentRow.Cells[0].Value.ToString();
            nv_date.Text= datanv.CurrentRow.Cells[3].Value.ToString();
            txt_tennv.Text = datanv.CurrentRow.Cells[1].Value.ToString();
            txt_diachi.Text = datanv.CurrentRow.Cells[5].Value.ToString();
            txt_sdtnv.Text = datanv.CurrentRow.Cells[4].Value.ToString();
            txt_tk.Text = datanv.CurrentRow.Cells[6].Value.ToString();
            txt_mk.Text = datanv.CurrentRow.Cells[7].Value.ToString();
            set_gt(datanv.CurrentRow.Cells[2].Value.ToString());
            set_quyen(datanv.CurrentRow.Cells[8].Value.ToString());
        }
        int get_gt()
        {
            if (radioButton1.Checked == true)
            {
                return 1;
            }
            else return 0;
        }
        private void add_nv(object sender, EventArgs e)
        {
            if (nv_add.Text == "Refresh")
            {
                nv_add.Text = "Thêm";
                load_Ma_nv();
                txt_tennv.Text = "";
                txt_diachi.Text = "";
                txt_sdtnv.Text = "";
                txt_tk.Text = "";
                txt_mk.Text = "";
                set_gt("1");
                set_quyen("0");
            }
            else
            {
                if (check_add_edit_nv())
                {
                    String sql = "insert into tb_nv(ma_nv, ten_nv, gt, ns, sdt, diachi) values('"+clear(txt_manv)+"', N'"+ clear(txt_tennv) + "',  '"+ get_gt()+ "', '"+ nv_date.Text + "','"+clear(txt_sdtnv)+"', N'"+ clear(txt_diachi) + "')";
                    String nv_ql = "insert into tb_login(tk,mk,nv_ql,login_nv) values('"+clear(txt_tk)+"','"+ clear(txt_mk) + "','"+ getquyen() + "','"+ clear(txt_manv) + "')";
                    int check = DataProvider.Instance.ExecuteNonQuery(sql);
                    if (Dialog_kq(check,"Thêm Thành Công","Thêm thất bại"))
                    {
                        load_tbnhanvien();
                        check = DataProvider.Instance.ExecuteNonQuery(nv_ql);
                        if (Dialog_kq(check,"Thêm thành công","Thêm thất bại"))
                        {
                            load_tbnhanvien();
                        }
                    }
                }

            }
        }
        int getquyen()
        {
            if (txt_quyen.Text=="Nhân Viên")
            {
                return 0;
            }
            else {
                if (txt_quyen.Text == "Quản Lý")
                {
                    return 1;
                }
                else return 2;
            }
        }
        private void delete_nv(object sender, EventArgs e)
        {

        }

        private void edit_nv(object sender, EventArgs e)
        {
            DialogResult dl = sms.Show.Dialog("Bạn co muốn xóa nhân viên"+txt_manv);
            if (dl==DialogResult.Yes)
            {
                String query = "exec delete_nv @ma";
                int check = DataProvider.Instance.ExecuteNonQuery(query, new Object[] { clear(txt_manv)});
                if (Dialog_kq(check,"Xóa Thành Công","Xóa Thất Bại"))
                {
                    load_tbnhanvien();
                }
            }
        }
    }
}
