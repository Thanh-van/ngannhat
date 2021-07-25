using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ql_nhat.DAO;
namespace ql_nhat.DTO
{
    class Xuly
    {
        private static Xuly instance;

        public static Xuly Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Xuly();
                }
                return Xuly.instance;
            }
            private set { Xuly.instance = value; }
        }
        // Hoạt động khi seach và set lại label
        #region setLabel
        public void setLabel(DataGridView data_khach,Label label1,String text = null)
        {
            try
            {
                label1.Text = "";
                if (text != null)
                {
                    label1.Text = "Tổng số khách: " + (data_khach.RowCount - 1).ToString();
                }
                else{
                    label1.Text = "Tổng : " +  (data_khach.RowCount - 1).ToString();
                }
                
            }
            catch (Exception e) { label1.Text = e.ToString(); }

        }
        #endregion
        // Hàm Dùng để check số lượng khi thêm sản phẩm mới vào list
        // Dùng cho form nhập hàng và form bán hàng
        #region CheckSL

        public Boolean check_sl(ListViewItem ls, String Ma_sp, NumericUpDown txt_sl, int chuyendoi, int Sl, String Ma_dvt = null)
        {
            int tam = 0;
            int sl = int.Parse(txt_sl.Value.ToString());
            if (Ma_dvt == null)
            {

                if (ls.SubItems[0].Text.ToString() == Ma_sp)
                {
                    tam += int.Parse(ls.SubItems[10].Text) * int.Parse(ls.SubItems[4].Text);
                }

                return xuly_check_sl(sl, chuyendoi, tam, Sl);
            }
            else
            {
                int dem = 0;

                if (ls.SubItems[0].Text.ToString() == Ma_sp)
                {
                    tam += int.Parse(ls.SubItems[10].Text) * int.Parse(ls.SubItems[4].Text);
                    if (ls.SubItems[0].Text.ToString() == Ma_sp && ls.SubItems[8].Text.ToString() == Ma_dvt)
                    {
                        dem += int.Parse(ls.SubItems[10].Text) * int.Parse(ls.SubItems[4].Text);
                    }
                }

                return xuly_check_sl(sl, chuyendoi, tam - dem, Sl);
            }

        }
        public Boolean xuly_check_sl(int sl, int chuyendoi, int tam, int Sl)
        {
            if ((sl * chuyendoi + tam) <= (Sl))
            {
                return true;
            }
            else return false;
        }
        #endregion

        // Hàm dùng để xóa các sản phẩm đã được thêm trong danh sách
        #region XoaSanPham
        public Boolean Xoa_SP(ListView listView1, ComboBox txt_nameproduct, ComboBox txt_dvt)
        {
            Boolean check = false;

            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {

                    if (item.SubItems[0].Text == txt_nameproduct.SelectedValue.ToString() && txt_dvt.SelectedValue.ToString() == item.SubItems[8].Text)
                    {
                        DialogResult dl = sms.Show.Dialog("Bạn có muốn xóa sản phẩm " + item.SubItems[3].Text.ToString());
                        if (dl == DialogResult.Yes)
                        {
                            check = true;
                            listView1.Items.Remove(item);

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
            return check;
        }
        #endregion

        // Hàm dùng để thay đổi tổng tiền 
        #region change_meney()
        public void change_meney(ListView listView1, Label txt_tien)
        {
            int tam = 0;
            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    tam += int.Parse(item.SubItems[7].Text);
                }
            }
            txt_tien.Text = tam.ToString() + "đ";
        }
        #endregion

        // Tạo danh sách hóa đơn
        #region Tạo hóa list
        public void create_list(ListView listView1)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("id", 0); //0
            listView1.Columns.Add("STT", 50); //1
            listView1.Columns.Add("Loại", 70);//2
            listView1.Columns.Add("Tên sản phẩm", 150); //3
            listView1.Columns.Add("Sl", 60);//4
            listView1.Columns.Add("DVT", 70);//5
            listView1.Columns.Add("Giá", 100);//6
            listView1.Columns.Add("Thành tiền", 150);//7
            listView1.Columns.Add("Mã DVT", 0);//8
            listView1.Columns.Add("Mã Loại", 0);//9
            listView1.Columns.Add("Chuyển đổi", 0);//10
        }
        #endregion

        public int getid(String tb)
        {
            String query = "SELECT IDENT_CURRENT('"+tb+"')+1";
            Object a = DataProvider.Instance.ExecuteScalar(query);
            String tam = Convert.ToString(a);
            int id = int.Parse(tam);
            return id;
        }
        public String sub_colum(String query)
        {
            
            Object a = DataProvider.Instance.ExecuteScalar(query);
            String tam = Convert.ToString(a);
            return tam;
        }
        // Hàm dùng để load khi click vào listview
        #region Loading in listview
        public Boolean select(object sender, ComboBox fromnhap_list_loai, ComboBox formnhap_txt_nameproduct, ComboBox fromnhap_dvt, String Ma_sp, Label txt_dongia, NumericUpDown formnhap_txt_sl)
        {
            try
            {
                ListView lsv = sender as ListView;
                if (lsv.SelectedItems.Count > 0)
                {
                    ListViewItem item = lsv.SelectedItems[0];
                    fromnhap_list_loai.SelectedValue = item.SubItems[9].Text;
                    String check = item.SubItems[0].Text;
                    formnhap_txt_nameproduct.SelectedValue = check;
                    Ma_sp = check;
                    load_dvt(fromnhap_dvt, Ma_sp);
                    String tam = item.SubItems[8].Text;
                    fromnhap_dvt.SelectedValue = tam;
                    txt_dongia.Text = item.SubItems[6].Text;
                    formnhap_txt_sl.Value = int.Parse(item.SubItems[4].Text);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion

        // Hàm dùng để load combobox đơn vị tính
        #region load_dvt
        public void load_dvt(ComboBox fromnhap_dvt, String Ma_sp)
        {
            String sql = " select * from dvt where ma_sp='" + Ma_sp + "' ";
            Combobox_data.Instance.Combobox(fromnhap_dvt, sql, "ten", "ma_dvt");
        }
        #endregion

        // Ẩn hiện thêm sửa xóa
        #region 1 show 2 hide (them)
        public void btn_enabled(Button btn_sua, Button btn_x, int key)
        {
            if (key == 1)
            {
                btn_sua.Enabled = false;
                btn_x.Enabled = false;
            }
            else
            {
                btn_sua.Enabled = true;
                btn_x.Enabled = true;
            }
        }
        #endregion
        
        public void docso(String dl)
        {
            int str = int.Parse(dl);
            String s = null;
            if (str % 3 == 0)
            {
                s = "Trăm";
                str /= 100;
            }
            else
            {
                if (str % 2 == 0)
                {
                    s = "Mươi";
                    str /= 10;
                }
            }
            
        }

        #region Insert__tb_Hoadonbanhang
        public int Insert_tbhdb(String id_hdb,String id_khach,String id_nv,String ngay,float tongtien,float no)
        {
            try
            {
                String sql = "insert into tb_hoadonban(id_hdb,id_khach,id_nv,ngay,tongtien,tienno) values()";
                int key = DataProvider.Instance.ExecuteNonQuery(sql);
                if (key == 0)
                {
                    return 0;
                }
                else return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        #endregion
        #region Insert__tb__chi tiet_Hoadonbanhang
        public int Insert_ct_tbhdb(String id_hdb, String id_sp, String ten_sp, float gia, int sl)
        {
            try
            {
                String sql = "insert into tb_chitiethoadonban(id_hdb,id_sp,ten_sp,sl) values()";
                int key = DataProvider.Instance.ExecuteNonQuery(sql);
                if (key == 0)
                {
                    return 0;
                }
                else return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        #endregion
        string[] mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');
         private string DocHangChuc(double so, bool daydu)
        {
            string chuoi = "";
            //Hàm để lấy số hàng chục ví dụ 21/10 = 2
            Int64 chuc = Convert.ToInt64(Math.Floor((double)(so / 10)));
            //Lấy số hàng đơn vị bằng phép chia 21 % 10 = 1
            Int64 donvi = (Int64)so % 10;
            //Nếu số hàng chục tồn tại tức >=20
            if (chuc > 1)
            {
                chuoi = " " + mNumText[chuc] + " mươi";
                if (donvi == 1)
                {
                    chuoi += " mốt";
                }
            }
            else if (chuc == 1)
            {//Số hàng chục từ 10-19
                chuoi = " mười";
                if (donvi == 1)
                {
                    chuoi += " một";
                }
            }
            else if (daydu && donvi > 0)
            {//Nếu hàng đơn vị khác 0 và có các số hàng trăm ví dụ 101 => thì biến daydu = true => và sẽ đọc một trăm lẻ một
                chuoi = " lẻ";
            }
            if (donvi == 5 && chuc >= 1)
            {//Nếu đơn vị là số 5 và có hàng chục thì chuỗi sẽ là " lăm" chứ không phải là " năm"
                chuoi += " lăm";
            }
            else if (donvi > 1 || (donvi == 1 && chuc == 0))
            {
                chuoi += " " + mNumText[donvi];
            }
            return chuoi;
        }
        private string DocHangTram(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng trăm ví du 434 / 100 = 4 (hàm Floor sẽ làm tròn số nguyên bé nhất)
            Int64 tram = Convert.ToInt64(Math.Floor((double)so / 100));
            //Lấy phần còn lại của hàng trăm 434 % 100 = 34 (dư 34)
            so = so % 100;
            if (daydu || tram > 0)
            {
                chuoi = " " + mNumText[tram] + " trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }
            return chuoi;
        }
        private string DocHangTrieu(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng triệu
            Int64 trieu = Convert.ToInt64(Math.Floor((double)so / 1000000));
            //Lấy phần dư sau số hàng triệu ví dụ 2,123,000 => so = 123,000
            so = so % 1000000;
            if (trieu > 0)
            {
                chuoi = DocHangTram(trieu, daydu) + " triệu";
                daydu = true;
            }
            //Lấy số hàng nghìn
            Int64 nghin = Convert.ToInt64(Math.Floor((double)so / 1000));
            //Lấy phần dư sau số hàng nghin 
            so = so % 1000;
            if (nghin > 0)
            {
                chuoi += DocHangTram(nghin, daydu) + " nghìn";
                daydu = true;
            }
            if (so > 0)
            {
                chuoi += DocHangTram(so, daydu);
            }
            return chuoi;
        }
        public string ChuyenSoSangChuoi(double so)
        {
            if (so == 0)
                return mNumText[0];
            string chuoi = "", hauto = "";
            Int64 ty;
            do
            {
                //Lấy số hàng tỷ
                ty = Convert.ToInt64(Math.Floor((double)so / 1000000000));
                //Lấy phần dư sau số hàng tỷ
                so = so % 1000000000;
                if (ty > 0)
                {
                    chuoi = DocHangTrieu(so, true) + hauto + chuoi;
                }
                else
                {
                    chuoi = DocHangTrieu(so, false) + hauto + chuoi;
                }
                hauto = " tỷ";
            } while (ty > 0);
            return chuoi + " đồng";
        }
    }
}
