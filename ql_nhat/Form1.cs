using ql_nhat.DAO;
using ql_nhat.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ql_nhat
{
    public partial class Form1 : Form
    {


        #region khoitao
        public Form1()
        {
            InitializeComponent();
            Thread th = new Thread(Load_NhanVien);
            th.Start();
        }
        #endregion
        #region getdatafromkhach
        private void Getvalue(string Message, string Message1)
        {
            txt_tenkhach.Text = Message;
            ma_khach.Text = Message1;
        }
        #endregion
        #region khaibaobien
        private Boolean check = false;
        private String Ma_sp = null;
        private String Ma_dvt = null;
        private String Ten_loai = null;
        private int Sl;
        private int chuyendoi = 1;
        private Thread thread = null;
        private String Ma_loai = null;
        public String Manv = null;
        public String Tennv = null;
        public String level = null;
        #endregion
        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            
            Xuly.Instance.btn_enabled(btn_sua, btn_x, 1);
            load_listLoai();
            check = true;
            load_product(Ma_loai);
            load_dvt();
            dongia_chage();
            id_product.Visible = false;
            Xuly.Instance.create_list(listView1);
            txt_nv.Text = Manv+" - "+Tennv;

        }
        #endregion
        private void Load_NhanVien()
        {
            String sql = "select * from tb_nv,tb_login where tb_nv.ma_nv=tb_login.login_nv and tb_nv.ma_nv='" + Manv + "' ";
            DataTable dt = DataProvider.Instance.ExecuteQuery(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tennv = dt.Rows[i].ItemArray[2].ToString();
                    level =  dt.Rows[i].ItemArray[11].ToString();
                }
            }
        }
        #region Combobox loại
        void load_listLoai()
        {
            String sql = "select * from loai where ma_loai in (select ma_loai from sanpham)";
            Combobox_data.Instance.Combobox(list_loai, sql, "ten", "ma_loai");
            Ma_loai = list_loai.SelectedValue.ToString();
            Ten_loai = list_loai.SelectedText.ToString();
        }
        #endregion

        // Kiểm tra sp tồn tại trong hóa đơn 
        #region check_add_id
        private Boolean check_add_id()
        {
            int sl = int.Parse(txt_sl.Value.ToString());
            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (Ma_sp == item.SubItems[0].Text)
                    {

                        sl = sl + int.Parse(item.SubItems[4].Text);
                        if (Xuly.Instance.check_sl(item, Ma_sp, txt_sl, chuyendoi, Sl) == true)
                        {
                            item.SubItems[4].Text = Convert.ToString(int.Parse(item.SubItems[4].Text) + txt_sl.Value);
                            item.SubItems[7].Text = Convert.ToString(int.Parse(item.SubItems[4].Text) * int.Parse(item.SubItems[6].Text));
                            Xuly.Instance.change_meney(listView1, txt_tien);
                        }
                        else
                        {
                            sms.Show.Waring("Không đủ số lượng");
                        }
                        return false;
                    }

                }
            }
            else
            {
                if (Xuly.Instance.xuly_check_sl(sl, chuyendoi, 0, Sl))
                {
                    return true;
                }
                else
                {
                    sms.Show.Waring("Không đủ số lượng");
                    return false;
                }
            }
            return true;
        }
        #endregion
        // Kiểm Tra số lượng hàng trong kho khi thêm sửa
        // thêm item
        #region add_item
        private void add_item()
        {
            if (check_add_id() != false)
            {
                if (txt_nameproduct.Items.Count > 0)
                {
                    string[] arr = new string[11];
                    ListViewItem itm;
                    //Thêm Item đầu tiên
                    arr[0] = Ma_sp;
                    arr[1] = (listView1.Items.Count + 1).ToString();
                    arr[2] = list_loai.Text;
                    arr[3] = txt_nameproduct.Text;
                    arr[4] = txt_sl.Value.ToString();
                    arr[5] = txt_dvt.Text;
                    arr[6] = txt_dongia.Text;
                    arr[7] = Convert.ToString(txt_sl.Value * int.Parse(txt_dongia.Text.ToString()));
                    arr[8] = Ma_dvt;
                    arr[9] = Ma_loai;
                    arr[10] = chuyendoi.ToString();
                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);
                    Xuly.Instance.change_meney(listView1, txt_tien);
                }
                else
                {
                    sms.Show.Error("Không Có Sản Phẩm", "Lỗi Thêm !");
                }
            }
        }
        #endregion

        #region open_form_quanly
        private void quanly(object sender, EventArgs e)
        {
            Loai loai = new Loai();
            loai.Show();

        }
        #endregion
        #region click_listview
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Xuly.Instance.btn_enabled(btn_sua, btn_x, 2);

            Xuly.Instance.select(sender, list_loai, txt_nameproduct, txt_dvt, Ma_sp, txt_dongia, txt_sl);

        }
        #endregion
        #region themsp
        private void button4_Click(object sender, EventArgs e)
        {
            add_item();
        }
        #endregion
        // Hàm xóa item
        #region btn_xoa
        private void btn_xoa(object sender, EventArgs e)
        {
            try
            {
                if (Xuly.Instance.Xoa_SP(listView1, txt_nameproduct, txt_dvt))
                {
                    Xuly.Instance.change_meney(listView1, txt_tien);
                    Xuly.Instance.btn_enabled(btn_sua, btn_x, 1);
                }

            }
            catch { }

        }
        #endregion

        // Load combobox cho sản phẩm
        #region load_sanpham
        private void load_product(String key)
        {
            String sql = "select * from sanpham  where ma_loai = N'" + key + "' ";
            Combobox_data.Instance.Combobox(txt_nameproduct, sql, "ten", "ma_sp");
        }
        #endregion
        #region list_loai_SelectedIndexChanged
        private void list_loai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (check == true)
            {
                ComboBox cb = (ComboBox)sender;
                if (cb.Items.Count > 0)
                {
                    try
                    {
                        Ma_loai = list_loai.SelectedValue.ToString();
                        Ten_loai = list_loai.SelectedText.ToString();
                        String sql = "select * from sanpham where ma_loai = N'" + Ma_loai + "'";
                        txt_nameproduct.DataSource = null;
                        Combobox_data.Instance.Combobox(txt_nameproduct, sql, "ten", "ma_sp");

                    }
                    catch (Exception)
                    {
                        sms.Show.Error(e.ToString());
                    }

                }
                else
                {
                    sms.Show.Waring("Không có dữ liệu");
                }

            }

        }
        #endregion
        // changed khi select product
        #region combobox sanpham changed
        private void txt_nameproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cb = (ComboBox)sender;
                if (cb.Items.Count > 0)
                {
                    Ma_sp = txt_nameproduct.SelectedValue.ToString();
                    load_dvt();
                    dongia_chage();
                }

            }
            catch (Exception)
            {
                // MessageBox.Show(a.ToString(), "Lỗi Hệ Thống !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion
        private void load_dvt()
        {
            String sql = " select * from dvt where ma_sp='" + Ma_sp + "' ";
            Combobox_data.Instance.Combobox(txt_dvt, sql, "ten", "ma_dvt");
        }
        #region dongia_chage
        private void dongia_chage()
        {

            String sql = "select * from dvt,sanpham where dvt.ma_sp='" + Ma_sp + "' and sanpham.ma_sp=dvt.ma_sp and dvt.ma_dvt = '" + Ma_dvt + "'";
            String[] arr = DataProvider.Instance.SqldataReader(sql);
            txt_dongia.Text = arr[0];
            if (arr[2] != null)
            {
                Sl = int.Parse(arr[1]);
                chuyendoi = int.Parse(arr[2]);
            }
        }
        #endregion
        private void txt_dvtChanged(object sender, EventArgs e)
        {
            try
            {
                Ma_dvt = txt_dvt.SelectedValue.ToString();
                dongia_chage();
            }
            catch { }

        }
        // Sửa danh sách mua hàng
        #region btn_sua
        private void btn_sua_click(object sender, EventArgs e)
        {
            Boolean check = false;
            int sl = int.Parse(txt_sl.Value.ToString());
            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (Ma_sp == item.SubItems[0].Text && Ma_dvt == item.SubItems[8].Text)
                    {
                        if (Xuly.Instance.check_sl(item, Ma_sp, txt_sl, chuyendoi, Sl, Ma_dvt) == true)
                        {
                            item.SubItems[4].Text = txt_sl.Value.ToString();
                            item.SubItems[7].Text = Convert.ToString(int.Parse(item.SubItems[4].Text) * int.Parse(item.SubItems[6].Text));
                            Xuly.Instance.change_meney(listView1, txt_tien);
                            sms.Show.Info("Sửa thành công");
                        }
                        else
                        {
                            sms.Show.Waring("Không đủ số lượng");
                        }
                        check = true;
                    }

                }
                if (check != true)
                {
                    sms.Show.Waring("Vui lòng thêm mới sản phẩm");
                }
            }
        }
        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string[] tieen = txt_tien.Text.Split('đ');
                txt_tra.Text = Convert.ToString(int.Parse(txt_dua.Text) - int.Parse(tieen[0])) + " đ";
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //listView1.Items.Clear();
            string[] tieen = txt_tien.Text.Split('đ');
            sms.Show.Error(Xuly.Instance.ChuyenSoSangChuoi(Double.Parse(tieen[0])));
        }

        private void btn_daily_Click(object sender, EventArgs e)
        {
            Khach k = new Khach();
            k.mydata = new Khach.GETDATA(Getvalue);
            k.ShowDialog();
        }

        private void btn_le_Click(object sender, EventArgs e)
        {
            set_khach("KL0", "Khách Lẻ", "300 đ", "100 đ");
        }
        public void set_khach(String ma, String ten, String mua, String no)
        {
            ma_khach.Text = ma;
            txt_tenkhach.Text = ten;
            txt_summua.Text = mua;
            txt_no.Text = no;
        }

        private void txt_sl_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }

        private void txt_dua_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (int.Parse(txt_dua.Text) >= 0)
            {

                if (listView1.Items.Count > 0)
                {
                    create_hoadon();
                    DialogResult dl = sms.Show.Dialog("Bạn có muốn in hóa đơn không");
                    if (dl == DialogResult.Yes)
                    {

                        if (thread == null || thread.IsAlive == false)
                        {
                            thread = new Thread(Loadhoadon);
                            thread.Start();
                        }
                        else
                        {
                            sms.Show.Waring("Dg Show Hoa don");
                        }
                    }
                }
                else
                    sms.Show.Error("Chưa có sản phẩm");

            }
            else
            {
                sms.Show.Error("Vui lòng nhập tiền");
            }
        }
        #region Create tb_hoadonban
        private void create_hoadon()
        {
            int id = Xuly.Instance.getid("tb_hoadonban");
            String query = "exec HoaDonBan @id_hdb , @ma_sp , @ten_sp , @gia , @sl ";
            foreach (ListViewItem item in listView1.Items)
            {
                String masp=item.SubItems[0].Text.ToString();
                String ten = item.SubItems[3].Text.ToString();
                float gia = float.Parse(item.SubItems[6].Text.ToString());
                int sl = int.Parse(item.SubItems[4].Text.ToString());
                int doi = int.Parse(item.SubItems[10].Text.ToString());
                sl = sl * doi;
                DataProvider.Instance.ExecuteNonQuery(query, new object[] { id , masp , ten , gia , sl });
                query = "exec update_sanpham_ban @sl , @id ";
                DataProvider.Instance.ExecuteNonQuery(query, new object[] { sl, masp});
            }
            query = "exec tb_HDB @id_khach , @id_nv , @tongtien , @tienno ";
            String makhach = ma_khach.Text;
            String manv = Manv;
            String tam = txt_tien.Text;
            float tt = float.Parse(tam.Split('đ')[0]);
            tam = txt_tra.Text;
            float tienno = tt- float.Parse(txt_dua.Text);
            if (tienno < 0)tienno = 0;
             DataProvider.Instance.ExecuteNonQuery(query, new object[] { makhach , manv , tt , tienno });
            
        }
        #endregion
        private void Loadhoadon()
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke((MethodInvoker)delegate ()
                {
                    Inhoadon.Instance.listview = new ListView();

                    foreach (ListViewItem item in listView1.Items)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.SubItems.Add(item.SubItems[9].Text.ToString());
                        lvi.SubItems.Add(item.SubItems[2].Text.ToString());
                        lvi.SubItems.Add(item.SubItems[3].Text.ToString());
                        lvi.SubItems.Add(item.SubItems[4].Text.ToString());
                        lvi.SubItems.Add(item.SubItems[5].Text.ToString());
                        lvi.SubItems.Add(item.SubItems[6].Text.ToString());
                        lvi.SubItems.Add(item.SubItems[7].Text.ToString());
                        Inhoadon.Instance.listview.Items.Add(lvi);
                    }
                });

            }
            Inhoadon.Instance.tongtien = txt_tien.Text;
            Inhoadon.Instance.khachdua = txt_dua.Text;
            Inhoadon.Instance.tralai = txt_tra.Text;
            Inhoadon.Instance.khachhang = txt_tenkhach.Text;
            Inhoadon.Instance.nvban = Manv+"-"+ Tennv;

            Inhoadon.Instance.Show();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
