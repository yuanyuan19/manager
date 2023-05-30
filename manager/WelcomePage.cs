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
using mynamespace;
namespace manager
{
    public partial class WelcomePage : Form
    {
        String username = "";
        String premission = "";

        public WelcomePage()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime fromdate=dateTimePicker1.Value;
            DateTime todate = dateTimePicker2.Value;
            String maintext=textBox1.Text;
            String author=textBox2.Text;
            String publisher=textBox3.Text;
            String category=comboBox1.Text.ToString();
            dataGridView1.Rows.Clear();

            String sql = $"SELECT * FROM all_books WHERE book_id IN (SELECT book_id FROM book WHERE (book_name LIKE '%{maintext}%' AND book_author LIKE '%{author}%' AND book_publisher LIKE '%{publisher}%' AND book_publish_date >= '{fromdate}' AND book_publish_date <= '{todate}'));";
            if (category != "全部")
            {
                sql = $"SELECT * FROM all_books WHERE book_id IN (SELECT book_id FROM book WHERE (book_name LIKE '%{maintext}%' AND book_author LIKE '%{author}%' AND book_publisher LIKE '%{publisher}%' AND book_publish_date >= '{fromdate}' AND book_publish_date <= '{todate}' AND book_id in (SELECT book_id FROM book_category WHERE category_id in (SELECT category_id FROM category WHERE category_name='{category}'))));";
            }
            Connectsql cons = new Connectsql();
            SqlDataReader reader=cons.excutesql(sql,true);

            while (reader.Read())
            {
                dataGridView1.Rows.Add(GetStringArray(reader));
            }
            reader.Close();
            cons.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // 获取当前所选行的第二个属性的值
            string secondColumnValue = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            label7.Text="当前所选书:"+secondColumnValue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (username == "")
            {
                MessageBox.Show("请先登录");
            }
        }
    }
}
