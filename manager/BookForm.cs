using mynamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    public partial class BookForm : UserControl
    {
        MainForm f;
        int rolls;
        String book_id;
        public BookForm(MainForm f)
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
        public void get_data()
        {
                DateTime fromdate = dateTimePicker1.Value;
                DateTime todate = dateTimePicker2.Value;
                String maintext = textBox1.Text;
                String author = textBox2.Text;
                String publisher = textBox3.Text;
                String category = comboBox1.Text.ToString();
                dataGridView1.Rows.Clear();
                rolls = 0;

                String sql = $"SELECT * FROM book WHERE book_id IN (SELECT book_id FROM book WHERE (book_name LIKE '%{maintext}%' AND book_author LIKE '%{author}%' AND book_publisher LIKE '%{publisher}%' AND book_publish_date >= '{fromdate}' AND book_publish_date <= '{todate}'));";
                if (category != "全部")
                {
                    sql = $"SELECT * FROM book WHERE book_id IN (SELECT book_id FROM book WHERE (book_name LIKE '%{maintext}%' AND book_author LIKE '%{author}%' AND book_publisher LIKE '%{publisher}%' AND book_publish_date >= '{fromdate}' AND book_publish_date <= '{todate}' AND book_id in (SELECT book_id FROM book_category WHERE category_id in (SELECT category_id FROM category WHERE category_name='{category}'))));";
                }
                Connectsql cons = new Connectsql();
                SqlDataReader reader = cons.excutesql(sql, true);

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(GetStringArray(reader));
                    rolls++;
                }
                reader.Close();
                cons.Close();
        }

        private void BookForm_Load(object sender, EventArgs e)
        { 
            //comboBox1
            Connectsql cons = new Connectsql();
            SqlDataReader reader = cons.excutesql("SELECT category_name FROM category", true);
            comboBox1.Items.Add("全部");
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            comboBox1.SelectedIndex = 0;
            reader.Close();
            cons.Close();

            //dataGridView1
            get_data();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            get_data();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {   
            //表格被修改后触发，从第一条记录为0开始
            if (e.RowIndex >= 0&&e.RowIndex<rolls)
            {
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;
                String[] s = { "book_id", "book_name", "book_author", " book_publisher", "book_publish_date", "book_price", "book_stock"};
                // 根据行和列索引获取单元格的值
                DataGridViewCell cell = dataGridView1[columnIndex, rowIndex];
                //没填就用null代替插入
                String value;
                if (cell.Value == null)
                    value = "null";
                else
                    value = $"'{cell.Value.ToString()}'";
                Connectsql cons = new Connectsql();
                cons.excutesql($"UPDATE book SET {s[columnIndex]} = {value} WHERE book_id = {book_id};");
                cons.Close();
                MessageBox.Show("值已成功修改为"+value);
                get_data();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //index从第一行记录开始，为0
            if (dataGridView1.CurrentRow.Index < rolls)
            {
                book_id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                label1.Text = "当前book_id为"+book_id;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connectsql cons = new Connectsql();
            cons.excutesql($"DELETE FROM book WHERE book_id={book_id};");
            cons.Close();
            MessageBox.Show("编号为"+book_id + "的书被删除");
            get_data();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            String s = textBox4.Text;
            String[] s_split = s.Split('，');
            if (s_split.Length == 6)
            {
                String res = "(";
                for (int index = 0; index < s_split.Length; index++)
                {
                    if (s_split[index] == "null")
                        res += "null";
                    else
                        res += $"'{s_split[index]}'";
                    if (index != s_split.Length - 1)
                    {
                        res += ',';
                    }
                }
                res += ')';
                Connectsql cons = new Connectsql();
                cons.excutesql($"INSERT INTO book (book_name, book_author, book_publisher, book_publish_date, book_price, book_stock) VALUES {res}");
                cons.Close();
                MessageBox.Show(res + "已经成功添加");
                get_data();
            }
            else
            {
                MessageBox.Show("格式不对捏");
            }
        }
    }
}
