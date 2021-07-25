using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ql_nhat.DTO
{
    class Inhoadon
    {
        public ListView listview = null;
        private static Inhoadon instance;
        public static Inhoadon Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Inhoadon();
                }
                return Inhoadon.instance;
            }
            set { Inhoadon.instance = value; }
        }
        public String tongtien = null;
        public String khachdua = null;
        public String nvban = null;
        public String khachhang = null;
        public String tralai = null;
        private PrintDialog printDialog;
        private PrintPreviewDialog printPreviewDialog;
        private PrintDocument printDocument;
        public Inhoadon()
        {
            printDialog = new PrintDialog();
            printPreviewDialog = new PrintPreviewDialog();
            printDocument = new PrintDocument();
            this.printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);

            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("hoadon", 350, 600);
        }
        private static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        int startX = 0;
        int startY = 0;
        int Offset = 0;
        private int chia(Double pt)
        {
            Double w = printDocument.DefaultPageSettings.PaperSize.Width;
            return (int)(pt * w) / 100;
        }
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Offset += 5;
            Center(e, Str("nt"));
            Offset = Offset + 25;
            startX = chia(3);
            
            graphics.DrawString("Số HD: ", font(8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            graphics.DrawString("Khách Hàng : "+khachhang, font(8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            graphics.DrawString("Nhân Viên: " + nvban, font(8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            Left(e, Str("dc"), 8, startX, startY + Offset);
            Offset += 16;
            graphics.DrawString(Str("day"), font(8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            KeNgang(e);
            Offset = Offset + 5;
            Left(e, "Tên", 8, startX, startY + Offset);
            Left(e, "SL/DVT", 8, chia(30), startY + Offset);
            Left(e, "Đơn giá", 8, chia(45), startY + Offset);
            Left(e, "Thành Tiền", 8, chia(70), startY + Offset);
            Offset = Offset + 10;
            KeNgang(e);
            Offset = Offset + 10;
            Load_list(e);
            load_monney(e);
            footer(e);
        }
        private void load_monney(PrintPageEventArgs e)
        {
            String no = (Double.Parse(tongtien.Split('đ')[0])-Double.Parse(khachdua)).ToString();
            if (Double.Parse(no) < 0)
            {
                no = "0";
            }
            Left(e, "Tổng Tiền : "+ String.Format("{0:#,##0.##}", int.Parse(tongtien.Split('đ')[0])) +"đ("+Xuly.Instance.ChuyenSoSangChuoi(Double.Parse(tongtien.Split('đ')[0]))+")", 8, startX, startY + Offset);
            Offset = Offset + 10;
            Left(e, "Khách Đưa: " + String.Format("{0:#,##0.##}", int.Parse(khachdua.Split('đ')[0]))  +"đ", 8, startX, startY + Offset);
            Offset = Offset + 10;
            Left(e, "Trả Lại: "+ String.Format("{0:#,##0.##}", int.Parse(tralai.Split('đ')[0]))+"đ" , 8, startX, startY + Offset);
            Offset = Offset + 10;
            Left(e, "Nợ : "+ String.Format("{0:#,##0.##}", int.Parse(no.Split('đ')[0])) + "đ", 8, startX, startY + Offset);
            Offset = Offset + 10;
        }
        private void KeNgang(PrintPageEventArgs e)
        {
            SizeF stringSize = new SizeF();
            Double w = e.PageSettings.PaperSize.Width;
            Graphics gfx = Graphics.FromImage(new Bitmap(1, 1));
            stringSize = gfx.MeasureString("-", font(5));
            // This will give you string width, from which you can calculate further 
            Double width = stringSize.Width;
            Double tam = width;
            String text = null;
            while (tam <= (w - chia(10)))
            {
                text += "-";
                tam = gfx.MeasureString(text, font(5)).Width;
            }
            e.Graphics.DrawString(text, font(5), new SolidBrush(Color.Black), startX, startY + Offset);

        }
        private void Load_list(PrintPageEventArgs e)
        {
            if (listview != null)
            {
                for (int i = 0; i < cout_layloai(); i++)
                {
                    Left(e, layloai()[i], 7, startX, startY + Offset);
                    Offset += 8;
                    KeNgang(e);
                    Offset += 10;
                    foreach (ListViewItem item in listview.Items)
                    {
                        if (layloai()[i].ToString() == item.SubItems[2].Text)
                        {
                            Left(e, item.SubItems[3].Text, 6, startX + chia(1), startY + Offset);
                            String addd = (item.SubItems[4].Text + "/" + item.SubItems[5].Text);
                            Left(e, addd, 6, chia(30), startY + Offset, dorongin(chia(30), chia(45)).ToString());
                            Left(e, String.Format("{0:#,##0.##}", int.Parse(item.SubItems[6].Text.Split('đ')[0])) , 6, chia(45), startY + Offset, dorongin(chia(47), chia(70)).ToString());
                            Left(e, String.Format("{0:#,##0.##}", int.Parse(item.SubItems[7].Text.Split('đ')[0])), 6, chia(70), startY + Offset, dorongin(chia(72), chia(90)).ToString());
                            Offset += 8;
                            KeNgang(e);
                            Offset += 8;
                        }
                    }
                }

            }
        }
        private Double dorongin(Double d1,Double d2)
        {
            return (d2 - d1);
        }
        private int cout_layloai()
        {
            int i = 0;
            foreach (String item in layloai())
            {
                if (item!=null)
                {
                    i++;
                }
            }
            return i;
        }
        private String[] layloai()
        {
            String[] data=new String[listview.Items.Count];
            String key = null;
            
            int tam2 = 1;
            data[0] = listview.Items[0].SubItems[2].Text;
            for (int i = 0; i < listview.Items.Count; i++)
            {
                int tam = 0;
                for (int j = 0 ; j < tam2; j++)
                {
                    if (String.Compare(listview.Items[i].SubItems[2].Text.ToString(), data[j].ToString()) == 0)
                    {
                        tam++;
                    }
                }
                if (tam == 0)
                {
                    data[tam2] = listview.Items[i].SubItems[2].Text;
                    tam2++;
                   
                }
            }
            return data;
        }
        // Trả về các text mặc định
        #region Str(key)
        private String Str(String key)
        {
            String s = null;
            try
            {
                switch (key)
                {
                    case "nt":
                        {
                            s = "--- Nông Trại 36 ---";
                            break;
                        }
                    case "dc":
                        {
                            s = "Địa Chỉ : Thành Nàng -  Tân Thành - Thường Xuân - Thanh Hóa";
                            break;
                        }
                    case "day":
                        {
                            s = "Date :" + DateTime.Now.ToString("dd/MM/yyyy HH-mm");
                            break;
                        }
                }
                return s;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion
        private void Center(PrintPageEventArgs e, String text)
        {
            SizeF stringSize = new SizeF();
            Double w = e.PageSettings.PaperSize.Width;
            Graphics gfx = Graphics.FromImage(new Bitmap(1, 1));
            stringSize = gfx.MeasureString(text, font(12));
            // This will give you string width, from which you can calculate further 
            Double width = stringSize.Width;
            Double tam = (w - width) / 2;
            e.Graphics.DrawString(text, font(12), new SolidBrush(Color.Black), new Point((int)tam,startY+Offset));
        }
        private void Left(PrintPageEventArgs e, String text, int f, int x, int y, String dorong = null)
        {
            Double w = e.PageSettings.PaperSize.Width;
            if (dorong==null)
            {
                Cat(e, w, text, f, x, y);
            }
            else
            {
                Cat(e, w, text, f, x, y,dorong);
            }

        }
        private Double Width(String text, int size_font)
        {
            SizeF stringSize = new SizeF();
            Graphics gfx = Graphics.FromImage(new Bitmap(1, 1));
            stringSize = gfx.MeasureString(text, font(size_font));
            return stringSize.Width;
        }
        private void Cat(PrintPageEventArgs e, Double w, String text, int f, int x, int y,String dorong = null)
        {
            Boolean kt = false;
            Boolean check = false;
            String tam = null;
            String tam2 = dorong;
            Double d = 0;
            if (dorong == null)
            {
                if (Width(text, f) <= (w - chia(10)))
                {
                    Text(e, text, x, y,f.ToString());
                }
                else
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        tam += text[i];
                        d = Width(tam, (f));
                        if (d >= (w - chia(10)))
                        {
                            while (text[i] != ' ')
                            {
                                i--;
                                tam = tam.Remove(i);
                            }
                            Cat(e, w, tam, f, x, y);
                            y += 16;
                            tam = "";
                            check = true;
                        }
                    }
                    if (check == true)
                    {
                        Text(e, tam, x, y);
                        Offset += 16;
                    }
                }
            }
            else
            {
                kt = false;
                Double dr = Double.Parse(dorong);
                Double ls = Width(text, f);
                if (Width(text, f) <= (dr))
                {
                    Text(e, text, x, y,f.ToString());
                }
                else
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        tam += text[i];
                        d = Width(tam, (f));
                        if (d >= (dr))
                        {
                            while (text[i] != ' ')
                            {
                                i--;
                                if (i == -1)
                                {
                                    kt = true;
                                }
                                else
                                {
                                    tam = tam.Remove(i);
                                }
                            }
                            if (kt==true)
                            {
                                tam = tam.Remove(i);
                            }
                            Cat(e, dr, tam, f, x, y, dorong);
                            y += 16;
                            tam = "";
                            check = true;
                            kt = false;
                        }
                    }
                    if (check == true)
                    {
                        Text(e, tam, x, y);
                        Offset += 16;
                    }
                }
            }
            
        }
        private void Text(PrintPageEventArgs e, String text, int x, int y, String size = null)
        {
            if (size == null)
            {
                e.Graphics.DrawString(text, new Font("Courier New", 8), Brushes.Black, new Point(x, y));
            }
            else
            {
                e.Graphics.DrawString(text, new Font("Courier New", int.Parse(size)), Brushes.Black, new Point(x, y));
            }
        }
        void title(PrintPageEventArgs e, String text, int h)
        {
            e.Graphics.DrawString(text, new Font("Courier New", 10), Brushes.Black, new Point(h, 70));
        }
        public void In()
        {
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        public void Show()
        {
            try
            {
                Offset = 0;
                
                printPreviewDialog.Document = printDocument;
                
                printPreviewDialog.ShowDialog();
            }
            catch (Exception) { }

        }
        private void footer(PrintPageEventArgs e)
        {
            Offset += 15;
            Center(e, "*** Xin Cảm ơn ***");
        }
        private Font font(int size)
        {
            return (new Font("Courier New", size));
        }
    }
}
