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
        public UserControl[] controls = new UserControl[6];
        //直接赋值，无需实例化，用到this要在构造函数中赋值，因为this在实例化后才指向当前实例
        public String userid = "";
        public String username = "";
        public String premission = "";
        public String readerid = "";
        public MainForm()
        {
            InitializeComponent();
            //这里this就已经指向当前实例了
            controls[0] = new WelcomeForm(this);
            controls[1] = new loginForm(this);
            controls[2] = new returnForm(this);
            controls[3] = new BookForm(this);
            controls[4] = new UserForm(this);
            controls[5] = new BorrowForm(this);
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

        public void set_label(String s)
        {
            label1.Text = s;
        }

        public void switchall(String s)
        {
            //根据权限修改页面内容
            if (s == "1")
            {
                
                int[] ints = {3, 4, 5 ,6};
                switchButtom(ints);
                switchForm(3);
            }
            else
            {
                int[] ints = { 0, 2, 6};
                switchButtom(ints);
                switchForm(0);
            }
        }

        private void 主页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //主页无需重新获取数据
            switchForm(0);
        }
        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchForm(1);
        }
        private void 还书ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //不能让其他用户看见你的视图，切换到这个视图时重新获取下数据
            ((returnForm)controls[2]).get_data();
            switchForm(2);
        }

        private void 登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] buttomint = { 0, 1 };
            switchButtom(buttomint);
            int[] formint = { 0 };
            switchForm(1);

            userid =username =premission = readerid = "";
            set_label("未登录");

    }

        private void 管理书籍ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((BookForm)controls[3]).get_data();
            switchForm(3);
        }

        private void 管理用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchForm(4);
        }

        private void 管理借阅记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchForm(5);
        }

    }
}
