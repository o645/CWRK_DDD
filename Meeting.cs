namespace CWRK_DDD;

public class Meeting
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
    
}