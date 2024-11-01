using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GeneratSettings;
//Because the code is automatically generated, press (ctrl + K + D) to organize the code .      (-;
/// <summary>
/// Provides data access methods for managing Users in the database.
/// </summary>
namespace DataAccessLayer
{
    /// <summary>
    /// Represents a User data transfer object (DTO) for transferring User information.
    /// </summary>
    public class clsUsersDTO
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode { set; get; }

        public int? UserID { set; get; }
        public int? PersonID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool? IsActive { set; get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="clsUsersDTO"/> class with specified properties.
        /// </summary>
        public clsUsersDTO(int? UserID, int? PersonID, string UserName, string Password, bool? IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
        }
    }
    public class clsUsersData
    {
        /// <summary>
        /// Logs an error to the Windows Event Log with the specified method name and exception details.
        /// </summary>
        /// <param name="methodName">The name of the method where the error occurred.</param>
        /// <param name="ex">The exception that was thrown.</param>
        static void LogError(string methodName, Exception ex)
        {
            string source = "DVLD";
            string logName = "Application";

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }

            EventLog.WriteEntry(source, $"Error in " + methodName + ":" + ex.Message, EventLogEntryType.Error);
        }
        /// <summary>
        /// Retrieves all Users from the database.
        /// </summary>
        /// <returns>A list of <see cref="clsUsersDTO"/> objects representing all Users.</returns>
        public static List<clsUsersDTO> GetAllUsers()
        {
            var UsersList = new List<clsUsersDTO>();
            using (SqlConnection connection = new SqlConnection(" "))
            {
                using (SqlCommand command = new SqlCommand("SP_SelectFormUsersTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UsersList.Add(new clsUsersDTO(
                                      Convert.ToInt32(reader["UserID"]),
                                      Convert.ToInt32(reader["PersonID"]),
                                      Convert.ToString(reader["UserName"]),
                                      Convert.ToString(reader["Password"]),
                                      Convert.ToBoolean(reader["IsActive"])));
                            }
                        }
                    }
                    catch (Exception ex) { LogError("GetAll Method", ex); }

                    return UsersList;
                }
            }
        }/// <summary>
         /// Finds a Users by their unique UserID.
         /// </summary>
         /// <param name="UserID">The unique identifier for the User.</param>
         /// <returns>A <see cref="clsUsersDTO"/> object if the User is found, otherwise null.</returns>
        static public clsUsersDTO FindByUserID(int UserID)
        {
            clsUsersDTO UsersDTO = null;
            using (SqlConnection connection = new SqlConnection(" "))
            {
                using (SqlCommand command = new SqlCommand("SP_FindFormUsersTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UsersDTO = new clsUsersDTO(
                                      Convert.ToInt32(reader["UserID"]),
                                      Convert.ToInt32(reader["PersonID"]),
                                      Convert.ToString(reader["UserName"]),
                                      Convert.ToString(reader["Password"]),
                                      Convert.ToBoolean(reader["IsActive"]));
                            }
                        }
                    }
                    catch (Exception ex) { LogError("Find Method", ex); }

                    return UsersDTO;
                }
            }
        }
        /// <summary>
        /// Adds a new User to the database.
        /// </summary>
        /// <param name="UsersDTO">The <see cref="clsUsersDTO"/> object containing Users information.</param>
        /// <returns>The UserID of the newly added User, or null if the operation fails.</returns>
        static public int? AddToUsersTable(clsUsersDTO UsersDTO)
        {
            using (SqlConnection connection = new SqlConnection(" "))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertIntoUsersTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", UsersDTO.PersonID);
                    command.Parameters.AddWithValue("@UserName", UsersDTO.UserName);
                    command.Parameters.AddWithValue("@Password", UsersDTO.Password);
                    command.Parameters.AddWithValue("@IsActive", UsersDTO.IsActive);

                    SqlParameter parameter = new SqlParameter("@UserID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    int? UserID = null;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        UserID = (int)command.Parameters["@UserID"].Value;
                    }
                    catch (Exception ex) { LogError("AddNew Method", ex); }

                    return UserID;
                }
            }
        }

        /// <summary>
        /// Updates an existing User's information in the database.
        /// </summary>
        /// <param name="UsersDTO">The <see cref="clsUsersDTO"/> object containing updated User information.</param>
        /// <returns>True if the update is successful, otherwise false.</returns>
        static public bool UpdateUsersTable(clsUsersDTO UsersDTO)
        {
            using (SqlConnection connection = new SqlConnection(" "))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateUsersTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UsersDTO.UserID);
                    command.Parameters.AddWithValue("@PersonID", UsersDTO.PersonID);
                    command.Parameters.AddWithValue("@UserName", UsersDTO.UserName);
                    command.Parameters.AddWithValue("@Password", UsersDTO.Password);
                    command.Parameters.AddWithValue("@IsActive", UsersDTO.IsActive);

                    SqlParameter parameter = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    bool IsSuccess = false;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        IsSuccess = (bool)command.Parameters["@IsSuccess"].Value;
                    }
                    catch (Exception ex) { LogError("Update Method", ex); }


                    return IsSuccess;
                }
            }
        }

        /// <summary>
        /// Deletes a User from the database based on their UserID.
        /// </summary>
        /// <param name="UserID">The unique identifier for the User to delete.</param>
        /// <returns>True if the deletion is successful, otherwise false.</returns>
        static public bool DeleteUsers(int UserID)
        {
            using (SqlConnection connection = new SqlConnection(" "))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteFormUsersTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserID", UserID);
                    SqlParameter parameter = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    bool IsSuccess = false;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        IsSuccess = (bool)command.Parameters["@IsSuccess"].Value;
                    }
                    catch (Exception ex) { LogError("Delete Method", ex); }

                    return IsSuccess;
                }
            }
        }

    }
}
