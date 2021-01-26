using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ReportAssign
{
    public static class SqlHelper
    {
        private static readonly string constr = ConfigurationManager.ConnectionStrings["ReportDB"].ConnectionString;

        public static SqlDataReader ExcuteReader(CommandType sqltype, string sql, params SqlParameter[] pms)
        {
            SqlConnection conn = new SqlConnection(constr);
        
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                cmd.Parameters.AddRange(pms);
                cmd.CommandType = sqltype;

                return cmd.ExecuteReader();
            }
         
           
        }

        public static int ExecuteNonQuery(CommandType sqltype, string sql, params SqlParameter[] pms)
        {
            SqlConnection conn = new SqlConnection(constr);
           
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                cmd.Parameters.AddRange(pms);
                cmd.CommandType = sqltype;

                return cmd.ExecuteNonQuery();
            }
            
        }
    }
}

