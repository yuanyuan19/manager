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
        //存子组件
        UserControl[] controls = new UserControl[2];
        //直接赋值，无需实例化，用到this要在构造函数中赋值，因为this在实例化后才指向当前实例
        public String userid = "";
        public String username = "";
        public String premission = "";
        public MainForm()
        {
            InitializeComponent();
            //这里this就已经指向当前实例了
            controls[0] = new WelcomeForm(this);
            controls[1] = new loginForm(this);
        }

        public void switchForm(int showform)
        {
            //切换界面
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
            //添加子组件
            foreach(UserControl i in controls)
            {
                i.Hide();
                i.Location = new Point(0, 0);
                this.Controls.Add(i);
            }
            //初始化页面展示
            int[] buttomint={ 0,1 };
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

        public void set_label(String s)
        {
            label1.Text = s;
        }

        public void switchall(String s)
        {
            //根据权限修改页面内容
            if (s == "1")
            {
                
                int[] ints = { 0, 3, 4, 5 };
                switchButtom(ints);
                switchForm(3);
            }
            else
            {
                int[] ints = { 0, 2, 5 };
                switchButtom(ints);
                switchForm(0);
            }
        }
    }
}
