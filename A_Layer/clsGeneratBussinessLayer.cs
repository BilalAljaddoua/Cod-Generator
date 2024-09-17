 

using GeneratSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
 
public class clsGeneratBussinessLayer
{
    private struct stColumns
    {
        public string DataType;

        public string TabelName;

        public bool AllowNull;
    }

    private static string GetPrimaryKey(string tableName)
    {
        string allColumnsFromTable = GetAllColumnsFromTable(tableName);
        int length = allColumnsFromTable.IndexOf(",");
        return allColumnsFromTable.Substring(0, length).Trim();
    }

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

    public static void SetStringConnection(string connectionString)
    {
        clsSettingsClass.ConnectionString = connectionString;
    }

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

    private static string SetDefaultValues(string TableName)
    {
        List<stColumns> dataTypeAndColumnNamesAndNullble = GetDataTypeAndColumnNamesAndNullble(TableName);
        string text = "";
        for (int i = 1; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            text = text + dataTypeAndColumnNamesAndNullble[i].DataType + " " + dataTypeAndColumnNamesAndNullble[i].TabelName;
            text = ((!(dataTypeAndColumnNamesAndNullble[i].DataType == "string")) ? ((!(dataTypeAndColumnNamesAndNullble[i].DataType == "bool")) ? ((!(dataTypeAndColumnNamesAndNullble[i].DataType == "DateTime")) ? (text + "= -1 ; ") : (text + "=DateTime.Now; ")) : (text + "=false; ")) : (text + "=\"\" ; "));
        }

        return text + "\n";
    }

    private static string GetAllColumnsFromTable(string TableName, string Seperator = "", string Prefix = "\n\t\t\t")
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
            text = text + Prefix + Seperator + @string + ",";
        }

        sqlDataReader.Close();
        sqlConnection.Close();
        return text.Substring(0, text.Length - 1);
    }

    private static string GetAllColumnsFromTable_064BWithoutFirstOne(string TableName, string Seperator = "", string Prefix = "\n\t\t\t")
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
            text = text + Prefix + Seperator + @string + ",";
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
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
            text = text + Prefix + GetDataType(sqlDataReader.GetString(1)) + " " + sqlDataReader.GetString(0) + ",";
        }

        sqlDataReader.Close();
        sqlConnection.Close();
        return text.Substring(0, text.Length - 1);
    }

    public static string GeneratEnums()
    {
        return "        public enum enMode { AddNew = 0, Update = 1 };\n        public enMode Mode = enMode.AddNew;";
    }

    private static string GeneratMethodes(string TableName)
    {
        string text = "";
        List<stColumns> dataTypeAndColumnNamesAndNullble = GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            text = text + "        public   " + dataTypeAndColumnNamesAndNullble[i].DataType + "  " + dataTypeAndColumnNamesAndNullble[i].TabelName + " { set; get; } \n";
        }

        return text;
    }

    private static string GeneratDefalutConstructor(string TableName)
    {
        string text = "           cls" + TableName + "(){";
        List<stColumns> dataTypeAndColumnNamesAndNullble = GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            text = ((!(dataTypeAndColumnNamesAndNullble[i].DataType == "string")) ? ((!(dataTypeAndColumnNamesAndNullble[i].DataType == "DateTime")) ?
                                                                                                                                                    ((!(dataTypeAndColumnNamesAndNullble[i].DataType == "bool")) ?
                                                                                                                                                     (text + "        this.  " + dataTypeAndColumnNamesAndNullble[i].TabelName + " =-1 ;\n") 
                                                                                                                                                    : (text + "        this.  " + dataTypeAndColumnNamesAndNullble[i].TabelName + " =false ;\n"))
                                                                                                                                                  : (text + "        this.  " + dataTypeAndColumnNamesAndNullble[i].TabelName + " =DateTime.Now ;\n"))
                                                                                                                                                  : (text + "        this.  " + dataTypeAndColumnNamesAndNullble[i].TabelName + " =\"\" ;\n"));
        }

        text += "         Mode = enMode.AddNew;\n";
        return text + "}";
    }

    private static string GeneratParameterConstructor(string TableName)
    {
        string text = "           cls" + TableName + "(" + AddParameterWithDataType(TableName) + "){";
        List<stColumns> dataTypeAndColumnNamesAndNullble = GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            text = text + "        this. " + dataTypeAndColumnNamesAndNullble[i].TabelName + "=" + dataTypeAndColumnNamesAndNullble[i].TabelName + ";\n";
        }

        text += "         Mode = enMode.Update;\r\n";
        return text + "}";
    }

    private static string GeneratAdd(string TableName)
    {
        return "        private bool _Add" + TableName + "()\r\n        {\r\n \r\n            this." + GetPrimaryKey(TableName) + " = cls" + TableName + "Data.Add" + TableName + "(" + GetAllColumnsFromTable_064BWithoutFirstOne(TableName, "", " ") + ");\r\n              \r\n\r\n            return (this." + GetPrimaryKey(TableName) + " != -1);\r\n        }";
    }

    private static string GeneratGetAll(string TableName)
    {
        return "        static public DataTable GetAll" + TableName + "()\r\n        {\r\n                return cls" + TableName + "Data.GetAll" + TableName + "();\r\n         }";
    }

    private static string GeneratUpdate(string TableName)
    {
        return "        private bool _Update" + TableName + "()\r\n        {\r\n \r\n            bool IsSuccess= cls" + TableName + "Data.Update" + TableName + "(" + GetAllColumnsFromTable_064BWithoutFirstOne(TableName, "", " ") + ");\r\n              \r\n\r\n            return IsSuccess;\r\n        }";
    }

    private static string GeneratFind(string TableName)
    {
        return "         public cls" + TableName + " Find" + TableName + "(int " + GetPrimaryKey(TableName) + ")\r\n           {\r\n                 " +
            SetDefaultValues(TableName) + "\r\n               if(cls" + TableName + "Data.Find" + TableName + "(" + 
            GetAllColumnsFromTable(TableName, "", " ref ") + "))\r\n               {\r\n                   return new cls" + TableName 
            + "(" + GetAllColumnsFromTable(TableName, "", " ") + ");\r\n               }\r\n             return null;\r\n    }";
    }

    private static string GeneratDelete(string TableName)
    {
        return "           static bool Delete" + TableName + "(int " + GetPrimaryKey(TableName) + ")\r\n        {\r\n              return cls" + TableName + "Data.Delete" + TableName + "(" + GetPrimaryKey(TableName) + ");\r\n        }";
    }

    private static string GeneratSave(string TableName)
    {
        return "        public bool Save()\r\n        {\r\n          \r\n            switch (Mode)\r\n            {\r\n                case enMode.AddNew:\r\n                    if (_Add" + TableName + "())\r\n                    {\r\n\r\n                        Mode = enMode.Update;\r\n                        return true;\r\n                    }\r\n                    else\r\n                    {\r\n                        return false;\r\n                    }\r\n\r\n                case enMode.Update:\r                    return _Update" + TableName + "();\r\n\r\n            }\r\n\r\n            return false;\r\n         \r\n\r\n        }";
    }

    public static string GeneratBadyCod(string TableName)
    {
        string text = "";
        text = text + "\n\n" + GeneratEnums();
        text = text + "\n\n" + GeneratMethodes(TableName);
        text = text + "\n\n" + GeneratDefalutConstructor(TableName);
        text = text + "\n\n" + GeneratParameterConstructor(TableName);
        text = text + "\n\n" + GeneratAdd(TableName);
        text = text + "\n\n" + GeneratGetAll(TableName);
        text = text + "\n\n" + GeneratUpdate(TableName);
        text = text + "\n\n" + GeneratFind(TableName);
        text = text + "\n\n" + GeneratDelete(TableName);
        return text + "\n\n" + GeneratSave(TableName);
    }

    public static string GeneratDataBussiness(string TableName)
    {
        return "using System;\r\nusing System.Collections.Generic;\r\nusing System.Data;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nusing DataAccessLayer;\r\nnamespace Bussiness_Layer\r\n{\r\n    public class cls" + TableName + "\r\n    {\r\n" + GeneratBadyCod(TableName) + "\r\n    } \r\n    }\r\n";
    }
}
 