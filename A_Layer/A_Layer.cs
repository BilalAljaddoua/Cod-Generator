using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GeneratSettings;
public class clsDataAccessLAyer
{
     private struct stColumns
    {
        public string DataType;

        public string TabelName;

        public bool AllowNull;
    }
    /// <summary>
    /// this function responsible to get the first coulmn in any table.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    private static string GetPrimaryKey(string tableName)
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
    private static string GetDataType(string DataType)
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
    private static string GetConvertType(string DataType)
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
            list[i] = dataTypeAndColumnNamesAndNullble[i].TabelName;
        }

        return list;
    }
    /// <summary>
    /// this function get three prooerties form database : Table name , Data type , Is nullble or not
    /// </summary>
    /// <param name="TabelName"></param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static List<stColumns> GetDataTypeAndColumnNamesAndNullble(string TabelName)
    {
        List<stColumns> list = new List<stColumns>();
        stColumns item = default(stColumns);
        string cmdText = "SELECT DATA_TYPE, COLUMN_NAME, IS_NULLABLE \r\n                                          FROM INFORMATION_SCHEMA.COLUMNS\r\n                                         WHERE TABLE_NAME = '" + TabelName + "' ";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
            string tabelName = sqlDataReader["COLUMN_NAME"].ToString();
            string text = sqlDataReader["IS_NULLABLE"].ToString();
            string dataType = GetDataType(sqlDataReader["DATA_TYPE"].ToString());
            item.TabelName = tabelName;
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
    private static string GetAllColumnsFromTable(string TableName, string Seperator = "")
    {
        string text = "";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        string cmdText = "\r\n                                                 SELECT COLUMN_NAME\r\n                                                 FROM INFORMATION_SCHEMA.COLUMNS\r\n                                                 WHERE TABLE_NAME = '" + TableName + "' ";
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
    private static string GetAllColumnsFromTableWithoutFirstOne(string TableName, string Seperator = "")
    {
        string text = "";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        string cmdText = "\r\n                                                 SELECT COLUMN_NAME\r\n                                                 FROM INFORMATION_SCHEMA.COLUMNS\r\n                                                 WHERE TABLE_NAME = '" + TableName + "' ";
        SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
            string @string = sqlDataReader.GetString(0);
            text = text + "\n\t"+ @string+"=" + Seperator + @string + ",";
        }

        sqlDataReader.Close();
        int num = text.IndexOf(",");
        text = text.Substring(num + 1, text.Length - num - 1);
        text = text.Substring(0, text.Length - 1);
        sqlConnection.Close();
        return text;
    }
    /// <summary>
    /// this function used to get cod that can add parameters to command 
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string AddAllParameterWithValue(string TableName)
    {
        string text = "";
        List<stColumns> ColumnInfo = GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 1; i < ColumnInfo.Count; i++)
        {
            if (ColumnInfo[i].AllowNull)
            {
                text = ((!(ColumnInfo[i].DataType == "string")) ?  
                    ((!(ColumnInfo[i].DataType == "DateTime")) ? 
                    (text + " if ( (" + ColumnInfo[i].TabelName + " !=0))\n   command.Parameters.AddWithValue(\"@" 
                    + ColumnInfo[i].TabelName +                    "\", " + ColumnInfo[i].TabelName + ");\n else\n     command.Parameters.AddWithValue(\"@" 
                    + ColumnInfo[i].TabelName + "\", System.DBNull.Value);\n") : (text + " if ((" + ColumnInfo[i].TabelName + " )!=null)\r\n                                                                                              command.Parameters.AddWithValue(\"@" + ColumnInfo[i].TabelName + "\", " + ColumnInfo[i].TabelName + ");\r\n                                                                                          else\r\n                                                                                              command.Parameters.AddWithValue(\"@" + ColumnInfo[i].TabelName + "\", System.DBNull.Value);\n")) : (text + " if (!string.IsNullOrEmpty(" + ColumnInfo[i].TabelName + " ))\r\n                                                                                              command.Parameters.AddWithValue(\"@" + ColumnInfo[i].TabelName + "\", " + ColumnInfo[i].TabelName + ");\r\n                                                                                          else\r\n                                                                                              command.Parameters.AddWithValue(\"@" + ColumnInfo[i].TabelName + "\", System.DBNull.Value);\n"));
            }
            else
            text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].TabelName + "\", " + ColumnInfo[i].TabelName + ");\n";
        }
         return text;
    }
    /// <summary>
    /// this function used to add the peimary key as parameter to command
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string AddOneParameterWithValue(string TableName)
    {
        return "\n command.Parameters.AddWithValue(\"@" + GetPrimaryKey(TableName) + "\", " + GetPrimaryKey(TableName) + ");";
    }
    /// <summary>
    /// this function used to add parameters with the data type of this parameter.
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <param name="Prefix"></param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string AddParameterWithDataType(string TableName, string Prefix = "")
    {
        string text = "";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        string cmdText = "\r\n                                                   SELECT COLUMN_NAME,DATA_TYPE \r\n                                                 FROM INFORMATION_SCHEMA.COLUMNS\r\n                                                 WHERE TABLE_NAME = '" + TableName + "' ";
        SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
            text = text + Prefix + GetDataType(sqlDataReader.GetString(1)) + " " + sqlDataReader.GetString(0) + ",";
        }

        sqlDataReader.Close();
        sqlConnection.Close();
        return text.Substring(0, text.Length - 1);
    }
    /// <summary>
    /// this function preper the query for INSERT Function
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string GetInsertQuery(string TableName)
    {
        string allColumnsFromTable = GetAllColumnsFromTable(TableName);
        int num = allColumnsFromTable.IndexOf(",");
        string allColumnsFromTable2 = GetAllColumnsFromTable(TableName, "@");
        int num2 = allColumnsFromTable.IndexOf(",");
        return "@\" INSERT INTO [dbo].[" + TableName + "]    (" + allColumnsFromTable.Substring(num + 1, allColumnsFromTable.Length - num - 1) + ")  \n\tVALUES (" + allColumnsFromTable2.Substring(num2 + 2, allColumnsFromTable2.Length - 2 - num2) + ") ;";
    }
    /// <summary>
    /// this function responsible to generat the cod that do inserting to database.
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved. </param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    public static string GeneratAddNewCod(string TableName)
    {
        string text = AddParameterWithDataType(TableName);
        int num = text.IndexOf(",");
        string text2 = "static public int Add" + TableName + "(" + text.Substring(num + 1, text.Length - num - 1) + ")\n" +
                       "{\n" +
                       "\tSqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);\n\n" +
                       "\tstring quere = " + GetInsertQuery(TableName) + "\";\n\n" +
                       "\tSqlCommand command = new SqlCommand(quere, connection);\n\n" +
                       AddAllParameterWithValue(TableName) + "\n\n" +
                       "\tint ID = 0;\n\n" +
                       "\ttry\n" +
                       "\t{\n" +
                       "\t\tconnection.Open();\n" +
                       "\t\tobject Result = command.ExecuteScalar();\n" +
                       "\t\tif (Result != null && int.TryParse(Result.ToString(), out int IDNumber))\n" +
                       "\t\t{\n" +
                       "\t\t\tID = IDNumber;\n" +
                       "\t\t}\n" +
                       "\t}\n" +
                       "\tcatch (Exception ex) { }\n" +
                       "\tfinally\n" +
                       "\t{\n" +
                       "\t\tconnection.Close();\n" +
                       "\t}\n\n" +
                       "\treturn ID;\n" +
                       "}\n";
        return text2;
    }
    /// <summary>
    /// this function responsible to grt inforamtion from database
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string GetInformationForRead(string TableName)
    {
        string text = "";
        List<stColumns> dataTypeAndColumnNamesAndNullble = GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            if (dataTypeAndColumnNamesAndNullble[i].AllowNull)
            {
                text = text + "      if (reader[\"" + dataTypeAndColumnNamesAndNullble[i].TabelName + "\"] != DBNull.Value)\r\n" +
                    " {\r\n  " + dataTypeAndColumnNamesAndNullble[i].TabelName +
                    " = (" + GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType)
                    + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].TabelName + "\"]));\r\n} ";
            }
            else
            text = text + dataTypeAndColumnNamesAndNullble[i].TabelName + " =  " + GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType) + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].TabelName + "\"]);\n\t\t";
        }

        return text.Substring(0, text.Length - 1);
    }
    /// <summary>
    /// this function preper the query for FIND Function
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string GetFindQuery(string TableName)
    {
        return "@\" SELECT * FROM [dbo].[" + TableName + "]  WHERE " + GetPrimaryKey(TableName) + " = @" + GetPrimaryKey(TableName) + " ;";
    }
    /// <summary>
    /// this function responible to generate cod that can search in database and get the result.
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    public static string GeneratFindCod(string TableName)
    {
        string text = "static public bool Find" + TableName + "(ref " + AddParameterWithDataType(TableName) + ")\n" +
                       "{\n" +
                       "\tSqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);\n\n" +
                       "\tstring quere = " + GetFindQuery(TableName) + "\";\n\n" +
                       "\tSqlCommand command = new SqlCommand(quere, connection);\n\n" +
                       AddOneParameterWithValue(TableName) + "\n\n" +
                       "\tbool IsRead = false;\n\n" +
                       "\ttry\n" +
                       "\t{\n" +
                       "\t\tconnection.Open();\n" +
                       "\t\tSqlDataReader reader = command.ExecuteReader();\n" +
                       "\t\tif (reader.Read())\n" +
                       "\t\t{\n" +
                       "\t\t\tIsRead = true;\n" +
                       "\t\t\t" + GetInformationForRead(TableName) + "\n" +
                       "\t\t}\n" +
                       "\t\treader.Close();\n" +
                       "\t}\n" +
                       "\tcatch (Exception ex) { }\n" +
                       "\tfinally\n" +
                       "\t{\n" +
                       "\t\tconnection.Close();\n" +
                       "\t}\n\n" +
                       "\treturn IsRead;\n" +
                       "}\n";
        return text;
    }
    /// <summary>
    /// this function preper the query for UPDATE Function
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string GetUpdateQuery(string TableName)
    {
        return "@\" UPDATE [dbo].[" + TableName + "]      \n\t SET (" + GetAllColumnsFromTableWithoutFirstOne(TableName, "@") + ") ; \n WHERE  " + GetPrimaryKey(TableName) + "=@" + GetPrimaryKey(TableName);
    }
    /// <summary>
    /// this function responible to update columns  in database and get the result.
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    public static string GeneratUpdateCod(string TableName)
    {
        string text = "static public bool Update" + TableName + "(" + AddParameterWithDataType(TableName) + ")\n" +
                       "{\n" +
                       "\tSqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);\n\n" +
                       "\tstring quere = " + GetUpdateQuery(TableName) + "\";\n\n" +
                       "\tSqlCommand command = new SqlCommand(quere, connection);\n\n" +
AddAllParameterWithValue(TableName) + "\n\n" +
                       "\tbool IsUpdate = false;\n\n" +
                       "\ttry\n" +
                       "\t{\n" +
                       "\t\tconnection.Open();\n" +
                       "\t\tint EffectedRow = command.ExecuteNonQuery();\n" +
                       "\t\tif (EffectedRow > 0)\n" +
                       "\t\t{\n" +
                       "\t\t\tIsUpdate = true;\n" +
                       "\t\t}\n" +
                       "\t}\n" +
                       "\tcatch (Exception ex) { }\n" +
                       "\tfinally\n" +
                       "\t{\n" +
                       "\t\tconnection.Close();\n" +
                       "\t}\n\n" +
                       "\treturn IsUpdate;\n" +
                       "}\n";
        return text;
    }
    /// <summary>
    /// this function responible to search in database and get the result.
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string GetDeleteQuery(string TableName)
    {
        return "@\" DELETE FROM [dbo].[" + TableName + "]      \t WHERE " + GetPrimaryKey(TableName) + "=@" + GetPrimaryKey(TableName);
    }
    /// <summary>
    /// this function preper cod that can delete row from tables in databases
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    public static string GeneratDeleteCod(string TableName)
    {
        string text = "static public bool Delete" + TableName + "(" + AddParameterWithDataType(TableName) + ")\n" +
                       "{\n" +
                       "\tSqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);\n\n" +
                       "\tstring quere = " + GetDeleteQuery(TableName) + "\";\n\n" +
                       "\tSqlCommand command = new SqlCommand(quere, connection);\n\n" +
                        AddOneParameterWithValue(TableName) + "\n\n" +
                       "\tbool IsDelete = false;\n\n" +
                       "\ttry\n" +
                       "\t{\n" +
                       "\t\tconnection.Open();\n" +
                       "\t\tint EffectedRow = command.ExecuteNonQuery();\n" +
                       "\t\tif (EffectedRow > 0)\n" +
                       "\t\t{\n" +
                       "\t\t\tIsDelete = true;\n" +
                       "\t\t}\n" +
                       "\t}\n" +
                       "\tcatch (Exception ex) { }\n" +
                       "\tfinally\n" +
                       "\t{\n" +
                       "\t\tconnection.Close();\n" +
                       "\t}\n\n" +
                       "\treturn IsDelete;\n" +
                       "}\n";
        return text;
    }
    /// <summary>
    /// this function generate Select Query  
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string GetSelectQuery(string TableName)
    {
        return "@\" SELECT * FROM [dbo].[" + TableName + "]   ";
    }
    /// <summary>
    /// this function can generat cod to get all results from database
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    private static string GeneratSelectCod(string TableName)
    {
        string text = "static public DataTable GetAll" + TableName + "()\r\n                   \r\n     {  SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);\r\n\r\n                                      string quere =  " + GetSelectQuery(TableName) + "\";\n";
        return text + "SqlCommand command = new SqlCommand(quere, connection);\r\n\r\n          DataTable dt=new DataTable();\r\n\r\n          try\r\n          {\r\n              connection.Open();\r\n                   SqlDataReader reader=  command.ExecuteReader();\r\n               if(reader.HasRows)\r\n           {\r\n                      dt.Load(reader);\r\n            }\r\n                             reader.Close();\r\n\r\n          }\r\n          catch (Exception ex) { }\r\n          finally\r\n          {\r\n              connection.Close();\r\n          }\r\n\r\n          return dt;\r\n\r\n\r\n      }    \r\n\r\n";
    }
    /// <summary>
    /// this function call the all the main functions to generat the whole class.
    /// </summary>
    /// <param name="TableName">The name of the table from which column data is retrieved.</param>
    /// <returns>A string that represents the parameters and their data types .</returns>
    public static string GeneratDataAccess(string TableName)
    {
        string text = "";
        text += GeneratSelectCod(TableName);
        text += GeneratFindCod(TableName);
        text += GeneratAddNewCod(TableName);
        text += GeneratUpdateCod(TableName);
        text += GeneratDeleteCod(TableName);
        return "using System;\r\nusing System.Collections.Generic;\r\nusing System.Data;\r\nusing System.Data.SqlClient;\r\nusing System.IO;\r\nusing System.Linq;\r\nusing System.Runtime.InteropServices;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nusing GeneratSettings;\r\n\r\n\r\nnamespace DataAccessLayer\r\n{\r\n    public class  cls" + TableName + "Data\r\n    {\r\n" + text + "\r\n    }\r\n}\r\n";
    }
}
 