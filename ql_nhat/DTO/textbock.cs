using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ql_nhat.DTO
{
    class textbock
    {
        private static textbock instance;

        public static textbock Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new textbock();
                }
                return textbock.instance;
            }
            private set { textbock.instance = value; }

        }

        public void Pree_key(object sender,KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))

            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
                
            }
        }
    }
}
