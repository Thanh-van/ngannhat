using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ql_nhat.DTO
{
    class sms
    {
        //Dùng để show các thông báo trong chương trình
        #region KhoiTao()
        private static sms show;

        public static sms Show
        {
            get
            {
                if (show == null)
                {
                    show = new sms();
                }
                return sms.show;
            }
            private set { sms.show = value; }
        }
        #endregion

        // Hàm dùng để đưa ra cảnh báo
        #region Waring
        public void Waring(String text,String title = null)
        {
            if (title != null)
            {
                MessageBox.Show(text,title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            MessageBox.Show(text, "Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }
        #endregion

        // Hàm dùng để hỏi
        #region Dialog
        public DialogResult Dialog(String text)
        {
                DialogResult dl = MessageBox.Show(text, "Thông Báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dl;
        }
        #endregion

        // Hàm dùng để đưa ra cảnh báo Error
        #region Error
        public void Error(String text, String title = null)
        {
            if (title != null)
            {
                MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show(text, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        // Hàm dùng để đưa ra thông báo Information
        #region Info
        public void Info(String text, String title = null)
        {
            if (title != null)
            {
                MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
                MessageBox.Show(text, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        #endregion
        // Hàm dùng để

        
        #region
        #endregion

        // Hàm dùng để
        #region
        #endregion


        // Hàm dùng để
        #region
        #endregion
    }
}
