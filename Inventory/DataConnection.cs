using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace Inventory
{
    public class DataConnection
    {
        public String ConnectionString;

        public DataConnection(String connectionString)
        {
            ConnectionString = connectionString;
        }

        public DataConnection(String server, String database, String user, String password)
        {
            ConnectionString = String.Concat("User Id=", user, ";Password=", password, ";Initial Catalog=", database, ";Data Source=", server, ";Provider=SQLOLEDB");
        }

        OleDbConnection Open()
        {
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public DataTable Select(String sql)
        {
            OleDbConnection connection = Open();
            OleDbCommand command = new OleDbCommand(sql, connection);
            command.CommandTimeout = 20800;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            DataSet set = new DataSet();
            adapter.SelectCommand = command;
            adapter.Fill(set);
            adapter.Dispose();
            command.Dispose();
            connection.Close();
            return set.Tables[0];
        }

        public int Execute(String sql)
        {
            OleDbConnection connection = Open();
            OleDbCommand command = new OleDbCommand(sql, connection);
            int ret = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return ret;
        }

        public int SelectScalarInt32(String sql)
        {
            return (int)Select(sql).Rows[0][0];
        }

        public bool TableExists(String table)
        {
            return SelectScalarInt32("select count(*) from sysobjects where Name = '" + table + "' and type = 'U'") == 1;
        }

        public List<String> ListFields(String table)
        {
            List<String> ret = new List<string>();
            foreach (DataColumn c in Select("select top 0 * from [" + table + "]").Columns)
                ret.Add(c.Caption);
            return ret;
        }

        public void AddField(String table, String field, Type type)
        {
            String typeInfo = "";
            switch (type.Name)
            {
                case "String":
                    typeInfo = "varchar(max)";
                    break;
                case "Boolean":
                    typeInfo = "bit";
                    break;
                case "Int32":
                    typeInfo = "int";
                    break;
                case "Double":
                    typeInfo = "float";
                    break;
                case "DateTime":
                    typeInfo = "datetime";
                    break;
            }

            Execute("alter table " + table + " add " + field + " " + typeInfo);
        }

        public DataRow SelectRow(String sql)
        {
            return Select(sql).Rows[0];
        }

        public String Filter(String value)
        {
            return value.Replace("'", "''");
        }

        public static DataConnection Current
        {
            get
            {
                return (DataConnection)CallContext.GetData("DataConnection");
            }

            set
            {
                CallContext.SetData("DataConnection", value);
            }
        }
    }
}