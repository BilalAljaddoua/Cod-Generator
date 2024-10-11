﻿using GeneratSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Layer
{
    public class clsGenerateStordProcedures
    {


        //These Functions resposible to generate Update Stored Proceder
        public static string GenerateParameterForUpdate(string TableName)
        {
            List<clsGeneralUtils.stColumns> columns = clsGeneralUtils.GetDataTypeAndColumnNamesForSP(TableName);
            string PreperParameters = "";

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].DataType == "nvarchar" || columns[i].DataType == "varchar" || columns[i].DataType == "char")
                {
                    PreperParameters += $@"@{columns[i].ColumnName}  {columns[i].DataType}(1000) ," + "\n";
                }
                else
                    PreperParameters += $@"@{columns[i].ColumnName}  {columns[i].DataType} ," + "\n";
            }
            PreperParameters = PreperParameters.Substring(0, PreperParameters.Length - 2);
            return PreperParameters;
        }
        public static string GetUpdateQuery(string TableName)
        {
            string Query = $@"
UPDATE {TableName}
set
{clsGeneralUtils.GetAllColumnsWithoutFirstOneForUpdate(TableName, "@")}
where
{clsGeneralUtils.GetPrimaryKey(TableName)}=@{clsGeneralUtils.GetPrimaryKey(TableName)}


";
            return Query;


        }
        public static string GeneratSP_ForUpdate(string TableName)
        {
            string Scrept = $@"
            CREATE PROCEDURE SP_Update{TableName}Table
            {GenerateParameterForUpdate(TableName)} 
            as
            begin
            {GetUpdateQuery(TableName)}
            end";
            SqlConnection connection = new SqlConnection(clsSettingsClass.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(Scrept, connection);
            bool IsSuccess = true;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { IsSuccess = false; }
            finally { connection.Close(); }

            if (IsSuccess)
                return $"\"SP_Update{TableName}Table\"";
            else
                return "We didn't create stored procedure";
        }

        //These Functions resposible to generate Insert Stored Proceder
        public static string GenerateParameterForInsert(string TableName)
        {
            List<clsGeneralUtils.stColumns> columns = clsGeneralUtils.GetDataTypeAndColumnNamesForSP(TableName);
            string PreperParameters = "";

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].DataType == "nvarchar" || columns[i].DataType == "varchar" || columns[i].DataType == "char")
                {
                    PreperParameters += $@"@{columns[i].ColumnName}  {columns[i].DataType}(1000) ," + "\n";
                }
                else
                    PreperParameters += $@"@{columns[i].ColumnName}  {columns[i].DataType} ," + "\n";
            }
            PreperParameters += "@ID INT output";

            return PreperParameters;
        }
        public static string GetInsertQuery(string TableName)
        {
            string Query = $@"
INSERT INTO {TableName}
(
{clsGeneralUtils.GetAllColumnsWithoutFirstOneForInsert(TableName)} )
VALUES
({clsGeneralUtils.GetAllColumnsWithoutFirstOneForInsert(TableName, "@")})

SET @ID=SCOPE_IDENTITY();
";
            return Query;


        }
        public static string GeneratSP_ForInsert(string TableName)
        {
            string Scrept = $@"
            CREATE PROCEDURE SP_InsertInto{TableName}Table
            {GenerateParameterForInsert(TableName)} 
            as
            begin
            {GetInsertQuery(TableName)}
            end";
            SqlConnection connection = new SqlConnection(clsSettingsClass.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(Scrept, connection);
            bool IsSuccess = true;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { IsSuccess = false; }
            finally { connection.Close(); }

            if (IsSuccess)
                return $"\"SP_InsertInto{TableName}Table\"";
            else
                return "We didn't create stored procedure";
        }

        //These Functions resposible to generate Select Stored Proceder

        public static string GeneratSP_ForSelect(string TableName)
        {
            string Scrept = $@" CREATE PROCEDURE SP_SelectForm{TableName}Table
            as
            begin
             SELECT * FROM {TableName}
            end";
            SqlConnection connection = new SqlConnection(clsSettingsClass.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(Scrept, connection);
            bool IsSuccess = true;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { IsSuccess = false; }
            finally { connection.Close(); }

            if (IsSuccess)
                return $"\"SP_SelectForm{TableName}Table\"";
            else
                return "\"We didn't create stored procedure\"";
        }

        //These Functions resposible to generate Select Stored Proceder
        public static string GeneratSP_ForFind(string TableName)
        {
            string Scrept = $@" CREATE PROCEDURE SP_FindForm{TableName}Table
            @{clsGeneralUtils.GetPrimaryKey(TableName)} int
            as
            begin
             SELECT * FROM {TableName}
             where {clsGeneralUtils.GetPrimaryKey(TableName)} = @{clsGeneralUtils.GetPrimaryKey(TableName)}
            end";
            SqlConnection connection = new SqlConnection(clsSettingsClass.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(Scrept, connection);
            bool IsSuccess = true;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { IsSuccess = false; }
            finally { connection.Close(); }

            if (IsSuccess)
                return $"\"SP_FindForm{TableName}Table\"";
            else
                return "We didn't create stored procedure";
        }

        //These Functions resposible to generate Select Stored Proceder
        public static string GeneratSP_ForDelete(string TableName)
        {
            string Scrept = $@" CREATE PROCEDURE SP_DeleteForm{TableName}Table
            @{clsGeneralUtils.GetPrimaryKey(TableName)} int
            as
            begin
             DELETE FROM {TableName}
             where {clsGeneralUtils.GetPrimaryKey(TableName)} = @{clsGeneralUtils.GetPrimaryKey(TableName)}
            end";
            SqlConnection connection = new SqlConnection(clsSettingsClass.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(Scrept, connection);
            bool IsSuccess = true;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { IsSuccess = false; }
            finally { connection.Close(); }

            if (IsSuccess)
                return $"\"SP_DeleteForm{TableName}Table\"";
            else
                return "We didn't create stored procedure";
        }




    }




}
