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
    public partial class Loai : Form
    {
        public Loai()
        {
            InitializeComponent();
        }


        Boolean check_nhap = false;
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private String[] arr = null;
        private void Loai_Load(object sender, EventArgs e)
        {
            Combobox_data.Instance.Combobox(txt_loai, "select * from loai", "ten", "ma_loai");
            tabControl1.TabPages.Clear();

            tabControl1.TabPages.Add(home);
        }
        // Load sản phẩm
        #region load_product
        private void load_product()
        {
            String query = "exec tb_sanpham";
            data_sp.DataSource = DataProvider.Instance.ExecuteQuery(query);
            this.data_sp.Columns[0].Visible = false;
            this.data_sp.Columns[1].Visible = false;
            this.data_sp.Columns[8].Visible = false;
        }
        #endregion
        private void Sanpham(object sender, EventArgs e)
        {
            tab_Page(tab_sp);
            load_product();
            arr = new string[data_sp.RowCount];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = data_sp.Rows[i].Cells[1].Value.ToString();

            }
            load_Ma();
            Xuly.Instance.setLabel(data_sp, label1);
        }
        // Load Mã sản phẩm
        #region load_Ma
        private void load_Ma()
        {
            if (int.Parse(create_Masp()) < 10)
            {
                txt_masp.Text = "SP0" + (int.Parse(create_Masp()) + 1).ToString();

            }
            else
            {
                txt_masp.Text = "SP" + (int.Parse(create_Masp()) + 1).ToString();
            }
            txt_madvt.Text = txt_masp.Text + "V1";
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
        private void Nhap_hang(object sender, EventArgs e)
        {
            Xuly.Instance.btn_enabled(fromnhap_btn_sua, fromnha_btn_x, 1);
            check_nhap = false;
            tab_Page(tab_nhaphang);
            Xuly.Instance.create_list(listView1);
            load_listLoai();
            check_nhap = true;
            load_product(Ma_loai);

        }
        // Chuyển tab
        #region tab_Page
        private void tab_Page(TabPage tab)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(home);
            tabControl1.TabPages.Add(tab);
            tabControl1.SelectedTab = tab;
        }
        #endregion
        private void Chitiet(object sender, EventArgs e)
        {
            tab_Page(tab_ct);
            data_loai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Thread dvt = new Thread(load_data_dvt);
            dvt.Start();
            Thread dvt2 = new Thread(load_data_loai);
            dvt2.Start();
            txt_vt.Text = load_vitri().ToString();
            load_combobox_sp();
            refesh_dvt_from();

        }
        private void create_ma_dvt()
        {
            if (data_dvt.Rows.Count > 0)
            {
                int tam = 0;
                for (int i = 0; i < data_dvt.Rows.Count - 1; i++)
                {
                    if (txt_ma_dvt.Text == data_dvt.Rows[i].Cells[1].Value.ToString())
                    {
                        tam++;
                    }
                }
                txt_ma_dvt.Text += "V" + (tam + 1).ToString();
            }
        }
        private void load_combobox_sp()
        {
            String sql = "select * from sanpham";
            Combobox_data.Instance.Combobox(txt_sp_fromdvt, sql, "ten", "ma_sp");
        }
        // load bảng đơn vị tính
        #region load_data_dvt
        private void load_data_dvt()
        {
            String query = "exec show_data_dvt";
            data_dvt.Invoke(new MethodInvoker(delegate ()
            {
                data_dvt.DataSource = DataProvider.Instance.ExecuteQuery(query);
                txt_ma_dvt.Text = txt_sp_fromdvt.SelectedValue.ToString();
                create_ma_dvt();
            }));
        }
        #endregion
        // Load data chi tiết
        #region load_data_loai
        private void load_data_loai()
        {
            String query = "  select  ma_loai as 'Mã',ten as 'Tên Loại',vitri as 'Vị Trí' from loai";
            data_loai.Invoke(new MethodInvoker(delegate ()
            {
                data_loai.DataSource = DataProvider.Instance.ExecuteQuery(query);
            }));

        }
        #endregion

        private void tab_sp_Click(object sender, EventArgs e)
        {

        }

        private void data_sp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        #region data_sp_CellClick
        private void data_sp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_them.Text = "Refresh";
            txt_masp.Enabled = false;

            txt_masp.Text = data_sp.CurrentRow.Cells[1].Value.ToString();
            txt_loai.SelectedValue = data_sp.CurrentRow.Cells[0].Value.ToString();
            txt_namesp.Text = data_sp.CurrentRow.Cells[2].Value.ToString();
            txt_gianhap.Text = data_sp.CurrentRow.Cells[4].Value.ToString();
            txt_giaban.Text = data_sp.CurrentRow.Cells[5].Value.ToString();
            txt_dvt.Text = data_sp.CurrentRow.Cells[6].Value.ToString();
            txt_sl.Text = data_sp.CurrentRow.Cells[7].Value.ToString();
            txt_madvt.Text = data_sp.CurrentRow.Cells[8].Value.ToString();
        }
        #endregion

        private void txt_seach_TextChanged(object sender, EventArgs e)
        {
            String sql = null;
            if (txt_seach.Text.Trim() == null)
            {
                sql = "exec tb_sanpham";
                data_sp.DataSource = DataProvider.Instance.ExecuteQuery(sql);

            }
            else
            {
                sql = "exec tb_sanpham_s @key";
                data_sp.DataSource = DataProvider.Instance.ExecuteQuery(sql, new object[] { txt_seach.Text.Trim() });
            }


            this.data_sp.Columns[0].Visible = false;
            this.data_sp.Columns[8].Visible = false;
            Xuly.Instance.setLabel(data_sp, label1);
        }
        /// <summary>
        /// Kiểm tra xem giá nhập và giá bán
        /// </summary>
        #region 
        private Boolean check_gia()
        {
            try
            {
                if (int.Parse(txt_gianhap.Text) > int.Parse(txt_giaban.Text))
                {
                    return false;
                }
                else return true;
            }
            catch
            {
            }
            return false;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            if (btn_them.Text == "Thêm")
            {
                if (check_gia())
                {
                    MessageBox.Show(txt_loai.SelectedValue.ToString());
                }
                else
                {
                    MessageBox.Show("false");
                }
            }
            else
            {
                btn_them.Text = "Thêm";
                clear();
            }
        }
        // clear form text 
        #region clear
        private void clear()
        {
            load_Ma();
            txt_namesp.Text = "";
            txt_giaban.Text = "0";
            txt_gianhap.Text = "0";
            txt_dvt.Text = "";
            txt_sl.Text = "0";
        }
        #endregion
        // check có chọn sản phẩm để xóa chưa
        #region check_ma
        private Boolean check_ma()
        {
            if (arr.Length > 0)
            {
                foreach (String item in arr)
                {
                    if (item == txt_masp.Text)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            if (check_ma())
            {
                MessageBox.Show("Tồn tại");
            }
            else
            {
                MessageBox.Show("Không tồn tại");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void data_loai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_them_chitiet.Text = "Refresh";
            txt_maloai.Enabled = false;
            txt_maloai.Text = data_loai.CurrentRow.Cells[0].Value.ToString();
            txt_tenloai.Text = data_loai.CurrentRow.Cells[1].Value.ToString();
            txt_vt.Text = data_loai.CurrentRow.Cells[2].Value.ToString();
        }
        private Boolean check_rong(String x)
        {
            if (x.Trim() == "")
            {
                return false;
            }
            else return true;
        }
        private Boolean check_add_chitiet()
        {
            if (check_rong(txt_maloai.Text))
            {
                if (check_rong(txt_tenloai.Text))
                {
                    try
                    {
                        if (int.Parse(txt_vt.Text) > 0)
                        {
                            return true;
                        }
                        else return false;

                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else return false;
            }
            else
            {
                return false;
            }
        }
        // check trùng mã loại
        private Boolean check_ma_trung()
        {
            if (data_loai.Rows.Count > 0)
            {
                for (int i = 0; i < data_loai.Rows.Count - 1; i++)
                {
                    if (txt_maloai.Text == data_loai.Rows[i].Cells[0].Value.ToString())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void form_loai_add(object sender, EventArgs e)
        {
            if (btn_them_chitiet.Text == "Thêm")
            {
                if (check_add_chitiet())
                {
                    if (check_ma_trung())
                    {
                        String query = "exec insert_loai @ma_loai , @ten_loai , @vt";
                        int i = DataProvider.Instance.ExecuteNonQuery(query, new object[] { txt_maloai.Text.Trim(), txt_tenloai.Text, txt_vt.Text });
                        if (i == 1)
                        {
                            MessageBox.Show("Thêm Thành công");
                            load_data_loai();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi Hệ thống");
                        }
                    }
                    else
                    {
                        sms.Show.Error( "Trùng dadddIP", "Lỗi Thêm");
                    }

                }
                else
                {
                    sms.Show.Error("Không được dể trống","Lỗi thêm" );
                }
            }
            else
            {
                btn_them_chitiet.Text = "Thêm";
                txt_maloai.Enabled = true;
                txt_maloai.Text = "";
                txt_tenloai.Text = "";
                txt_vt.Text = load_vitri().ToString();
            }
        }
        // load vị trí max
        #region load_vitri
        private int load_vitri()
        {
            if (data_loai.Rows.Count > 0)
            {
                int tam = int.Parse(data_loai.Rows[0].Cells[2].Value.ToString());
                for (int i = 0; i < data_loai.Rows.Count - 1; i++)
                {
                    if (tam < int.Parse(data_loai.Rows[i].Cells[2].Value.ToString()))
                    {
                        tam = int.Parse(data_loai.Rows[i].Cells[2].Value.ToString());
                    }
                }
                return tam + 1;
            }
            return 1;
        }
        #endregion


        private void btn_sua_chitiet_Click(object sender, EventArgs e)
        {
            if (check_add_chitiet())
            {
                String query = "update loai set ten = N'" + txt_tenloai.Text + "',vitri=" + txt_vt.Text + "  where ma_loai='" + txt_maloai.Text + "'";
                int n = DataProvider.Instance.ExecuteNonQuery(query);
                if (n == 1)
                {
                    MessageBox.Show("Sửa Thành công");
                }
                load_data_loai();
            }

        }
        private void refesh_dvt_from()
        {
            txt_ma_dvt.Text = txt_sp_fromdvt.SelectedValue.ToString();
            create_ma_dvt();
            txt_lock.Text = "2";
            txt_quydoi.Text = "1";
            txt_tendvt.Text = "";
            txt_giaban_dvt.Text = "";
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (btn_form_dvt_add.Text == "Thêm")
            {
                String Ma = txt_sp_fromdvt.SelectedValue.ToString();
                String query = "exec insert_dvt @ma_dvt , @ma_sp , @ten , @giaban , @doi";
                int i = DataProvider.Instance.ExecuteNonQuery(query, new object[] { txt_ma_dvt.Text, Ma, txt_tendvt.Text, int.Parse(txt_giaban_dvt.Text), int.Parse(txt_quydoi.Text) });
                if (i == 1)
                {
                    //load_data_dvt();
                    this.Refresh();
                    sms.Show.Info("Sửa Thành Công");
                }
                else
                {
                    MessageBox.Show("False");
                }
            }
            else
            {
                btn_form_dvt_add.Text = "Thêm";
                refesh_dvt_from();
                txt_lock.Enabled = false;
            }
        }

        private void data_dvt_cellcontentclick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                btn_form_dvt_add.Text = "Refresh";

                txt_sp_fromdvt.SelectedValue = data_dvt.CurrentRow.Cells[1].Value.ToString();
                txt_ma_dvt.Text = data_dvt.CurrentRow.Cells[0].Value.ToString();
                txt_gianhap_dvt.Text = data_dvt.CurrentRow.Cells[3].Value.ToString();
                txt_giaban_dvt.Text = data_dvt.CurrentRow.Cells[4].Value.ToString();
                txt_tendvt.Text = data_dvt.CurrentRow.Cells[5].Value.ToString();
                txt_quydoi.Text = data_dvt.CurrentRow.Cells[6].Value.ToString();
                txt_lock.Text = data_dvt.CurrentRow.Cells[7].Value.ToString();
                //create_ma_dvt();
                txt_lock.Enabled = true;
            }
            catch (Exception)
            {

            }
        }

        private void txt_sp_fromdvt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox a = sender as ComboBox;
                if (a.Items.Count > 0)
                {
                    txt_ma_dvt.Text = txt_sp_fromdvt.SelectedValue.ToString();
                    create_ma_dvt();
                    change_gia();
                    refesh_dvt_from();
                }
            }
            catch(Exception) {  }
            
        }
        private void change_gia()
        {
            if (data_dvt.Rows.Count > 0)
            {
                for (int i = 0; i < data_dvt.Rows.Count; i++)
                {
                    if (txt_sp_fromdvt.SelectedValue.ToString() == data_dvt.Rows[i].Cells[1].Value.ToString())
                    {
                        txt_gianhap_dvt.Text = data_dvt.Rows[i].Cells[3].Value.ToString();
                        break;
                    }
                }
            }
        }

        private void btn_sua_fromdvt_Click(object sender, EventArgs e)
        {
            String query = "exec update_data_dvt @ma , @ten , @giaban , @doi , @lock";
            int i = DataProvider.Instance.ExecuteNonQuery(query, new object[] { txt_ma_dvt.Text, txt_tendvt.Text, int.Parse(txt_giaban_dvt.Text), int.Parse(txt_quydoi.Text), int.Parse(txt_lock.Text) });
            if (i == 1)
            {
                //load_data_dvt();
                this.Invalidate();
                sms.Show.Info("Sửa Thành Công");
            }
            else
            {
                MessageBox.Show("False");
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }
        #region check_add_id
        private Boolean check_add_id()
        {
            int sl = int.Parse(formnhap_txt_sl.Value.ToString());
            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (Ma_sp == item.SubItems[0].Text && Ma_dvt == item.SubItems[8].Text)
                    {
                        item.SubItems[4].Text = Convert.ToString(int.Parse(item.SubItems[4].Text) + formnhap_txt_sl.Value);
                        item.SubItems[7].Text = Convert.ToString(int.Parse(item.SubItems[4].Text) * int.Parse(item.SubItems[6].Text));
                        chage_meney();
                        return false;
                    }

                }
            }
            return true;
        }
        #endregion
        #region add_item
        private void add_item()
        {
            if (check_add_id() != false)
            {
                if (formnhap_txt_nameproduct.Items.Count > 0)
                {
                    string[] arr = new string[10];
                    ListViewItem itm;
                    //Thêm Item đầu tiên
                    arr[0] = Ma_sp;
                    arr[1] = (listView1.Items.Count + 1).ToString();
                    arr[2] = fromnhap_list_loai.Text;
                    arr[3] = formnhap_txt_nameproduct.Text;
                    arr[4] = formnhap_txt_sl.Value.ToString();
                    arr[5] = fromnhap_dvt.Text;
                    arr[6] = txt_dongia.Text;
                    arr[7] = Convert.ToString(formnhap_txt_sl.Value * int.Parse(txt_dongia.Text.ToString()));
                    arr[8] = Ma_dvt;
                    arr[9] = Ma_loai;
                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);
                    chage_meney();
                }
                else
                {
                    sms.Show.Error("Không Có Sản Phẩm");
                }

            }
        }
        #endregion
        private void chage_meney()
        {
            int tam = 0;
            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    tam += int.Parse(item.SubItems[7].Text);
                }
            }
            label12.Text = tam.ToString() + "đ";
        }
        #region Thêm hàng vào hóa đơn nhập
        private void button1_Click_1(object sender, EventArgs e)
        {
            add_item();
        }
        #endregion
        private String Ma_loai = null, Ten_loai = null, Ma_sp = null, Ma_dvt = null;
        private void fromnhap_list_loai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (check_nhap)
            {
                ComboBox cb = (ComboBox)sender;
                if (cb.Items.Count > 0)
                {

                    try
                    {
                        Ma_loai = fromnhap_list_loai.SelectedValue.ToString();
                        Ten_loai = fromnhap_list_loai.SelectedText.ToString();
                        String key = fromnhap_list_loai.SelectedValue.ToString();
                        String sql = "select * from sanpham where ma_loai = N'" + key + "'";
                        Combobox_data.Instance.Combobox(formnhap_txt_nameproduct, sql, "ten", "ma_sp");

                    }
                    catch (Exception a)
                    {

                        sms.Show.Error(a.ToString());

                    }
                }
                else
                {
                    sms.Show.Waring("Không có dữ liệu");
                }

            }

        }
        #region Combobox loại
        void load_listLoai()
        {
            String sql = "select * from loai where ma_loai in (select ma_loai from sanpham)";
            Combobox_data.Instance.Combobox(fromnhap_list_loai, sql, "ten", "ma_loai");
            Ma_loai = fromnhap_list_loai.SelectedValue.ToString();
            Ten_loai = fromnhap_list_loai.SelectedText.ToString();
        }
        #endregion
        private void formnhap_txt_nameproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cb = (ComboBox)sender;
                if (cb.Items.Count > 0)
                {
                    Ma_sp = formnhap_txt_nameproduct.SelectedValue.ToString();
                    Xuly.Instance.load_dvt(fromnhap_dvt, Ma_sp);
                    dongia_chage();
                }
            }
            catch (Exception a)
            { }
        }
        private int Sl;
        private int chuyendoi;

        private void fromnhap_dvt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Ma_dvt = fromnhap_dvt.SelectedValue.ToString();
                dongia_chage(fromnhap_dvt.SelectedValue.ToString());
            }
            catch (Exception a)
            {

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Xuly.Instance.btn_enabled(fromnhap_btn_sua, fromnha_btn_x, 2);
            Xuly.Instance.select(sender, fromnhap_list_loai, formnhap_txt_nameproduct, fromnhap_dvt, Ma_sp, txt_dongia, formnhap_txt_sl);
        }

        private void fromnhap_btn_sua_Click(object sender, EventArgs e)
        {
            Boolean check = false;
            if (listView1.Items.Count > 0)
            {

                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.SubItems[0].Text == formnhap_txt_nameproduct.SelectedValue.ToString() && fromnhap_dvt.SelectedValue.ToString() == item.SubItems[8].Text)
                    {
                        check = true;
                        item.SubItems[4].Text = formnhap_txt_sl.Value.ToString();
                        item.SubItems[7].Text = Convert.ToString(int.Parse(item.SubItems[4].Text) * int.Parse(item.SubItems[6].Text));
                        Xuly.Instance.change_meney(listView1, label12);
                        sms.Show.Info("Sửa thành công");
                        Xuly.Instance.btn_enabled(fromnhap_btn_sua, fromnha_btn_x, 1);
                    }
                }
                if (check != true)
                {
                    sms.Show.Waring("Vui lòng thêm mới sản phẩm");
                }
            }

        }

        private void xoa_loai(object sender, EventArgs e)
        {

        }

        private void txt_quydoi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txt_giaban_dvt.Text = (int.Parse(txt_gianhap_dvt.Text) * int.Parse(txt_quydoi.Text)).ToString();
            }
            catch (Exception)
            {

            }

        }
#region Cam Nhap text cho Text box
        private void txt_quydoi_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }

        private void txt_giaban_dvt_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }

        private void txt_vt_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }

        private void formnhap_txt_sl_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }

        private void txt_gianhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }

        private void txt_giaban_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbock.Instance.Pree_key(sender, e);
        }
        #endregion
        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dl = sms.Show.Dialog("Bạn có muốn xóa ??");
            if (dl==DialogResult.Yes)
            {
                String query = "delete * from dvt where ma_dvt = " + txt_ma_dvt.Text + "";
                int i = DataProvider.Instance.ExecuteNonQuery(query, new object[] { txt_ma_dvt.Text, txt_tendvt.Text, int.Parse(txt_giaban_dvt.Text), int.Parse(txt_quydoi.Text), int.Parse(txt_lock.Text) });
                if (i == 1)
                {
                    //load_data_dvt();
                    this.Invalidate();
                    sms.Show.Info("Sửa Thành Công");
                }
                else
                {
                    MessageBox.Show("False");
                }
            }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            nhap();
        }
        private void nhap()
        {
            int id = Xuly.Instance.getid("tb_hoadonnhap");
            String query = "exec tb_CTHDN  @id_hd , @id_sp , @ten_sp , @dg , @sl ";
            foreach (ListViewItem item in listView1.Items)
            {
                String masp = item.SubItems[0].Text.ToString();
                String ten = item.SubItems[3].Text.ToString();
                float gia = float.Parse(item.SubItems[6].Text.ToString());
                int sl = int.Parse(item.SubItems[4].Text.ToString());
                int doi = int.Parse(item.SubItems[10].Text.ToString());
                sl = sl * doi;
                DataProvider.Instance.ExecuteNonQuery(query, new object[] { id, masp, ten, gia, sl });
                query = "exec update_sanpham_nhap @sl , @id ";
                DataProvider.Instance.ExecuteNonQuery(query, new object[] { sl, masp });
            }
           
            query = "exec tb_HDN @id_nv , @tongtien ";
            
            String tam = label12.Text;
            float tt = float.Parse(tam.Split('đ')[0]);
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { Login.manv , tt});
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Hoadon f = new Hoadon();         
            this.Hide();
            f.ShowDialog();

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fromnha_btn_x_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean check = false;

                if (listView1.Items.Count > 0)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {

                        if (item.SubItems[0].Text == formnhap_txt_nameproduct.SelectedValue.ToString() && fromnhap_dvt.SelectedValue.ToString() == item.SubItems[8].Text)
                        {
                            DialogResult dl = sms.Show.Dialog("Bạn có muốn xóa sản phẩm " + item.SubItems[3].Text);
                            if (dl == DialogResult.Yes)
                            {
                                check = true;
                                listView1.Items.Remove(item);
                                chage_meney();
                                Xuly.Instance.btn_enabled(fromnhap_btn_sua, fromnha_btn_x, 1);
                            }

                        }
                    }

                }
                int i = 0;
                if (check == true)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        i++;
                        item.SubItems[1].Text = i.ToString();
                    }
                }
            }
            catch { }
        }
        #region dongia_chage
        private void dongia_chage(String key = null)
        {
            String sql = null;
            if (key == null)
            {
                sql = "select * from dvt,sanpham where dvt.ma_sp='" + Ma_sp + "' and sanpham.ma_sp=dvt.ma_sp and dvt.trangthai=1";

            }
            else
            {
                sql = "select * from dvt,sanpham where dvt.ma_sp='" + Ma_sp + "' and sanpham.ma_sp=dvt.ma_sp and dvt.ma_dvt = '" + key + "'";
            }
            String[] arr = DataProvider.Instance.SqldataReader(sql);
            txt_dongia.Text = arr[0];
            if (arr[2] != null)
            {
                Sl = int.Parse(arr[1]);
                chuyendoi = int.Parse(arr[2]);
            }
        }
        #endregion
        private void load_product(String key)
        {
            String sql = "select * from sanpham  where ma_loai = N'" + key + "' ";
            Combobox_data.Instance.Combobox(formnhap_txt_nameproduct, sql, "ten", "ma_sp");
        }
    }
}
