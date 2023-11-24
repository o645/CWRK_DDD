using System.Data.SQLite;
using CWRK_DDD.Database;
using static CWRK_DDD.Database.DatabaseHandler.MeetingDatabaseFields;

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

    public static List<Meeting> CreateMeetingsFromUser(UserAccounts.User user, DatabaseHandler database)
    {
        //query for list of meeting ids that involve the user.
        List<string> meetingIds = database.QueryMultipleMeetings(DatabaseHandler.Database.Meetings,
            MeetingID, Sender, user.ID);
        //go through list, create meeting for each.
        List<Meeting> MyMeetings = new List<Meeting>();
        foreach (var meetingId in meetingIds)
        {
            string sender = database.QuerySingleMeetingField(meetingId, Sender);
            string recipent = database.QuerySingleMeetingField(meetingId, Recipent);
            string encodedDateTime = database.QuerySingleMeetingField(meetingId, Date);
            DateTime dateTime = DateTime.Parse(encodedDateTime);
            Meeting meeting = new(meetingId, sender, recipent,dateTime);
            MyMeetings.Add(meeting);
        }

        return MyMeetings;
    }
    public static List<Meeting> CreateMeetingsForUser(UserAccounts.User user, DatabaseHandler database)
    {
        //query for list of meeting ids that involve the user.
        List<string> meetingIds = database.QueryMultipleMeetings(DatabaseHandler.Database.Meetings,
            MeetingID, Recipent, user.ID);
        //go through list, create meeting for each.
        List<Meeting> MyMeetings = new List<Meeting>();
        foreach (var meetingId in meetingIds)
        {
            string sender = database.QuerySingleMeetingField(meetingId, Sender);
            string recipent = database.QuerySingleMeetingField(meetingId, Recipent);
            string encodedDateTime = database.QuerySingleMeetingField(meetingId, Date);
            DateTime dateTime = DateTime.Parse(encodedDateTime);
            Meeting meeting = new(meetingId, sender, recipent,dateTime);
            MyMeetings.Add(meeting);
        }
        return MyMeetings;
    }
}