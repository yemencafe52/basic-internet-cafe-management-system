using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace YemenCafe
{
    internal class AccessDB
    {
        private OleDbConnection con;
        private OleDbCommand cmd;
        private OleDbDataReader dr;
        private string connection_string;
        internal AccessDB(string connection_string) 
        {
            this.connection_string = connection_string;
        }
        internal int ExcuteNonQuery(string cmdsql)
        {
            int res = 0;
            try
            {
                con = new OleDbConnection(this.connection_string);
                con.Open();
                cmd = new OleDbCommand(cmdsql, this.con);
                res = cmd.ExecuteNonQuery();
                con.Close();

            }
            catch
            { 
            }

            return res;
        }
        internal bool ExcuteQuery(string cmdsql)
        {
            bool res = false;
            try
            {
                con = new OleDbConnection(this.connection_string);
                con.Open();
                cmd = new OleDbCommand(cmdsql, this.con);
                this.dr = cmd.ExecuteReader();

                if(this.dr.HasRows)
                {
                    res = true;
                }
            
            }
            catch
            {

            }

            return res;

        }
        internal OleDbDataReader DataReader
        {
            get
            {
                return this.dr;
            }
        }
        internal void CloseConnection()
        {
            if (con != null)
            {
                try
                {
                    con.Close();
                    con = null;
                }
                catch
                {
                    con = null;
                }

            }

            GC.Collect();
        }
        ~AccessDB()
        {
            this.CloseConnection();
        }

    }
}
