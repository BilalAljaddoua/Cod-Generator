

using A_Layer;
using GeneratSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class clsGeneratBussinessLayer
{

    private static string SetDefaultValues(string TableName)
    {
        List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
        string text = "";
        for (int i = 1; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            if (dataTypeAndColumnNamesAndNullble[i].DataType!="string")
            text = text + dataTypeAndColumnNamesAndNullble[i].DataType + "? " + dataTypeAndColumnNamesAndNullble[i].ColumnName+"= null ;\t ";
           else
            {
                text = text + dataTypeAndColumnNamesAndNullble[i].DataType +" " + dataTypeAndColumnNamesAndNullble[i].ColumnName + "= null ;\t";

            }
        }
        
         return text + "\n";
    }
    private static string GetAllColumnsFromTable(string TableName, string Seperator = "", string Prefix = "\n\t\t\t")
    {
        string text = "";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        string cmdText = "\r\n  SELECT COLUMN_NAME\r\n  FROM INFORMATION_SCHEMA.COLUMNS\r\n  WHERE TABLE_NAME = '" + TableName + "' ";
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
        sqlConnection.Close();  
        text = text.Substring(0, text.Length - 1);

        return text;
    }
    public static string GeneratEnums()
    {
        return "        public enum enMode { AddNew = 0, Update = 1 };\n        public enMode Mode = enMode.AddNew;\n";
    }

    private static string GeneratDefalutConstructor(string TableName)
    {
        string text = "    public       cls" + TableName + "(){";
        List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            text += "        this." + dataTypeAndColumnNamesAndNullble[i].ColumnName + "=null ; \n"; 
        }
        text += "         Mode = enMode.AddNew;\n";
        return text + "}";
    }
    private static string GeneratAdd(string TableName)
    {
        return "        private bool _Add" + TableName + "()\r\n        {    this." + clsGeneralUtils.GetNameOfPrimaryKey(TableName) + " = cls" + TableName + "Data.AddTo" + TableName + "Table(" + GetAllColumnsFromTable_064BWithoutFirstOne(TableName, "", " ") + ");\r\n            return (this." + clsGeneralUtils.GetNameOfPrimaryKey(TableName) + " != -1);\r\n        }";
    }
    private static string GeneratGetAll(string TableName)
    {
        return "        static public DataTable GetAll" + TableName + "()\r\n        {\r\n                return cls" + TableName + "Data.GetAll" + TableName + "();\r\n         }";
    }
    private static string GeneratUpdate(string TableName)
    {
        return "        private bool _Update" + TableName + "()\r\n        {            bool IsSuccess= cls" + TableName + "Data.Update" + TableName + "Table(" + clsGeneralUtils.GetAllColumnNames(TableName, "", " ") + ");\r\n            return IsSuccess;\r\n        }";
    }
    private static string GeneratFind(string TableName)
    {
        return "       static  public cls" + TableName + " Find" + TableName + "(int? " + clsGeneralUtils.GetNameOfPrimaryKey(TableName) + ")\r\n           {\r\n                 " +
            SetDefaultValues(TableName) + "\r\n               if(cls" + TableName + "Data.Find" + TableName + "(" +
            GetAllColumnsFromTable(TableName, "", " ref ") + "))\r\n               {\r\n                   return new cls" + TableName
            + "(" + GetAllColumnsFromTable(TableName, "", " ") + ");\r\n               }\r\n             return null;\r\n    }";
    }
    private static string GeneratDelete(string TableName)
    {
        return "           static bool Delete" + TableName + "(int " + clsGeneralUtils.GetNameOfPrimaryKey(TableName) + ")\r\n        {\r\n              return cls" + TableName + "Data.Delete" + TableName + "(" + clsGeneralUtils.GetNameOfPrimaryKey(TableName) + ");\r\n        }";
    }
    private static string GeneratSave(string TableName)
    {
        return "        public bool Save()\r\n        {\r\n                   switch (Mode)\r\n            {              case enMode.AddNew:\r\n                    if (_Add" + TableName + "())\r\n                    {\r\n                      Mode = enMode.Update;\r\n                        return true;\r\n                    }\r\n                    else\r\n                    {\r\n                        return false;\r\n                    }\r\n                case enMode.Update:\r                    return _Update" + TableName + "();\r\n\r\n            }\r\n\r\n            return false;\r\n                }";
    }
    public static string GeneratBadyCod(string TableName)
    {
        string text = "";
        text = text  + GeneratEnums();
        text = text + clsGeneralUtils.GeneratMethodesForClass(TableName);
        text = text + GeneratDefalutConstructor(TableName);
        text = text + clsGeneralUtils.GeneratParameterConstructor(TableName);
        text = text + GeneratAdd(TableName);
        text = text + GeneratGetAll(TableName);
        text = text + GeneratUpdate(TableName);
        text = text + GeneratFind(TableName);
        text = text + GeneratDelete(TableName);
        return text + GeneratSave(TableName);
    }
    public static string GeneratDataBussiness(string TableName)
    {
        return "using System;\r\nusing System.Collections.Generic;\r\nusing System.Data;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nusing DataAccessLayer;\r\nnamespace Bussiness_Layer\r\n{\r\n    public class cls" + TableName + "\r\n    {\r\n" + GeneratBadyCod(TableName) + "\r\n    } \r\n    }\r\n";
    }
}
