using System;
using CWRK_DDD.Database;
using static CWRK_DDD.Database.DatabaseHandler.Database;
using static CWRK_DDD.Database.DatabaseHandler.UserDatabaseFields;
using static CWRK_DDD.Database.DatabaseHandler.MeetingDatabaseFields;
using static CWRK_DDD.Database.DatabaseHandler.ReportDatabaseFields;
using static CWRK_DDD.Database.DatabaseHandler.AssignmentDatabaseFields;

namespace CWRK_DDD
{
    internal class Program
    {
        private const string DatabaseLocation =
            "D:\\Uni Materials\\Coursework Final\\DDD\\Git\\CWRK_DDD\\Database\\UserDatabase.db";
        private const string ConnectionString = $"Data Source={DatabaseLocation}";
        public static UserAccounts.User CurrentUser;
        public static DatabaseHandler MyDatabase;
        
        

        static void Main(string[] args)
        {
            MyDatabase = new DatabaseHandler(ConnectionString);
            
            
            //Login Loop
            LoginViaConsole();
            
            //Menu Loop
            Menu();
            
            //Close down program.
            MyDatabase.Deconstruct();
        }

        /// <summary>
        /// Does login procedures
        /// </summary>
        private static void LoginViaConsole()
        {

            while (true)
            {
                Console.WriteLine("Enter your Username");
                string username = Console.ReadLine();
                Console.WriteLine("Enter your Password");
                string password = Console.ReadLine();
                var status = Login(username, password);
                if (status == LoginStatus.LoggedIn)
                {
                    break;
                }
                else
                {
                    switch (status)
                    {
                        case(LoginStatus.WrongPassword):
                            Console.WriteLine("Wrong password inputted, please try again.");
                            break;
                        case(LoginStatus.NoUserFound):
                            Console.WriteLine("No user found with that username. Please try again.");
                            break;
                    }
                }
            }

            
        }

        /// <summary>
        /// Status of Login Attempt
        /// </summary>
        private enum LoginStatus
        {
            NoUserFound,
            WrongPassword,
            LoggedIn
        }

        private static LoginStatus Login(string userid, string userPassword)
        {
            if (MyDatabase.QueryForExistance(Users,UserID.ToString(),userid) != null)
            {
                UserAccounts.User foundUser = new UserAccounts.User(userid,MyDatabase);
                if (foundUser.CheckPassword(userPassword))
                {
                    CurrentUser = foundUser;
                    return LoginStatus.LoggedIn;
                }
                else
                {
                    return LoginStatus.WrongPassword;
                }
            }

            return LoginStatus.NoUserFound;
        }
        

        private static List<Meeting> GetMeetings(UserAccounts.User user)
        {
            //TODO get meetings for a user.
            return null;
        }

        private static void Menu()
        {
            bool Loop = true;
            int Option = 0;
            while (Loop)
            {
                //TODO: setup menu based on account type
                Console.WriteLine($"---- Logged in as {CurrentUser.Name} ----");
                //todo: 1. Create Meeting
                //todo: 2. View My Meetings
                //todo: 3. Create Report
                //todo: 4. View My Past Reports
                //todo. 5. Logoff

                Loop = MenuSelections(Loop);
            }
        }
        
        
        private static bool MenuSelections(bool Loop)
        {
            int Option = HelperMethods.RepeatUntilNumber();
            switch (Option)
            {
                case (1):
                    CreateMeeting();
                    break;
                case (2):
                    ViewMyMeetings();
                    break;
                case (3):
                    CreateReport();
                    break;
                case (4):
                    ViewMyReports();
                    break;
                case (5):
                    Loop = false;
                    break;
            }

            return Loop;
        }

        private static void ViewMyMeetings()
        {
            
        }

        private static void ViewMyReports()
        {
            //TODO: if user account is student, show all reports sent by student.
            //TODO: otherwise show all reports sent by assigned students, or all reports if ST.
        }

        public static void CreateMeeting()
        {
            //TODO: ask for date and time. then set user.
        }

        public static void CreateReport()
        {
            //TODO: ask for info, then create a report and add to database.
            //TODO: CREATE REPORT DATABASE STUFF.
        }
    }
}
    