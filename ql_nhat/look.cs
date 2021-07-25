using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ql_nhat
{
    public partial class look : Form
    {
        public look()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.pictureBox1.BackColor= Color.Black;
        }
       
    }
}
