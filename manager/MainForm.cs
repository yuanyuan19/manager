using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace manager
{
    public partial class MainForm : Form
    {
        UserControl[] controls = new UserControl[1];
        String username = "";
        String premission = "";
        public MainForm()
        {
            InitializeComponent();
            controls[0] = new WelcomeForm(this);
        }

        public void switchForm(int showform)
        {
            int j = 0;
            foreach(UserControl i in controls)
            {
                if (j == showform)
                    i.Show();
                else
                    i.Hide();
                j++;
            }
        }
        private void switchButtom(int[] showbuttom)
        {
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Visible = false;
            }
            for (int i=0;i<showbuttom.Length;i++)
            {
                int sho = showbuttom[i];
                menuStrip1.Items[sho].Visible = true;
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

            foreach(UserControl i in controls)
            {
                i.Hide();
                i.Location = new Point(0, 0);
                this.Controls.Add(i);
            }
            int[] buttomint={ 0,1};
            switchButtom(buttomint);
            int[] formint = { 0 };
            switchForm(0);
        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchForm(1);
        }

        private void 主页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchForm(0);
        }
    }
}
