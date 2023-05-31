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
    public partial class BorrowForm : UserControl
    {
        MainForm f;
        int rolls;
        String borrow_id;
        public BorrowForm(MainForm f)
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
            dataGridView1.Rows.Clear();
            rolls = 0;
            String sql = "SELECT * FROM borrow;";
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
        private void BorrowForm_Load(object sender, EventArgs e)
        {
            get_data();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //index从第一行记录开始，为0
            if (dataGridView1.CurrentRow.Index < rolls)
            {
                borrow_id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                label1.Text = "当前借阅编号为" + borrow_id;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //表格被修改后触发，从第一条记录为0开始
            if (e.RowIndex >= 0 && e.RowIndex < rolls)
            {
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;
                String[] s = { "borrow_id","reader_id", "book_id", "borrow_date", "due_date", "return_date"};
                // 根据行和列索引获取单元格的值
                DataGridViewCell cell = dataGridView1[columnIndex, rowIndex];
                //没填就用null代替插入
                String value;
                if (cell.Value == null)
                    value = "null";
                else
                    value = $"'{cell.Value.ToString()}'";
                Connectsql cons = new Connectsql();
                cons.excutesql($"UPDATE borrow SET {s[columnIndex]} = {value} WHERE borrow_id = {borrow_id};");
                cons.Close();
                MessageBox.Show("值已成功修改为" + value);
                get_data();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connectsql cons = new Connectsql();
            cons.excutesql($"DELETE FROM borrow WHERE borrow_id={borrow_id};");
            cons.Close();
            MessageBox.Show("编号为" + borrow_id + "的书被删除");
            get_data();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String s = textBox1.Text;
            String[] s_split = s.Split('，');
            if (s_split.Length == 5)
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
                MessageBox.Show(res + "已经成功添加");
                Connectsql cons = new Connectsql();
                cons.excutesql($"INSERT INTO borrow (reader_id, book_id, borrow_date, due_date, return_date) VALUES {res}");
                cons.Close();

                get_data();
            }
            else
            {
                MessageBox.Show("格式不对捏");
            }
            
        }
    }
}
