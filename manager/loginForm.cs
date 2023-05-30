using mynamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace manager
{
    public partial class loginForm : UserControl
    {
        MainForm f;
        public loginForm(MainForm f)
        {
            InitializeComponent();
            this.f = f;
        }

        private void loginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            String password = textBox2.Text;
            Connectsql cons = new Connectsql();
            SqlDataReader reader = cons.excutesql($"SELECT user_name,user_permission FROM [user] WHERE user_name='{username}' AND user_password='{password}';", true);
            if (reader.Read())
            {
                login(reader[0].ToString(), reader[1].ToString());
                MessageBox.Show("登录成功");
                f.switchForm(0);
            }
            else
            {
                MessageBox.Show("登录失败");
            }
            reader.Close();
            cons.Close();
        }
        
        private void login(String username,String premission)
        {
            if (premission == "0")
                f.premission = "普通用户";
            else
                f.premission = "管理员";
            f.username = username;
            f.set_label("当前在线" + premission + ":" + username);
        }
    }
}
