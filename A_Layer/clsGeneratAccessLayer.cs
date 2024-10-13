using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using A_Layer;
using GeneratSettings;
public class clsDataAccessLAyer
{
     private static string AddAllParameterWithValue(string TableName)
    {
        string text = "";
        List<clsGeneralUtils.stColumns> ColumnInfo = clsGeneralUtils.GetDataTypeAndColumnNamesAndNullble(TableName);
        for (int i = 1; i < ColumnInfo.Count; i++)
        {
            if (ColumnInfo[i].AllowNull)
            {
                text += $"command.Parameters.AddWithValue(\"@"+ ColumnInfo[i].ColumnName + $"\", (object){ColumnInfo[i].ColumnName} ?? DBNull.Value);\r\n";
             }
            else
            text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + "\", " + ColumnInfo[i].ColumnName + ");\n";
        }
         return text;
    }
    private static string AddOneParameterWithValue(string TableName)
    {
        return "\n command.Parameters.AddWithValue(\"@" + clsGeneralUtils.GetPrimaryKey(TableName) + "\", " + clsGeneralUtils.GetPrimaryKey(TableName) + ");";
    }
    private static string AddParameterWithDataType(string TableName, string Prefix = "")
    {
        string text = "";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        string cmdText = "\r\n    SELECT COLUMN_NAME,DATA_TYPE \r\n    FROM INFORMATION_SCHEMA.COLUMNS\r\n             WHERE TABLE_NAME = '" + TableName + "' ";
        SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
        SqlDataReader reader = sqlCommand.ExecuteReader();
        while (reader.Read())
        {
            if(clsGeneralUtils.GetDataType(reader.GetString(1)) !="string")
            text = text + Prefix + clsGeneralUtils.GetDataType(reader.GetString(1)) + "?  " + reader.GetString(0) + ",";
            else
                text = text + Prefix + clsGeneralUtils.GetDataType(reader.GetString(1)) + " " + reader.GetString(0) + ",";
        }

        reader.Close();
        sqlConnection.Close();
        return text.Substring(0, text.Length - 1);
    }
    public static string GeneratAddNewCod(string TableName)
    {
        string text = $@"
static public  int ? AddTo{TableName}Table({AddParameterWithDataType(TableName)})
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForInsert(TableName)}, connection))
        {{
              command.CommandType = CommandType.StoredProcedure;
{AddAllParameterWithValue(TableName)}
SqlParameter parameter = new SqlParameter(""@ID"", SqlDbType.Int)
             {{
                 Direction = ParameterDirection.Output
             }};
             command.Parameters.Add(parameter);
             int ? ID =null;
             try
             {{
                 connection.Open();
                 command.ExecuteNonQuery();
                 ID = (int)command.Parameters[""@ID""].Value;
             }}
             catch (Exception ex) {{ }}
             finally
             {{
                 connection.Close();
             }}
             return ID;
        }}
    }}
}}
";
         return text;
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
                    " {\r\n  " + dataTypeAndColumnNamesAndNullble[i].ColumnName +
                    " = (" + clsGeneralUtils.GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType)
                    + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].ColumnName + "\"]));\r\n} ";
            }
            else
            text = text + dataTypeAndColumnNamesAndNullble[i].ColumnName + " =  " + clsGeneralUtils.GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType) + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].ColumnName + "\"]);\n\t\t";
        }

        return text.Substring(0, text.Length - 1);
    }
    public static string GeneratFindCod(string TableName)
    {
        string text = $@"static public bool Find{TableName}({AddParameterWithDataType(TableName," ref ")})
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForFind(TableName)}, connection))
        {{ 
            command.CommandType = CommandType.StoredProcedure; 
{AddOneParameterWithValue(TableName)}
bool IsRead = false;

             try
             {{
                 connection.Open();
                 SqlDataReader reader = command.ExecuteReader();
           
                 if (reader.Read())
                 {{
                     IsRead = true;
{GetInformationForRead(TableName)}
}}
                 reader.Close();
             }}
             catch (Exception ex) {{ }}
             finally
             {{
                 connection.Close();
             }}
             return IsRead;
        }}
    }} }} ";
        return text;
    }
    public static string GeneratUpdateCod(string TableName)
    {
        string text = $@"static public bool Update{TableName}Table( {AddParameterWithDataType(TableName)} )
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
         using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForUpdate(TableName)}, connection))
         {{
             command.CommandType = CommandType.StoredProcedure; 
{AddAllParameterWithValue(TableName)}
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
             catch (Exception ex) {{ }}
             finally
             {{    connection.Close(); }}
           
             return IsSuccess;
         }}
    }}
}}
";
        return text;
    }
    public static string GeneratDeleteCod(string TableName)
    {
        string text2 = $@"static public bool Delete{TableName}()
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForDelete(TableName)}, connection))
        {{
            command.CommandType = CommandType.StoredProcedure;
         //   command.Parameters.AddWithValue(""@PersonID"", PersonID);
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
            catch (Exception ex) {{ }}
            finally
            {{
                connection.Close();
            }}
           
            return IsSuccess;
        }}
    }}
}}
"; 
        return text2;
    }
    private static string GeneratSelectCod(string TableName)
    {
        string text = $@" public static DataTable  GetAll{TableName}()
{{
    using (SqlConnection connection = new SqlConnection( clsDataAccessSettings.ConnectionString ))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForSelect(TableName)}, connection))
        {{
            command.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            try
            {{
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {{
                    dt.Load(Reader);
                }}
            }}
            catch (Exception ex) {{ }}
            finally
            {{  connection.Close(); }}
            return dt;
        }}
    }}
}}";


        return text;
    }
    public static string GeneratDataAccess(string TableName)
    {
          clsGenerateStordProcedures.GeneratSP_ForFind(TableName);
        string text = "";
        text += GeneratSelectCod(TableName);
        text += GeneratFindCod(TableName);
        text += GeneratAddNewCod(TableName);
        text += GeneratUpdateCod(TableName);
        text += GeneratDeleteCod(TableName);
        return @"using System;
 using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GeneratSettings;
     namespace DataAccessLayer
{  
public class  cls" + TableName + "Data\r\n    {\r\n" + text + "\r\n   } \r\n}\r\n";


    }


}
 