using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    public partial class UserForm : UserControl
    {
        MainForm f;
        public UserForm(MainForm f)
        {
            InitializeComponent();
            this.f=f;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {

        }
    }
}
