using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using A_Layer;
using GeneratSettings;
public class clsDataAccessLAyer
{
    public static string GenerateLogError()
    {
        string Commint = @"        /// <summary>
        /// Logs an error to the Windows Event Log with the specified method name and exception details.
        /// </summary>
        /// <param name=""methodName"">The name of the method where the error occurred.</param>
        /// <param name=""ex"">The exception that was thrown.</param>"+"\n";
        string Text = $@"  static  void LogError(string methodName, Exception ex)
    {{
        string source = ""{clsGeneralUtils.ApplicationName}"";
        string logName = ""Application"";

        if (!EventLog.SourceExists(source))
        {{
            EventLog.CreateEventSource(source, logName);
        }}

        EventLog.WriteEntry(source, $""Error in ""+methodName+"":""+ ex.Message, EventLogEntryType.Error);
}}";

        return Commint+ Text;
    }
    private static string AddAllParameterWithValueForInsert(string TableName)
    {
        string text = "";
        List<clsGeneralUtils.stColumns> ColumnInfo = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 1; i < ColumnInfo.Count; i++)
        {
            if (ColumnInfo[i].AllowNull)
            {
                text += $"command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + $"\", (object){ColumnInfo[i].ColumnName} ?? DBNull.Value);\r\n";
            }
            else
                text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + "\", " + $"{TableName}DTO." + ColumnInfo[i].ColumnName + ");\n";
        }
        return text;
    }
    private static string AddAllParameterWithValueForUpdate(string TableName)
    {
        string text = "";
        List<clsGeneralUtils.stColumns> ColumnInfo = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 0; i < ColumnInfo.Count; i++)
        {
            if (ColumnInfo[i].AllowNull)
            {
                text += $"command.Parameters.AddWithValue(\"@"+ ColumnInfo[i].ColumnName + $"\", (object){ColumnInfo[i].ColumnName} ?? DBNull.Value);\r\n";
             }
            else
            text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + "\", " + $"{TableName}DTO." + ColumnInfo[i].ColumnName + ");\n";
        }
         return text;
    }
    private static string AddOneParameterWithValue(string TableName)
    {
        return "\n command.Parameters.AddWithValue(\"@" + clsGeneralUtils.GetPK(TableName).ColumnName + "\", " + clsGeneralUtils.GetPK(TableName).ColumnName + ");";
    }
    private static string GeneratDTO_Class(string TableName)
    {
        string Comment = $@"
    /// <summary>
    /// Represents a {TableName.Substring(0, TableName.Length - 1)} data transfer object (DTO) for transferring {TableName.Substring(0, TableName.Length - 1)} information.
    /// </summary>"+"\n";
        string Text="";
        Text += $@"public class cls{TableName}DTO
{{
{clsGeneralUtils.GeneratEnumsForDTO()}
{clsGeneralUtils.GeneratMethodesForDTO(TableName)}
{clsGeneralUtils.GeneratParameterConstructoForDTO(TableName)}
}}";

        return Comment+Text;
    }
    public static string GeneratInsertCod(string TableName)
    {
        clsGeneralUtils.stColumns PK=clsGeneralUtils.GetPK(TableName);
        string Comment = $@"
        /// <summary>
        /// Adds a new {TableName.Substring(0, TableName.Length - 1)} to the database.
        /// </summary>
        /// <param name=""{TableName}DTO"">The <see cref=""cls{TableName}DTO""/> object containing {TableName} information.</param>
        /// <returns>The {PK.ColumnName} of the newly added {TableName.Substring(0,TableName.Length-1)}, or null if the operation fails.</returns>";
        string text = $@"
static public  int ? AddTo{TableName}Table(cls{TableName}DTO {TableName}DTO)
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForInsert(TableName)}, connection))
        {{
              command.CommandType = CommandType.StoredProcedure;
{AddAllParameterWithValueForInsert(TableName)}
SqlParameter parameter = new SqlParameter(""@{PK.ColumnName}"", SqlDbType.Int)
             {{
                 Direction = ParameterDirection.Output
             }};
             command.Parameters.Add(parameter);
             int ? {PK.ColumnName} =null;
             try
             {{
                 connection.Open();
                 command.ExecuteNonQuery();
                 {PK.ColumnName} = ({clsGeneralUtils.GetDataType(PK.DataType)})command.Parameters[""@{PK.ColumnName}""].Value;
             }}
             catch(Exception ex) {{  LogError(""AddNew Method"",ex); }}

             return {PK.ColumnName};
        }}
    }}
}}
";
         return Comment+text;
    }
    private static string GetInformationForRead(string TableName)
    {
        string text = "";
        List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);

        for (int i = 0; i < dataTypeAndColumnNamesAndNullble.Count; i++)
        {
            if (dataTypeAndColumnNamesAndNullble[i].AllowNull)
            {
                text = text + "      if (reader[\"" + dataTypeAndColumnNamesAndNullble[i].ColumnName + "\"] != DBNull.Value)\r\n" +
                    " {\r\n                             " + clsGeneralUtils.GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType)
                    + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].ColumnName + "\"]));\r\n} ";
            }
            else
            text = text + "\n                            " +    clsGeneralUtils.GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType) + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].ColumnName + "\"]),";
        }

        return text.Substring(0, text.Length - 1);
    }
     public static string GeneratFindCod(string TableName)
    {
        string Comment = $@"/// <summary>
        /// Finds a {TableName} by their unique {clsGeneralUtils.GetNameOfPrimaryKey(TableName)}.
        /// </summary>
        /// <param name=""{clsGeneralUtils.GetNameOfPrimaryKey(TableName)}"">The unique identifier for the {TableName.Substring(0, TableName.Length - 1)}.</param>
        /// <returns>A <see cref=""cls{TableName}DTO""/> object if the {TableName.Substring(0, TableName.Length - 1)} is found, otherwise null.</returns>" + "\n";
        string text = $@"static public  cls{TableName}DTO   FindBy{clsGeneralUtils.GetNameOfPrimaryKey(TableName)}({clsGeneralUtils.GetPK(TableName).DataType} {clsGeneralUtils.GetPK(TableName).ColumnName})
{{
            cls{TableName}DTO {TableName}DTO =null;
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForFind(TableName)}, connection))
        {{ 
            command.CommandType = CommandType.StoredProcedure;{AddOneParameterWithValue(TableName)}
              try
             {{
                 connection.Open();
        using ( SqlDataReader reader = command.ExecuteReader())
                 {{
                      if (reader.Read())
                 {{ 
                      {TableName}DTO = new cls{TableName}DTO({GetInformationForRead(TableName)});
                        }}
                 }}
                 }}
             catch (Exception ex) {{  LogError(""Find Method"",ex); }}
 
             return {TableName}DTO;
        }}
    }} }} ";
        return Comment+ text;
    }
    public static string GeneratUpdateCod(string TableName)
    {
        string comment=$@"
        /// <summary>
        /// Updates an existing {TableName.Substring(0, TableName.Length - 1)}'s information in the database.
        /// </summary>
        /// <param name=""{TableName}DTO"">The <see cref=""cls{TableName}DTO""/> object containing updated {TableName.Substring(0, TableName.Length - 1)} information.</param>
        /// <returns>True if the update is successful, otherwise false.</returns>"+"\n";
        string text = $@"static public bool Update{TableName}Table(cls{TableName}DTO {TableName}DTO)
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
         using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForUpdate(TableName)}, connection))
         {{
             command.CommandType = CommandType.StoredProcedure; 
{AddAllParameterWithValueForUpdate(TableName)}
SqlParameter parameter = new SqlParameter(""@IsSuccess"", SqlDbType.Bit)
             {{
                 Direction = ParameterDirection.Output
             }};
             command.Parameters.Add(parameter);
             bool IsSuccess = false;
             try
             {{
                 connection.Open();
                 command.ExecuteNonQuery();
                 IsSuccess = (bool)command.Parameters[""@IsSuccess""].Value;
             }}
             catch (Exception ex) {{  LogError(""Update Method"",ex); }}

           
             return IsSuccess;
         }}
    }}
}}
";
        return comment+text;
    }
    public static string GeneratDeleteCod(string TableName)
    {
        string Comment=$@"
        /// <summary>
        /// Deletes a {TableName.Substring(0, TableName.Length - 1)} from the database based on their {clsGeneralUtils.GetNameOfPrimaryKey(TableName)}.
        /// </summary>
        /// <param name=""{clsGeneralUtils.GetNameOfPrimaryKey(TableName)}"">The unique identifier for the {TableName.Substring(0,TableName.Length-1)} to delete.</param>
        /// <returns>True if the deletion is successful, otherwise false.</returns>"+"\n";
        string text2 = $@"static public bool Delete{TableName}({clsGeneralUtils.GetPK(TableName).DataType} {clsGeneralUtils.GetPK(TableName).ColumnName})
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForDelete(TableName)}, connection))
        {{
            command.CommandType = CommandType.StoredProcedure;
{AddOneParameterWithValue(TableName)} 
SqlParameter parameter = new SqlParameter(""@IsSuccess"", SqlDbType.Bit)
            {{
                Direction = ParameterDirection.Output
            }};
            command.Parameters.Add(parameter);
            bool IsSuccess = false;
            try
            {{
                connection.Open();
                command.ExecuteNonQuery();
                IsSuccess = (bool)command.Parameters[""@IsSuccess""].Value;
            }}
            catch (Exception ex) {{  LogError(""Delete Method"",ex) ;}}
           
            return IsSuccess;
        }}
    }}
}}
"; 
        return Comment+text2;
    }
    private static string GeneratSelectCod(string TableName)
    {
        string Comment = $@" 
        /// <summary>
        /// Retrieves all {TableName} from the database.
        /// </summary>
        /// <returns>A list of <see cref=""cls{TableName}DTO""/> objects representing all {TableName}.</returns>"+"\n";


        string text = $@" public static List<cls{TableName}DTO>  GetAll{TableName}()
    {{
  var {TableName}List=new List<cls{TableName}DTO>();
    using (SqlConnection connection = new SqlConnection( clsDataAccessSettings.ConnectionString ))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForSelect(TableName)}, connection))
        {{
            command.CommandType = CommandType.StoredProcedure;
            try
            {{
                connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
              {{  while (reader.Read())
                {{
                      {TableName}List.Add(new cls{TableName}DTO( {GetInformationForRead(TableName)}  ) );
                }}
            }}
            }}
            catch(Exception ex) {{  LogError(""GetAll Method"",ex); }}

            return {TableName}List;
        }}
    }}
}}";


        return Comment+text;
    }
    public static string GeneratDataAccess(string TableName)
    {
 
        string text = "";
        text += GenerateLogError();
        text += GeneratSelectCod(TableName);
        text += GeneratFindCod(TableName);
        text += GeneratInsertCod(TableName);
        text += GeneratUpdateCod(TableName);
        text += GeneratDeleteCod(TableName);
         return $@"using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using GeneratSettings;
//Because the code is automatically generated, press (ctrl + K + D) to organize the code .      (-;
    /// <summary>
    /// Provides data access methods for managing {TableName} in the database.
    /// </summary>
     namespace DataAccessLayer
{{ " + GeneratDTO_Class(TableName) +"\npublic class  cls" + TableName + "Data\r\n    {\r\n" + text + "\r\n   } \r\n}\r\n";


    }


}
 