using System.Security.Cryptography;
using System.Text;
using CWRK_DDD.Database;
using static CWRK_DDD.Database.DatabaseHandler.UserDatabaseFields;

namespace CWRK_DDD;

public class UserAccounts
{
    public class User //USERID,PASSWORD,NAME,ACCOUNTTYPE,ASSIGNEDUSER,COURSE
    {
        /// <summary>
        /// User's ID. Should never be changed once set.
        /// </summary>
        public string ID
        {
            get;
            private set;
        }

        /// <summary>
        /// User's Name. Should be changeable via ChangeName();
        /// Defaults to 'Guest' 
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Type of Account
        /// <see cref="AccountType"/>
        /// </summary>
        public AccountType AccountType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Hashed Password in byte form.
        /// </summary>
        public string _hashedPassword
        {
            get;
            private set;
        }

        public void ChangePassword(string newPassword)
        {
            byte[] data = SHA256.HashData(Encoding.UTF8.GetBytes(newPassword));
            _hashedPassword = data.ToString();
        }


        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="id">User's ID</param>
        /// <param name="name">User's Name</param>
        /// <param name="hashedPass">Hash of Password in the form of Bytes</param>
        public User(string id, string name, string hashedPass)
        {
            ID = id;
            Name = name;
            _hashedPassword = hashedPass;
        }

        /// <summary>
        /// finds a user from the database and creates an User object for them
        /// </summary>
        /// <param name="ID">User's ID</param>
        public User(string userID, DatabaseHandler databaseHandler)
        {
            ID = userID;
            Name = databaseHandler.QuerySingleUserField(userID, DatabaseHandler.UserDatabaseFields.Name);
            _hashedPassword = databaseHandler.QuerySingleUserField(userID, Password);
            
        }

        /// <summary>
        /// Check if a password is correct for this account
        /// </summary>
        /// <param name="password">Password in UTF8</param>
        /// <returns>If the password is correct.</returns>
        public bool CheckPassword(string password)
        {
            byte[] data = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            string encoded = data.ToString();
            return (_hashedPassword == encoded);
        }

        /// <summary>
        /// Change a User's Name.
        /// </summary>
        /// <param name="newName">New name for the user.</param>
        public void ChangeName(string newName)
        {
            Name = newName;
        }
    }

    /// <summary>
    /// Type of User account.
    /// 0 = Student
    /// 1 = Personal Supervisor
    /// 2 = Senior Tutor
    /// </summary>
    public enum AccountType
    {
        STUDENT = 0,
        PERSONAL_SUPERVISOR = 1,
        SENIOR_TUTOR = 2
    }

    public class Student : User
    {
        public string assignedSupervisor;
        public Student(string id, string Name, string hashedPass) : base(id,Name, hashedPass)
        {
            AccountType = AccountType.STUDENT;
        }
    }

    public class PersonalSupervisor : User
    {
        public List<string> assignedStudentIds;

        
        public PersonalSupervisor(string _id, string _name, string _hashedPass) : base(_id, _name, _hashedPass)
        {
            
            AccountType = AccountType.PERSONAL_SUPERVISOR;
        }

        public PersonalSupervisor(string UserID, DatabaseHandler databaseHandler) : base(UserID, databaseHandler)
        {
            
        }
        
    }

    public class SeniorTutor : User
    {

        public SeniorTutor(string _id, string _name, string _hashedPass) : base(_id, _name, _hashedPass)
        {
            AccountType = AccountType.SENIOR_TUTOR;
        }
    }
}