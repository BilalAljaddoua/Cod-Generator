

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
        string Comment = $@"
        /// <summary>
        /// Enumeration representing the mode of the operation (Add or Update).
        /// </summary>"+"\n";
        string Text= @"        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Gets or sets the mode of the operation (AddNew or Update).
        /// </summary>
       public enMode Mode  { set; get; }
";
        return Comment + Text;
    }
    public static string GeneratParameterConstructor(string TableName)
    {
        string Commint = $@"
        /// <summary>
        /// Initializes a new instance of the <see cref=""cls{TableName}""/> class with the specified DTO and mode.
        /// </summary>
        /// <param name=""{TableName}DTO"">The DTO object containing {TableName.Substring(0, TableName.Length - 1)} data.</param>
        /// <param name=""nMode"">The mode of the operation (default is AddNew).</param>"+"\n";
        string Comment = $@"
    /// <summary>
    /// Represents a business layer class for managing {TableName.Substring(0, TableName.Length - 1)} operations and interactions.
    /// </summary>" +"\n";
        string text = "        public   cls" + TableName + $"(cls{TableName}DTO {TableName}DTO,enMode nMode = enMode.AddNew){{";
        List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            text = text + "        this. " + dataTypeAndColumnNamesAndNullble[i].ColumnName + $"= {TableName}DTO." + dataTypeAndColumnNamesAndNullble[i].ColumnName + ";\n";
        }

        text += "         this.Mode = nMode;\r\n";
        return Comment+text + "}";
    }
    private static string GeneratDTO_Method(string TableName)
    {
        string Comment = $@"
        /// <summary>
        /// Gets the Data Transfer Object (DTO) representing the current {TableName.Substring(0, TableName.Length - 1)}.
        /// </summary>"+"\n";
        string text = $"    public       cls{TableName}DTO {TableName.Substring(0,1)}DTO {{\n";
        List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
        text += $@" get{{return new cls{TableName}DTO("+"\n";
        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            text += "        this." + dataTypeAndColumnNamesAndNullble[i].ColumnName + ", \n"; 
        }
        text=text.Substring(0, text.Length - 3);
        return Comment+text + ");}}";
    }
    private static string GeneratAdd(string TableName)
    {
        string Comment=$@"
        /// <summary>
        /// Adds a new {TableName.Substring(0, TableName.Length - 1)} to the database.
        /// </summary>
        /// <returns>True if the {TableName.Substring(0, TableName.Length - 1)} was added successfully; otherwise, false.</returns>
";
        string Text= $@"        private bool _Add{ TableName } () 
{{    this.{  clsGeneralUtils.GetNameOfPrimaryKey(TableName) } = cls{ TableName}Data.AddTo{ TableName}Table({TableName.Substring(0,1)}DTO);  
return (this.{clsGeneralUtils.GetNameOfPrimaryKey(TableName)} != null);        }}";

        return Comment + Text;
    }
    private static string GeneratGetAll(string TableName)
    {
        string Comment= $@"
        /// <summary>
        /// Retrieves all {TableName} from the database.
        /// </summary>
        /// <returns>A list of <see cref=""cls{TableName}DTO""/> objects representing all {TableName}.</returns>
        ";
        string Text= $"        static public List<cls{TableName}DTO> GetAll" + TableName + "()\r\n        {\r\n                return cls" + TableName + "Data.GetAll" + TableName + "();\r\n         }";
        return Comment + Text;
    }
    private static string GeneratUpdate(string TableName)
    {
        string Comment = $@"
        /// <summary>
        /// Updates an existing {TableName.Substring(0, TableName.Length - 1)}'s information in the database.
        /// </summary>
        /// <returns>True if the update was successful; otherwise, false.</returns>"+"\n";
        string Text= "        private bool _Update" + TableName + "()\r\n " +
   "       {            return cls" + TableName + "Data.Update" + TableName + "Table(" + TableName.Substring(0, 1) + "DTO);\r\n         }";
        return Comment + Text;
    }
    private static string GeneratFind(string TableName)
    {
        string Comment = $@"
        /// <summary>
        /// Finds a {TableName} by their {clsGeneralUtils.GetNameOfPrimaryKey(TableName)}.
        /// </summary>
        /// <param name=""{clsGeneralUtils.GetNameOfPrimaryKey(TableName)}"">The unique identifier for the {TableName.Substring(0, TableName.Length - 1)}.</param>
        /// <returns>A <see cref=""cls{TableName}""/> object if the {TableName.Substring(0, TableName.Length - 1)} is found; otherwise, null.</returns>"+"\n";
        string Text = $@" static  public cls{TableName} FindBy{clsGeneralUtils.GetNameOfPrimaryKey(TableName)}(int { clsGeneralUtils.GetNameOfPrimaryKey(TableName) })   {{  
            var {TableName.Substring(0, TableName.Length - 1)} = clsUsersData.FindBy{clsGeneralUtils.GetNameOfPrimaryKey(TableName)}( {clsGeneralUtils.GetNameOfPrimaryKey(TableName)});
if({TableName.Substring(0, TableName.Length - 1)}!=null)
{{
return new  cls{TableName}({TableName.Substring(0, TableName.Length - 1)},enMode.Update) ;
}}
else
return null;
}}
";
        return Comment+ Text;
    }
    private static string GeneratDelete(string TableName)
    {
        string Comment = $@"
        /// <summary>
        /// Deletes a {TableName.Substring(0, TableName.Length - 1)} from the database based on their {clsGeneralUtils.GetNameOfPrimaryKey(TableName)}
        /// </summary>
        /// <param name=""{clsGeneralUtils.GetNameOfPrimaryKey(TableName)}"">The unique identifier for the {TableName.Substring(0, TableName.Length - 1)} to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>"+"\n";
        string Text= "         public  static bool Delete" + TableName + "(int " + clsGeneralUtils.GetNameOfPrimaryKey(TableName) + ")\r\n        {\r\n              return cls" + TableName + "Data.Delete" + TableName + "(" + clsGeneralUtils.GetNameOfPrimaryKey(TableName) + ");\r\n        }";
        return Comment + Text;
    }
    private static string GeneratSave(string TableName)
    {
        string Comment = $@"
        /// <summary>
        /// Saves the current User record in the database based on the specified mode.
        /// </summary>
        /// <returns>True if the operation is successful; otherwise, false.</returns>"+"\n";
        string Text= "        public bool Save()\r\n        {\r\n                   switch (Mode)\r\n            {              case enMode.AddNew:\r\n                    if (_Add" + TableName + "())\r\n                    {\r\n                      Mode = enMode.Update;\r\n                        return true;\r\n                    }\r\n                    else\r\n                    {\r\n                        return false;\r\n                    }\r\n                case enMode.Update:\r                    return _Update" + TableName + "();\r\n\r\n            }\r\n\r\n            return false;\r\n                }";
        return Comment + Text;
    }
    public static string GeneratBadyCod(string TableName)
    {
        string text = "";
        text = text  + GeneratEnums();
        text = text + clsGeneralUtils.GeneratMethodesForClass(TableName);
        text = text + GeneratDTO_Method(TableName);
        text = text + GeneratParameterConstructor(TableName);
        text = text + GeneratAdd(TableName);
        text = text + GeneratGetAll(TableName);
        text = text + GeneratUpdate(TableName);
        text = text + GeneratFind(TableName);
        text = text + GeneratDelete(TableName);
        return text + GeneratSave(TableName);
    }
    public static string GeneratDataBussiness(string TableName)
    {
        return @"using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataAccessLayer;
//Because the code is automatically generated, press (ctrl + K + D) to organize the code .      (-;

namespace Bussiness_Layer{
public class cls" + TableName + "\r\n    {\r\n" + GeneratBadyCod(TableName) + "\r\n    } \r\n    }\r\n";
    }
}
