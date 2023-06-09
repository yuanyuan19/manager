﻿using mynamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
            Connectsql cons = new Connectsql();
            SqlDataReader reader;
            String username = textBox1.Text;
            String password = textBox2.Text;
            reader = cons.excutesql($"SELECT user_name,user_permission,user_id FROM [user] WHERE user_name='{username}' AND user_password='{password}';", true);
            if (reader.Read())
            {
                login(reader[0].ToString(), reader[1].ToString(),reader[2].ToString());
            }
            else
            {
                MessageBox.Show("登录失败");
            }
            reader.Close();
            cons.Close();
        }
        
        private void login(String username,String premission,String userid)
        {
            //设置label
            if (premission == "0")
            {
                f.set_label("当前在线用户：" + username);
                //查询reader_id
                Connectsql cons = new Connectsql();
                SqlDataReader reader = cons.excutesql($"SELECT reader_id FROM reader WHERE user_id={userid};", true);
                reader.Read();
                String reader_id = reader[0].ToString();
                reader.Close();
                f.readerid = reader_id;
            }
            else
                f.set_label("当前在线管理员：" + username);
            //设置主form属性
            f.username = username;
            f.premission = premission;
            f.userid = userid;
            //修改界面内容
            f.switchall(premission);
        }
    }
}
