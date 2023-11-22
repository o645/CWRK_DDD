using System;

namespace CWRK_DDD
{
    internal class Program
    {
        private const string UserDatabaseFile = "/loginDB.csv";
        private const string MeetingsDatabase = "/meetingDB.csv";
        private static List<User> UserDBLoaded = new List<User>();
        private static void CreateUserDB()
        {
            StreamReader reader = new(UserDatabaseFile);
            do
            {
                string user = reader.ReadLine();
                if (user != null)
                {
                    User newUser = new User(user);
                }
            }
        }

        static void Main(string[] args)
        {
            //Login Loop
            User CurrentUser = null;
            while (CurrentUser == null)
            {
                CurrentUser = Login();
            }
            //Menu Loop
        }

        /// <summary>
        /// Does login procedures
        /// </summary>
        /// <returns>Logged in user, or Null if no user found.</returns>
        private static User Login() //TODO: setup really basic encryption?
        {
            StreamReader loginDatabase = new StreamReader(UserDatabaseFile); //USERID,PASSWORD,NAME,ACCOUNTTYPE,ASSIGNEDUSER,COURSE
            Console.WriteLine("Please enter your ID");
            string ID = Console.ReadLine();
            Console.WriteLine("Please enter your Password.");
            string pass = Console.ReadLine();
            string[] userinfo;
            string userfile;
            do
            {
                userfile = loginDatabase.ReadLine();
                userinfo = userfile.Split(",");
            } while (userinfo[0] == ID);
            loginDatabase.Close();
            if (userinfo[1] == pass)
            {
                return CreateUser(userinfo);
            }
            else
            {
                Console.WriteLine("Incorrect User ID or Password. Please login again.");
                return null;
            }
        }
        
        /// <summary>
        /// Creates a User of the appropriate type.
        /// </summary>
        /// <param name="userinfo">User's info from database</param>
        /// <returns>User instance.</returns>
        private static User CreateUser(string[] userinfo)
        {
            User User;
            AccountType.TryParse(userinfo[3].ToUpper(), out AccountType accountType);
            switch (accountType)
            {
                case (AccountType.STUDENT):
                    User = new Student(userinfo);
                    break;
                case (AccountType.SENIOR_TUTOR):
                    User = new SeniorTutor(userinfo);
                    break;
                case (AccountType.PERSONAL_SUPERVISOR):
                    User = new PersonalSupervisor(userinfo);
                    break;
                default:
                    return null; //should never happen, but just incase.

            }

            return User;
        }

        private static List<Meeting> GetMeetings(User user)
        {
            
        }
        private static void Menu(User user)
        {
            
        }

        public static void CreateMeeting(User currentUser)
        {
            //TODO: ask for date and time. then set user.
        }
    }

    class Meeting
    {
        public string MeetingID;
        public User sender;
        public User recipent;
        public DateTime MeetingTime;
    }

    class User
    {
        protected string ID;
        public string Name;
        public List<Meeting> currentMeetings;
        public AccountType _accountType;
    }

    internal enum AccountType
    {
        STUDENT,
        PERSONAL_SUPERVISOR,
        SENIOR_TUTOR
    }

    class Student : User
    {
        

        public string assignedSupervisor;

        public Student(String[] userfile)
        {
            ID = userfile[0];
            Name = userfile[2];
            _accountType = AccountType.Parse<AccountType>(userfile[3].ToUpper());
            assignedSupervisor = userfile[4];
        }

    }

    class PersonalSupervisor : User
    {
        public List<string> assignedStudents;
        public PersonalSupervisor(String[] userfile)
        {
            ID = userfile[0];
            Name = userfile[2];
            _accountType = AccountType.Parse<AccountType>(userfile[3].ToUpper());
            assignedStudents = userfile[4].Split("|").ToList();
        }
        
        //TODO: allow updating assigned students list.
    }

    class SeniorTutor : User
    {
        public SeniorTutor(String[] userfile)
        {
            ID = userfile[0];
            Name = userfile[2];
            _accountType = AccountType.Parse<AccountType>(userfile[3].ToUpper());
        }
    }
    
    
}