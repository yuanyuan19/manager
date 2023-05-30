using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mynamespace;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace manager
{
    public partial class WelcomeForm : UserControl
    {
        MainForm f;
        String book_id;
        public WelcomeForm(MainForm f)
        {
            InitializeComponent();
            this.f = f;
            
        }

        public String[] GetStringArray(SqlDataReader reader)
        {
            List<string> stringList = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                stringList.Add(reader[i].ToString());
            }
            return stringList.ToArray();
        }
        private void WelcomePage_Load(object sender, EventArgs e)
        {
            //dataGridView1
            Connectsql cons = new Connectsql();
            SqlDataReader reader=cons.excutesql("SELECT * FROM all_books", true);
            while (reader.Read())
            {
                dataGridView1.Rows.Add(GetStringArray(reader));
            }
            reader.Close();
            cons.Close();



            //comboBox1
            cons = new Connectsql();
            reader = cons.excutesql("SELECT category_name FROM category", true);
            comboBox1.Items.Add("全部");
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            comboBox1.SelectedIndex = 0;
            reader.Close();
            cons.Close();
        }
        private void get_data()
        {
            DateTime fromdate = dateTimePicker1.Value;
            DateTime todate = dateTimePicker2.Value;
            String maintext = textBox1.Text;
            String author = textBox2.Text;
            String publisher = textBox3.Text;
            String category = comboBox1.Text.ToString();
            dataGridView1.Rows.Clear();

            String sql = $"SELECT * FROM all_books WHERE book_id IN (SELECT book_id FROM book WHERE (book_name LIKE '%{maintext}%' AND book_author LIKE '%{author}%' AND book_publisher LIKE '%{publisher}%' AND book_publish_date >= '{fromdate}' AND book_publish_date <= '{todate}'));";
            if (category != "全部")
            {
                sql = $"SELECT * FROM all_books WHERE book_id IN (SELECT book_id FROM book WHERE (book_name LIKE '%{maintext}%' AND book_author LIKE '%{author}%' AND book_publisher LIKE '%{publisher}%' AND book_publish_date >= '{fromdate}' AND book_publish_date <= '{todate}' AND book_id in (SELECT book_id FROM book_category WHERE category_id in (SELECT category_id FROM category WHERE category_name='{category}'))));";
            }
            Connectsql cons = new Connectsql();
            SqlDataReader reader = cons.excutesql(sql, true);

            while (reader.Read())
            {
                dataGridView1.Rows.Add(GetStringArray(reader));
            }
            reader.Close();
            cons.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            get_data();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // 获取当前所选行的第二个属性的值
            string secondColumnValue = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            label7.Text="当前所选书:"+secondColumnValue;
            book_id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (f.username == "")
            {
                MessageBox.Show("请先登录");
                f.switchForm(1);
            }
            else
            {
                Connectsql cons = new Connectsql();
                SqlDataReader reader;
                String user_id = f.userid;
                //查询reader_id
                reader = cons.excutesql($"SELECT reader_id FROM reader WHERE user_id={user_id};", true);
                reader.Read();
                String reader_id = reader[0].ToString();
                reader.Close();
                //查询图书数量
                reader = cons.excutesql($"SELECT book_stock FROM book WHERE book_id={book_id};", true);
                reader.Read();
                int book_stock = int.Parse(reader[0].ToString());
                reader.Close();
                //查询借了几本书
                reader = cons.excutesql($"SELECT COUNT(*) as borrowed_count FROM borrow  WHERE reader_id = '{reader_id}' AND return_date IS NULL;", true);
                reader.Read();
                int borrowed_count = int.Parse(reader[0].ToString());
                reader.Close();
                if (book_stock==0)
                {
                    MessageBox.Show("图书库存不足");
                }
                else if ( borrowed_count> 3)
                {
                    MessageBox.Show("最大借阅3本书");
                }
                else
                {
                    MessageBox.Show("借书成功");
                    //插入记录
                    cons.excutesql($"INSERT INTO borrow (reader_id, book_id, borrow_date, due_date) VALUES ({reader_id}, {book_id}, GETDATE(), DATEADD(month, 1, GETDATE()));");
                    //减少图书库存
                    cons.excutesql($"UPDATE book SET book_stock = book_stock - 1 WHERE book_id = {book_id};");
                    //更新视图
                    get_data();
                }

            }
        }
    }
}
