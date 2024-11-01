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

        return Text;
    }

    private static string AddAllParameterWithValueForInsert(string TabelName)
    {
        string text = "";
        List<clsGeneralUtils.stColumns> ColumnInfo = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TabelName);
        for (int i = 1; i < ColumnInfo.Count; i++)
        {
            if (ColumnInfo[i].AllowNull)
            {
                text += $"command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + $"\", (object){ColumnInfo[i].ColumnName} ?? DBNull.Value);\r\n";
            }
            else
                text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + "\", " + $"{TabelName}DTO." + ColumnInfo[i].ColumnName + ");\n";
        }
        return text;
    }
    private static string AddAllParameterWithValueForUpdate(string TabelName)
    {
        string text = "";
        List<clsGeneralUtils.stColumns> ColumnInfo = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TabelName);
        for (int i = 0; i < ColumnInfo.Count; i++)
        {
            if (ColumnInfo[i].AllowNull)
            {
                text += $"command.Parameters.AddWithValue(\"@"+ ColumnInfo[i].ColumnName + $"\", (object){ColumnInfo[i].ColumnName} ?? DBNull.Value);\r\n";
             }
            else
            text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + "\", " + $"{TabelName}DTO." + ColumnInfo[i].ColumnName + ");\n";
        }
         return text;
    }
    private static string AddOneParameterWithValue(string TabelName)
    {
        return "\n command.Parameters.AddWithValue(\"@" + clsGeneralUtils.GetPK(TabelName).ColumnName + "\", " + clsGeneralUtils.GetPK(TabelName).ColumnName + ");";
    }
    private static string GeneratDTO_Class(string TableName)
    {
        string Text="";
        Text += $@"public class cls{TableName}DTO
{{
{clsGeneralUtils.GeneratEnumsForDTO()}
{clsGeneralUtils.GeneratMethodesForDTO(TableName)}
{clsGeneralUtils.GeneratParameterConstructoForDTO(TableName)}
}}";

        return Text;
    }
    public static string GeneratInsertCod(string TabelName)
    {
        clsGeneralUtils.stColumns PK=clsGeneralUtils.GetPK(TabelName);
        string text = $@"
static public  int ? AddTo{TabelName}Table(cls{TabelName}DTO {TabelName}DTO)
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForInsert(TabelName)}, connection))
        {{
              command.CommandType = CommandType.StoredProcedure;
{AddAllParameterWithValueForInsert(TabelName)}
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
         return text;
    }
    private static string GetInformationForRead(string TabelName)
    {
        string text = "";
        List<clsGeneralUtils.stColumns> dataTypeAndColumnNamesAndNullble = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TabelName);

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
     public static string GeneratFindCod(string TabelName)
    {
        string text = $@"static public  cls{TabelName}DTO   Find{TabelName}({clsGeneralUtils.GetPK(TabelName).DataType} {clsGeneralUtils.GetPK(TabelName).ColumnName})
{{
            cls{TabelName}DTO {TabelName}DTO =null;
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForFind(TabelName)}, connection))
        {{ 
            command.CommandType = CommandType.StoredProcedure;{AddOneParameterWithValue(TabelName)}
              try
             {{
                 connection.Open();
                 SqlDataReader reader = command.ExecuteReader();
                 if (reader.Read())
                 {{ 
                      {TabelName}DTO = new cls{TabelName}DTO({GetInformationForRead(TabelName)});
                        }}
                 reader.Close();
             }}
             catch (Exception ex) {{  LogError(""Find Method"",ex); }}
 
             return {TabelName}DTO;
        }}
    }} }} ";
        return text;
    }
    public static string GeneratUpdateCod(string TabelName)
    {
        string text = $@"static public bool Update{TabelName}Table(cls{TabelName}DTO {TabelName}DTO)
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
         using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForUpdate(TabelName)}, connection))
         {{
             command.CommandType = CommandType.StoredProcedure; 
{AddAllParameterWithValueForUpdate(TabelName)}
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
        return text;
    }
    public static string GeneratDeleteCod(string TabelName)
    {
        string text2 = $@"static public bool Delete{TabelName}({clsGeneralUtils.GetPK(TabelName).DataType} {clsGeneralUtils.GetPK(TabelName).ColumnName})
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForDelete(TabelName)}, connection))
        {{
            command.CommandType = CommandType.StoredProcedure;
{AddOneParameterWithValue(TabelName)} 
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
        return text2;
    }
    private static string GeneratSelectCod(string TabelName)
    {
       
        string text = $@" public static List<cls{TabelName}DTO>  GetAll{TabelName}()
    {{
  var {TabelName}List=new List<cls{TabelName}DTO>();
    using (SqlConnection connection = new SqlConnection( clsDataAccessSettings.ConnectionString ))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForSelect(TabelName)}, connection))
        {{
            command.CommandType = CommandType.StoredProcedure;
            try
            {{
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {{
                      {TabelName}List.Add(new cls{TabelName}DTO( {GetInformationForRead(TabelName)}  ) );
                }}
            }}
            catch(Exception ex) {{  LogError(""GetAll Method"",ex); }}

            return {TabelName}List;
        }}
    }}
}}";


        return text;
    }
    public static string GeneratDataAccess(string TabelName)
    {
 
        string text = "";
        text += GenerateLogError();
        text += GeneratSelectCod(TabelName);
        text += GeneratFindCod(TabelName);
        text += GeneratInsertCod(TabelName);
        text += GeneratUpdateCod(TabelName);
        text += GeneratDeleteCod(TabelName);
         return @"using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using GeneratSettings;
//Because the code is automatically generated, press (ctrl + K + D) to organize the code .      (-;
     namespace DataAccessLayer
{ " + GeneratDTO_Class(TabelName) +"\npublic class  cls" + TabelName + "Data\r\n    {\r\n" + text + "\r\n   } \r\n}\r\n";


    }


}
 