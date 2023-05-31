using mynamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    public partial class returnForm : UserControl
    {
        MainForm f;
        String borrow_id;
        String book_id;

        public returnForm(MainForm f)
        {
            InitializeComponent();
            this.f = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //修改记录并把书库存+1
            Connectsql cons = new Connectsql();
            int d=cons.excutesql($"UPDATE borrow SET return_date = GETDATE() WHERE borrow_id={borrow_id};");
            cons.excutesql($"UPDATE book SET book_stock = book_stock + 1 WHERE book_id = {book_id};");
            cons.Close();
            MessageBox.Show("归还成功");
            get_data();


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
        private void returnForm_Load(object sender, EventArgs e)
        {
        }
        public void get_data()
        {
            Connectsql cons = new Connectsql();
            SqlDataReader reader;
            String sql = "";
            if (radioButton1.Checked)
            {
                sql = $"SELECT borrow_id, book.book_id,(SELECT book_name FROM book WHERE borrow.book_id=book.book_id ),borrow_date, due_date, return_date,CASE WHEN return_date IS NULL AND GETDATE()>=due_date THEN LEAST(DATEDIFF(day,due_date, GETDATE()),50)*0.02*book_price WHEN return_date IS NULL THEN 0 WHEN return_date>=due_date THEN LEAST(DATEDIFF(day,due_date, return_date),50)*0.02*book_price ELSE 0 END AS Default_fee FROM borrow LEFT JOIN book ON book.book_id=borrow.book_id WHERE reader_id={f.readerid}";
            }else if(radioButton2.Checked)
            {
                sql = $"SELECT borrow_id, book.book_id,(SELECT book_name FROM book WHERE borrow.book_id=book.book_id ),borrow_date, due_date, return_date,CASE WHEN return_date IS NULL AND GETDATE()>=due_date THEN LEAST(DATEDIFF(day,due_date, GETDATE()),50)*0.02*book_price WHEN return_date IS NULL THEN 0 WHEN return_date>=due_date THEN LEAST(DATEDIFF(day,due_date, return_date),50)*0.02*book_price ELSE 0 END AS Default_fee FROM borrow LEFT JOIN book ON book.book_id=borrow.book_id WHERE reader_id={f.readerid} AND return_date IS NULL";
            }else if (radioButton3.Checked)
            {
                sql = $"SELECT borrow_id, book.book_id,(SELECT book_name FROM book WHERE borrow.book_id=book.book_id ),borrow_date, due_date, return_date,CASE WHEN return_date IS NULL AND GETDATE()>=due_date THEN LEAST(DATEDIFF(day,due_date, GETDATE()),50)*0.02*book_price WHEN return_date IS NULL THEN 0 WHEN return_date>=due_date THEN LEAST(DATEDIFF(day,due_date, return_date),50)*0.02*book_price ELSE 0 END AS Default_fee FROM borrow LEFT JOIN book ON book.book_id=borrow.book_id WHERE reader_id={f.readerid} AND return_date IS NULL AND due_date<GETDATE();";
            }
            reader = cons.excutesql(sql, true);
            dataGridView1.Rows.Clear();
            while (reader.Read())
            {
                dataGridView1.Rows.Add(GetStringArray(reader));
            }
            reader.Close();
            //count1
            reader = cons.excutesql($"SELECT COUNT(*) FROM borrow WHERE return_date IS NULL AND reader_id={f.readerid}", true);
            reader.Read();
            int count1 = int.Parse(reader[0].ToString());
            reader.Close();
            //count2
            reader = cons.excutesql($"SELECT COUNT(*) FROM borrow WHERE return_date IS NULL AND reader_id={f.readerid} AND due_date<GETDATE();", true);
            reader.Read();
            int count2 = int.Parse(reader[0].ToString());
            reader.Close();
            //count3
            reader = cons.excutesql($"SELECT ISNULL(SUM(book_price*LEAST(0.02 * DATEDIFF(day, due_date, GETDATE()),1)),0) AS total_fee FROM borrow JOIN book ON borrow.book_id = book.book_id WHERE reader_id = {f.readerid} AND due_date < GETDATE() AND return_date IS NULL;", true);
            reader.Read();
            float count3 = float.Parse(reader[0].ToString());

            label1.Text = $"您一共借了{count1}本书，其中有{count2}本逾期，共要缴纳{count3}元费用";
            cons.Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
         
                get_data();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
    
                get_data();
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
      
                get_data();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string secondColumnValue = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            label2.Text = "当前所选书:" + secondColumnValue;
            borrow_id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            book_id= dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
