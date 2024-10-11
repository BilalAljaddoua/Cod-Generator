﻿using GeneratSettings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Layer
{
    public class clsGeneralUtils
    {
        public   struct stColumns
        {
            public string DataType;

            public string ColumnName;

            public bool AllowNull;
        }
 
        public static string GetPrimaryKey(string tableName)
        {
            string allColumnsFromTable = GetAllColumnsFromTable(tableName);
            int length = allColumnsFromTable.IndexOf(",");
            return allColumnsFromTable.Substring(0, length).Trim();
        }
        /// <summary>
        /// this function responsible to get all names for databases .
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllDataBasses()
        {
            string connectionString = "Data Source=BILAL;Initial Catalog=master;Integrated Security=True;";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            SqlCommand sqlCommand = new SqlCommand("SELECT name FROM sys.databases", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string @string = sqlDataReader.GetString(0);
                DataRow dataRow = dataTable.NewRow();
                dataRow["Name"] = @string;
                dataTable.Rows.Add(dataRow);
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            return dataTable;
        }
        /// <summary>
        /// this founction responible to set the fit connection string that user can change it
        /// </summary>
        /// <param name="connectionString"></param>
        public static void SetStringConnection(string connectionString)
        {
            clsSettingsClass.ConnectionString = connectionString;
        }
        /// <summary>
        /// this function responsible to get all tables from database.
        /// </summary>
        /// <param name="DataBase"></param>
        /// <returns></returns>
        public static DataTable GetAlltables(string DataBase)
        {
            DataTable dataTable = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            string cmdText = "use " + DataBase + "; \r\nSELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            try
            {
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    dataTable.Load(sqlDataReader);
                }

                sqlDataReader.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                sqlConnection.Close();
            }

            return dataTable;
        }
        /// <summary>
        /// this function get all datatypes from database .
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public static string GetDataType(string DataType)
        {
            switch (DataType)
            {
                case "int":
                    return "int";
                case "tinyint":
                    return "short";
                case "nvarchar":
                    return "string";
                case "varchar":
                    return "string";
                case "datetime":
                case "DateTime":
                case "smalldatetime":
                    return "DateTime";
                case "smallmoney":
                    return "float";
                case "bit":
                    return "bool";
                default:
                    return "unknown";  // يمكنك تغيير "unknown" إلى أي قيمة افتراضية تفضلها
            }
        }

        /// <summary>
        /// this function convert datatype for database to C# 
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns>A string that represents the parameters and their data types .</returns>
        public static string GetConvertType(string DataType)
        {
            switch (DataType)
            {
                case "int":
                    return "Convert.ToInt32(";
                case "string":
                    return "Convert.ToString(";
                case "nvarchar":
                    return "Convert.ToString(";
                case "varchar":
                    return "Convert.ToString(";
                case "DateTime":
                    return "Convert.ToDateTime(";
                case "bool":
                    return "Convert.ToBoolean(";
                case "short":
                    return "Convert.ToInt16(";
                case "tinyint":  // 
                    return "Convert.ToInt16(";
                case "float":
                    return "Convert.ToSingle(";
                default:
                    return "koko2";  // يمكنك تغيير هذه القيمة الافتراضية إذا أردت
            }
        }
        /// <summary>
        /// this function will get the columns names from the tabel 
        /// </summary>
        /// <param name="TabelName"></param>
        /// <returns>A string that represents the parameters and their data types .</returns>
        public static List<string> GetAllColumnsFromTabel(string TabelName)
        {
            List<stColumns> dataTypeAndColumnNamesAndNullble = GetDataTypeAndColumnNamesAndNullble(TabelName);
            List<string> list = new List<string>();
            for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
            {
                list[i] = dataTypeAndColumnNamesAndNullble[i].ColumnName;
            }

            return list;
        }
        /// <summary>
        /// this function get three prooerties form database : Table name , Data type , Is nullble or not
        /// </summary>
        /// <param name="TabelName"></param>
        /// <returns>A string that represents the parameters and their data types .</returns>
        public static List<stColumns> GetDataTypeAndColumnNamesAndNullble(string TabelName)
        {
            List<stColumns> list = new List<stColumns>();
            stColumns item = default(stColumns);
            string cmdText = "SELECT DATA_TYPE, COLUMN_NAME, IS_NULLABLE \r\n           FROM INFORMATION_SCHEMA.COLUMNS\r\n                                         WHERE TABLE_NAME = '" + TabelName + "' ";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string tabelName = sqlDataReader["COLUMN_NAME"].ToString();
                string text = sqlDataReader["IS_NULLABLE"].ToString();
                string dataType = GetDataType(sqlDataReader["DATA_TYPE"].ToString());
                item.ColumnName = tabelName;
                item.AllowNull = text == "YES";
                item.DataType = dataType;
                list.Add(item);
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            return list;
        }
        /// <summary>
        /// this function will get the columns names from the tabel and it can add some Seperators
        /// </summary>
        /// <param name="TabelName"></param>
        /// <returns>A string that represents the parameters and their data types .</returns>
        public static List<stColumns> GetDataTypeAndColumnNamesForSP(string TabelName)
        {
            List<stColumns> list = new List<stColumns>();
            stColumns item = default(stColumns);
            string cmdText = "SELECT DATA_TYPE, COLUMN_NAME, IS_NULLABLE \r\n           FROM INFORMATION_SCHEMA.COLUMNS\r\n                                         WHERE TABLE_NAME = '" + TabelName + "' ";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string tabelName = sqlDataReader["COLUMN_NAME"].ToString();
                string text = sqlDataReader["IS_NULLABLE"].ToString();
                string dataType = (sqlDataReader["DATA_TYPE"].ToString());
                item.ColumnName = tabelName;
                item.AllowNull = text == "YES";
                item.DataType = dataType;
                list.Add(item);
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            return list;
        }

        public static string GetAllColumnsFromTable(string TableName, string Seperator = "")
        {
            string text = "";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = "\r\n SELECT COLUMN_NAME\r\n    FROM INFORMATION_SCHEMA.COLUMNS\r\n    WHERE TABLE_NAME = '" + TableName + "' ";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string @string = sqlDataReader.GetString(0);
                text = text + "\n\t\t\t" + Seperator + @string + ",";
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            return text.Substring(0, text.Length - 1);
        }
        /// <summary>
        /// this function get all columns from data base without the first one becaous we don't need it for some ooperations.
        /// </summary>
        /// <param name="TableName">The name of the table from which column data is retrieved.</param>
        /// <param name="Seperator"></param>
        /// <returns>A string that represents the parameters and their data types .</returns>
        public static string GetAllColumnsWithoutFirstOneForInsert(string TableName, string Seperator = "")
        {
            string text = "";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = "\r\n      SELECT COLUMN_NAME\r\n       FROM INFORMATION_SCHEMA.COLUMNS\r\n        WHERE TABLE_NAME = '" + TableName + "' ";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string @string = sqlDataReader.GetString(0);
                text = text + "\n\t"  + Seperator + @string + ",";
            }

            sqlDataReader.Close();
            int num = text.IndexOf(",");
            text = text.Substring(num + 1, text.Length - num - 1);
            text = text.Substring(0, text.Length - 1);
            sqlConnection.Close();
            return text;
        }
        public static string GetAllColumnsWithoutFirstOneForUpdate(string TableName, string Seperator = "")
        {
            string text = "";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = "\r\n      SELECT COLUMN_NAME\r\n       FROM INFORMATION_SCHEMA.COLUMNS\r\n        WHERE TABLE_NAME = '" + TableName + "' ";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string @string = sqlDataReader.GetString(0);
                text = text + "\n\t" + @string + "=" + Seperator + @string + ",";
            }

            sqlDataReader.Close();
            int num = text.IndexOf(",");
            text = text.Substring(num + 1, text.Length - num - 1);
            text = text.Substring(0, text.Length - 1);
            sqlConnection.Close();
            return text;
        }




    }
}