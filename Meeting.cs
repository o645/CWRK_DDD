namespace CWRK_DDD;

public class Meeting : IComparable<Meeting>
{
    public string MeetingID;
    public string SenderId;
    public string RecipentId;
    public DateTime MeetingTime;


    public Meeting(string meetingId,string senderID, string recipentID, DateTime meetingTime)
    {
        MeetingID = meetingId;
        SenderId = senderID;
        RecipentId = recipentID;
        MeetingTime = meetingTime;
    }

    /// <summary>
    /// Compare two meetings together, listing them in date order.
    /// </summary>
    public int CompareTo(Meeting? other)
    {
        return MeetingTime.CompareTo(other.MeetingTime);
    }
}