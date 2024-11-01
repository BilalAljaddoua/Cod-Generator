using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataAccessLayer;
//Because the code is automatically generated, press (ctrl + K + D) to organize the code .      (-;

namespace Bussiness_Layer
{
    public class clsUsers
    {

        /// <summary>
        /// Enumeration representing the mode of the operation (Add or Update).
        /// </summary>
        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Gets or sets the mode of the operation (AddNew or Update).
        /// </summary>
        public enMode Mode { set; get; }
        public int? UserID { set; get; }
        public int? PersonID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool? IsActive { set; get; }

        /// <summary>
        /// Gets the Data Transfer Object (DTO) representing the current User.
        /// </summary>
        public clsUsersDTO UDTO
        {
            get
            {
                return new clsUsersDTO(
                   this.UserID,
                   this.PersonID,
                   this.UserName,
                   this.Password,
                   this.IsActive);
            }
        }
        /// <summary>
        /// Represents a business layer class for managing User operations and interactions.
        /// </summary>
        public clsUsers(clsUsersDTO UsersDTO, enMode nMode = enMode.AddNew)
        {
            this.UserID = UsersDTO.UserID;
            this.PersonID = UsersDTO.PersonID;
            this.UserName = UsersDTO.UserName;
            this.Password = UsersDTO.Password;
            this.IsActive = UsersDTO.IsActive;
            this.Mode = nMode;
        }
        /// <summary>
        /// Adds a new User to the database.
        /// </summary>
        /// <returns>True if the User was added successfully; otherwise, false.</returns>
        private bool _AddUsers()
        {
            this.UserID = clsUsersData.AddToUsersTable(UDTO);
            return (this.UserID != null);
        }
        /// <summary>
        /// Retrieves all Users from the database.
        /// </summary>
        /// <returns>A list of <see cref="clsUsersDTO"/> objects representing all Users.</returns>
        static public List<clsUsersDTO> GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }
        /// <summary>
        /// Updates an existing User's information in the database.
        /// </summary>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        private bool _UpdateUsers()
        {
            return clsUsersData.UpdateUsersTable(UDTO);
        }
        /// <summary>
        /// Finds a Users by their UserID.
        /// </summary>
        /// <param name="UserID">The unique identifier for the User.</param>
        /// <returns>A <see cref="clsUsers"/> object if the User is found; otherwise, null.</returns>
        static public clsUsers FindByUserID(int UserID)
        {
            var User = clsUsersData.FindByUserID(UserID);
            if (User != null)
            {
                return new clsUsers(User, enMode.Update);
            }
            else
                return null;
        }

        /// <summary>
        /// Deletes a User from the database based on their UserID
        /// </summary>
        /// <param name="UserID">The unique identifier for the User to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public static bool DeleteUsers(int UserID)
        {
            return clsUsersData.DeleteUsers(UserID);
        }
        /// <summary>
        /// Saves the current User record in the database based on the specified mode.
        /// </summary>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddUsers())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:                    return _UpdateUsers();

            }

            return false;
        }
    }
}
