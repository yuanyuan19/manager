using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    public partial class BorrowForm : UserControl
    {
        MainForm f;
        public BorrowForm(MainForm f)
        {
            InitializeComponent();
            this.f = f;
        }

        private void BorrowForm_Load(object sender, EventArgs e)
        {

        }
    }
}
