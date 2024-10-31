using GeneratSettings;
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
        public static string ApplicationName;
        public   struct stColumns
        {
            public string DataType;

            public string ColumnName;

            public bool AllowNull;
        }
        public static stColumns GetPK(string TableName)
        {
             SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = $@"SELECT 
    COLUMN_NAME AS ColumnName,
    DATA_TYPE AS DataType,
    IS_NULLABLE AS AllowNull
FROM 
    INFORMATION_SCHEMA.COLUMNS
WHERE 
    TABLE_NAME = '{TableName}'
    AND COLUMNPROPERTY(object_id(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1;
";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader Reader = sqlCommand.ExecuteReader();
            stColumns PrimaryKey = new stColumns();
            try
            {

                while (Reader.Read())
                {
                    PrimaryKey.ColumnName = Reader["ColumnName"].ToString();
                    PrimaryKey.DataType = Reader["DataType"].ToString();
                    PrimaryKey.AllowNull = (bool)Reader["AllowNull"];
                }
                Reader.Close();

            }
            catch (Exception ex) { }
            finally
            {
            sqlConnection.Close();

            }

            return PrimaryKey  ;
        }
        public static string GetAllColumnNames(string TableName, string Seperator = "", string Prefix = "\n\t\t\t")
        {
            string text = "";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = @"
    SELECT COLUMN_NAME 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = '" + TableName + "'";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string @string = sqlDataReader.GetString(0);
                text = text + Prefix + Seperator + @string + ",";
            }

            sqlDataReader.Close();
            text = text.Substring(0, text.Length - 1);
            sqlConnection.Close();
            return text;
        }
        public static string GetAllColumnNamesWithoutIdintities(string TableName, string Seperator = "", string Prefix = "\n\t\t\t")
        {
            string text = "";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = @"
    SELECT COLUMN_NAME 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = '" + TableName + @"'  
      AND COLUMNPROPERTY(OBJECT_ID(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 0  ";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string @string = sqlDataReader.GetString(0);
                text = text + Prefix + Seperator + @string + ",";
            }

            sqlDataReader.Close();
            text = text.Substring(0, text.Length - 1);
            sqlConnection.Close();
            return text;
        }
        public static string GetNameOfPrimaryKey(string tableName)
        {
            stColumns PK= GetPK(tableName);
            return PK.ColumnName;
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
            switch (DataType.ToLower())
            {
                case "int":
                    return "int";
                case "tinyint":
                    return "byte";
                case "smallint":
                    return "short";
                case "bigint":
                    return "long";
                case "bit":
                    return "bool";
                case "decimal":
                case "numeric":
                    return "decimal";
                case "float":
                    return "double";
                case "real":
                    return "float";
                case "money":
                case "smallmoney":
                    return "decimal";
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    return "string";
                case "datetime":
                case "smalldatetime":
                case "date":
                case "time":
                case "datetime2":
                case "datetimeoffset":
                    return "DateTime";
                case "binary":
                case "varbinary":
                case "image":
                    return "byte[]";
                case "uniqueidentifier":
                    return "Guid";
                case "xml":
                    return "XmlDocument";
                case "sql_variant":
                    return "object";
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
            switch (DataType.ToLower().Trim())
            {
                case "int":
                    return "Convert.ToInt32(";
                case "byte":
                case "tinyint":
                    return "Convert.ToByte(";
                case "string":
                case "nvarchar":
                case "varchar":
                case "char":
                    return "Convert.ToString(";
                case "datetime":
                    return "Convert.ToDateTime(";
                case "bool":
                case "bit":
                    return "Convert.ToBoolean(";
                case "short":
                case "smallint":
                    return "Convert.ToInt16(";

                case "bigint":
                    return "Convert.ToInt64(";
                case "float":
                    return "Convert.ToSingle(";
                case "real":
                    return "Convert.ToSingle(";
                case "double":
                    return "Convert.ToDouble(";
                case "decimal":
                case "money":
                case "smallmoney":
                    return "Convert.ToDecimal(";
                case "uniqueidentifier":
                    return "Guid.Parse(";
                case "binary":
                case "varbinary":
                    return "Convert.ToBase64String("; // لاستخدام تحويل بسيط للبيانات الثنائية كـ Base64
                default:
                    return "koko2";  // يمكنك تعديل هذه القيمة الافتراضية حسب الحاجة
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
            string cmdText = $@"SELECT DATA_TYPE, COLUMN_NAME, IS_NULLABLE 
                                               FROM INFORMATION_SCHEMA.COLUMNS
                                                WHERE TABLE_NAME = '" + TabelName + "' ";
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
        public static List<stColumns> GetDataTypeAndColumnNamesWithoutPK(string TabelName)
        {
            List<stColumns> list = new List<stColumns>();
            stColumns item = default(stColumns);
            string cmdText = @"SELECT DATA_TYPE, COLUMN_NAME, IS_NULLABLE 
                   FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = '" + TabelName + @"' 
                   AND COLUMN_NAME NOT IN(
                       SELECT COLUMN_NAME
                       FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                       WHERE TABLE_NAME = '" + TabelName + @"'
                       AND OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA +'.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                   )";
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
        private static string AddParameterWithDataType(string TableName, string Prefix = "")
        {
            string text = "";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = "\r\n                                                   SELECT COLUMN_NAME,DATA_TYPE \r\n                                                 FROM INFORMATION_SCHEMA.COLUMNS\r\n                                                 WHERE TABLE_NAME = '" + TableName + "' ";
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader Reader = sqlCommand.ExecuteReader();
            while (Reader.Read())
            {
                if (clsGeneralUtils.GetDataType(Reader.GetString(1)) != "string")
                    text = text + Prefix + clsGeneralUtils.GetDataType(Reader.GetString(1)) + "?  " + Reader.GetString(0) + ",";
                else
                    text = text + Prefix + clsGeneralUtils.GetDataType(Reader.GetString(1)) + " " + Reader.GetString(0) + ",";

            }

            Reader.Close();
            sqlConnection.Close();
            return text.Substring(0, text.Length - 1);
        }
        public static string GeneratMethodesForClass(string TableName)
        {
            string text = "";
            List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
            for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
            {
                if (dataTypeAndColumnNamesAndNullble[i].DataType != "string")
                    text = text + "        public   " + dataTypeAndColumnNamesAndNullble[i].DataType + " ?  " + dataTypeAndColumnNamesAndNullble[i].ColumnName + " { set; get; } \n";
                else
                    text = text + "        public   " + dataTypeAndColumnNamesAndNullble[i].DataType + "    " + dataTypeAndColumnNamesAndNullble[i].ColumnName + " { set; get; } \n";

            }
            return text;
        }
        public static string GeneratParameterConstructor(string TableName)
        {
            string text = "           cls" + TableName + "(" + AddParameterWithDataType(TableName) + "){";
            List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
            for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
            {
                text = text + "        this. " + dataTypeAndColumnNamesAndNullble[i].ColumnName + "=" + dataTypeAndColumnNamesAndNullble[i].ColumnName + ";\n";
            }

            text += "         Mode = enMode.Update;\r\n";
            return text + "}";
        }
        public static string GeneratEnumsForDTO()
        {
            return "        public enum enMode { AddNew = 0, Update = 1 };\n        public enMode Mode { set; get; }\n";
        }
        private static string AddAllParameterWithDataTypeConstructorDTO(string TabelName, string Prefix = "")
        {
            string text = "";
            SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
            sqlConnection.Open();
            string cmdText = @"SELECT COLUMN_NAME,DATA_TYPE, IS_NULLABLE 
                   FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = '" + TabelName +"'   ";
            
            SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                if (clsGeneralUtils.GetDataType(reader.GetString(1)) != "string")
                    text = text + Prefix + clsGeneralUtils.GetDataType(reader.GetString(1)) + "?  " + reader.GetString(0) + ",";
                else
                    text = text + Prefix + clsGeneralUtils.GetDataType(reader.GetString(1)) + " " + reader.GetString(0) + ",";
            }

            reader.Close();
            sqlConnection.Close();
            return text.Substring(0, text.Length - 1);
        }

        public static string GeneratMethodesForDTO(string TableName)
        {
            string text = "";
            List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
            for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
            {
                if (dataTypeAndColumnNamesAndNullble[i].DataType != "string")
                    text = text + "        public   " + dataTypeAndColumnNamesAndNullble[i].DataType + " ?  " + dataTypeAndColumnNamesAndNullble[i].ColumnName + " { set; get; } \n";
                else
                    text = text + "        public   " + dataTypeAndColumnNamesAndNullble[i].DataType + "    " + dataTypeAndColumnNamesAndNullble[i].ColumnName + " { set; get; } \n";

            }
            return text;
        }
        public static string GeneratParameterConstructoForDTO(string TableName)
        {
            string text = "          public cls" + TableName +"DTO"+ $"( {AddAllParameterWithDataTypeConstructorDTO(TableName)} ){{";
            List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
            for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
            {
                text = text + "        this. " + dataTypeAndColumnNamesAndNullble[i].ColumnName + $"= " +dataTypeAndColumnNamesAndNullble[i].ColumnName + ";\n";
            }

             return text + "}";
        }


    }
}
