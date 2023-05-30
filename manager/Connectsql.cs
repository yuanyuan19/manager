using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace mynamespace
{

    internal class Connectsql
    {
        String str = "Server=localhost,1433;Database=LibraryManagement;User Id=sa;Password=2019chance.;";
        SqlConnection con;
        public Connectsql()
        {
            con = new SqlConnection(str);
            con.Open();
        }
        public bool Close()
        {
            con.Close();
            return true;
        }
        
        public int excutesql(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected;
        }

        public SqlDataReader excutesql(string sql,bool usereader)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

    }
}
