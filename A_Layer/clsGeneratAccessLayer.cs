using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using A_Layer;
using GeneratSettings;
public class clsDataAccessLAyer
{
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
                text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + "\", " + ColumnInfo[i].ColumnName + ");\n";
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
            text = text + " command.Parameters.AddWithValue(\"@" + ColumnInfo[i].ColumnName + "\", " + ColumnInfo[i].ColumnName + ");\n";
        }
         return text;
    }
    private static string AddOneParameterWithValue(string TabelName)
    {
        return "\n command.Parameters.AddWithValue(\"@" + clsGeneralUtils.GetPK(TabelName).ColumnName + "\", " + clsGeneralUtils.GetPK(TabelName).ColumnName + ");";
    }
    private static string AddAllParameterWithDataTypeForUpdate(string TabelName, string Prefix = "")
    {
        string text = "";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        string cmdText = "\r\n    SELECT COLUMN_NAME,DATA_TYPE \r\n    FROM INFORMATION_SCHEMA.COLUMNS\r\n             WHERE TABLE_NAME = '" + TabelName + "' ";
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
    private static string AddAllParameterWithDataTypeForInsert(string TabelName, string Prefix = "")
    {
        string text = "";
        SqlConnection sqlConnection = new SqlConnection(clsSettingsClass.ConnectionString);
        sqlConnection.Open();
        string cmdText = @"SELECT COLUMN_NAME,DATA_TYPE, IS_NULLABLE 
                   FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = '" + TabelName + @"' 
                   AND COLUMN_NAME NOT IN(
                       SELECT COLUMN_NAME
                       FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                       WHERE TABLE_NAME = '" + TabelName + @"'
                       AND OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA +'.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                   )"; SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
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
    public static string GeneratInsertCod(string TabelName)
    {
        clsGeneralUtils.stColumns PK=clsGeneralUtils.GetPK(TabelName);
        string text = $@"
static public  int ? AddTo{TabelName}Table({AddAllParameterWithDataTypeForInsert(TabelName)})
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
             catch (Exception ex) {{ }}
             finally
             {{
                 connection.Close();
             }}
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
                    " {\r\n  " + dataTypeAndColumnNamesAndNullble[i].ColumnName +
                    " = (" + clsGeneralUtils.GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType)
                    + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].ColumnName + "\"]));\r\n} ";
            }
            else
            text = text + dataTypeAndColumnNamesAndNullble[i].ColumnName + " =  " + clsGeneralUtils.GetConvertType(dataTypeAndColumnNamesAndNullble[i].DataType) + "reader[\"" + dataTypeAndColumnNamesAndNullble[i].ColumnName + "\"]);\n\t\t";
        }

        return text.Substring(0, text.Length - 1);
    }
    public static string GeneratFindCod(string TabelName)
    {
        string text = $@"static public bool Find{TabelName}({AddAllParameterWithDataTypeForUpdate(TabelName," ref ")})
{{
    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForFind(TabelName)}, connection))
        {{ 
            command.CommandType = CommandType.StoredProcedure; 
{AddOneParameterWithValue(TabelName)}
bool IsRead = false;

             try
             {{
                 connection.Open();
                 SqlDataReader reader = command.ExecuteReader();
           
                 if (reader.Read())
                 {{
                     IsRead = true;
{GetInformationForRead(TabelName)}
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
    public static string GeneratUpdateCod(string TabelName)
    {
        string text = $@"static public bool Update{TabelName}Table( {AddAllParameterWithDataTypeForUpdate(TabelName)} )
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
    private static string GeneratSelectCod(string TabelName)
    {
        string text = $@" public static DataTable  GetAll{TabelName}()
{{
    using (SqlConnection connection = new SqlConnection( clsDataAccessSettings.ConnectionString ))
    {{
        using (SqlCommand command = new SqlCommand({clsGenerateStordProcedures.GeneratSP_ForSelect(TabelName)}, connection))
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
    public static string GeneratDataAccess(string TabelName)
    {
        
        string text = "";
        text += GeneratSelectCod(TabelName);
        text += GeneratFindCod(TabelName);
        text += GeneratInsertCod(TabelName);
        text += GeneratUpdateCod(TabelName);
        text += GeneratDeleteCod(TabelName);
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
public class  cls" + TabelName + "Data\r\n    {\r\n" + text + "\r\n   } \r\n}\r\n";


    }


}
 