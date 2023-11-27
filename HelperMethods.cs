using System.Data.SQLite;
using System.Reflection.Metadata.Ecma335;
using CWRK_DDD.Database;
using static CWRK_DDD.Database.DatabaseHandler.Database;
using static CWRK_DDD.Database.DatabaseHandler.MeetingDatabaseFields;
using static CWRK_DDD.Database.DatabaseHandler.ReportDatabaseFields;
using static CWRK_DDD.Database.DatabaseHandler.UserDatabaseFields;
using static CWRK_DDD.Database.DatabaseHandler.AssignmentDatabaseFields;

namespace CWRK_DDD;

/// <summary>
/// Assorted methods for assisting in console operations.
/// </summary>
public static class HelperMethods
{
    /// <summary>
    /// Repeat entering input until the input can be parsed as a number, then return that number.
    /// </summary>
    /// <returns>Parsed number.</returns>
    public static int RepeatUntilNumber()
    {
        string input = Console.ReadLine();
        int number;
        while (int.TryParse(input, out number) == false)
        {
            Console.WriteLine("Not a number. Please try again.");
            input = Console.ReadLine();
        }
        return number;
    }
    
/// <summary>
/// Create a list of meetings sent from a user.
/// </summary>
/// <param name="user">User account</param>
/// <param name="database">Database handler</param>
/// <returns></returns>
    public static List<Meeting> CreateMeetingsFromUser(UserAccounts.User user, DatabaseHandler database, bool includePast = false)
    {
        //query for list of meeting ids that involve the user.
        List<string> meetingIds = database.QueryMultiple(Meetings,
            MeetingID.ToString(), Sender.ToString(), user.ID);
        //go through list, create meeting for each.
        List<Meeting> MyMeetings = new List<Meeting>();
        foreach (var meetingId in meetingIds)
        {
            string encodedDateTime = database.QuerySingleMeetingField(meetingId, Date);
            DateTime dateTime = DateTime.Parse(encodedDateTime);
            if (dateTime.CompareTo(DateTime.Today) < 0 && !includePast) continue;
            string sender = database.QuerySingleMeetingField(meetingId, Sender);
            string recipent = database.QuerySingleMeetingField(meetingId, Recipent);
            string meetingName = database.QuerySingleMeetingField(meetingId, MeetingName);
            Meeting meeting = new(meetingId, sender, recipent,meetingName,dateTime);
            MyMeetings.Add(meeting);
        }
        MyMeetings.Sort();
        return MyMeetings;
    }
    
    /// <summary>
    /// Create a list of Meetings sent to a user.
    /// </summary>
    /// <param name="user">User account</param>
    /// <param name="database">Database handler</param>
    /// <param name="includePast">Include meetings in the past. Defaults to False</param>
    /// <returns></returns>
    public static List<Meeting> CreateMeetingsForUser(UserAccounts.User user, DatabaseHandler database, bool includePast = false)
    {
        //query for list of meeting ids that involve the user.
        List<string> meetingIds = database.QueryMultiple(Meetings,
            MeetingID.ToString(), Recipent.ToString(), user.ID);
        //go through list, create meeting for each.
        List<Meeting> MyMeetings = new List<Meeting>();
        foreach (var meetingId in meetingIds)
        {
            string encodedDateTime = database.QuerySingleMeetingField(meetingId, Date);
            DateTime dateTime = DateTime.Parse(encodedDateTime);
            if(dateTime.CompareTo(DateTime.Today) < 0 && !includePast) continue;
            string sender = database.QuerySingleMeetingField(meetingId, Sender);
            string recipent = database.QuerySingleMeetingField(meetingId, Recipent);
            string meetingName = database.QuerySingleMeetingField(meetingId, MeetingName);
            Meeting meeting = new(meetingId, sender, recipent,meetingName,dateTime);
            MyMeetings.Add(meeting);
        }
        MyMeetings.Sort();
        return MyMeetings;
    }

    public static List<Meeting> AllMyMeetings(UserAccounts.User user, DatabaseHandler databaseHandler, bool IncludePast = false)
    {
        List<Meeting> AllOfThem = CreateMeetingsForUser(user,databaseHandler, IncludePast);
        AllOfThem.Concat(CreateMeetingsFromUser(user, databaseHandler, IncludePast));
        return AllOfThem;
    }
    
    
    /// <summary>
    /// Create Reports made from a user.
    /// </summary>
    /// <param name="user">User Account</param>
    /// <param name="database">Database handler</param>
    /// <returns></returns>
    public static List<Report> CreateReportsFromUser(UserAccounts.User user, DatabaseHandler database)
    {
        //query for list of meeting ids that involve the user.
        List<string> reportIds = database.QueryMultiple(Reports,
            ReportID.ToString(), ReporterID.ToString(), user.ID);
        //go through list, create meeting for each.
        List<Report> MyReports = new List<Report>();
        foreach (var reportId in reportIds)
        {
            string reportInfo = database.QuerySingleReportField(reportId, DatabaseHandler.ReportDatabaseFields.ReportInfo);
            string encodedDateTime = database.QuerySingleReportField(reportId, DatabaseHandler.ReportDatabaseFields.DateOfReport);
            DateTime dateTime = DateTime.Parse(encodedDateTime);
            Report report = new(reportId, user.ID, reportInfo, dateTime);
            MyReports.Add(report);
        }
        MyReports.Sort();
        return MyReports;
    }
}