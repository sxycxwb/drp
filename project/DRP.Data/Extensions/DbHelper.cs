/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DRP.Data.Extensions
{
    [Serializable]
    public enum SqlSourceType
    {
        Oracle,
        MSSql,
        MySql,
        SQLite
    }

    public class DbHelper
    {
        private static string connstring = ConfigurationManager.ConnectionStrings["DRPDbContext"].ConnectionString;
        private static string providerName = ConfigurationManager.ConnectionStrings["DRPDbContext"].ProviderName;

        public static int ExecuteSqlCommand(string cmdText)
        {
            using (DbConnection conn = new SqlConnection(connstring))
            {
                DbCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                return cmd.ExecuteNonQuery();
            }
        }
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (isOpenTrans != null)
                cmd.Transaction = isOpenTrans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }

        public static DbParameter GetDbParameter(string key, object value)
        {
            switch (providerName)
            {
                case "MySql.Data.MySqlClient":
                    return new MySqlParameter(key, value);
                    break;
                case "System.Data.SqlClient":
                    return new SqlParameter(key, value);
                    break;
            }
            return null;
        }

        public static DbParameter GetDbParameter(string key, object value, ParameterDirection direction)
        {
            var parameter = GetDbParameter(key, value);
            parameter.Direction = direction;
            return parameter;
        }
    }
}
