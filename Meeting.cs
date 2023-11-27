namespace CWRK_DDD;

public class Meeting : IComparable<Meeting>
{
    public string MeetingID;
    public string SenderId;
    public string RecipentId;
    public DateTime MeetingTime;
    public string MeetingName;


    public Meeting(string meetingId,string senderID, string recipentID, string meetingName, DateTime meetingTime)
    {
        MeetingID = meetingId;
        SenderId = senderID;
        RecipentId = recipentID;
        MeetingName = meetingName;
        MeetingTime = meetingTime;
    }

    /// <summary>
    /// Compare two meetings together, listing them in date order.
    /// </summary>
    public int CompareTo(Meeting? other)
    {
        return MeetingTime.CompareTo(other.MeetingTime);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Meeting {MeetingID}\r\n{SenderId} to {RecipentId}\r\nAt {MeetingTime.ToString()}";
    }
}