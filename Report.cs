namespace CWRK_DDD;

public class Report
{
    public string ReportID;
    public string ReporterId;
    public string ReportInfo;
    public DateTime reportDateTime;


    public Report(string reportId,string reporterID, string reportInfo, DateTime dateTime)
    {
        ReportID = reportId;
        ReporterId = reporterID;
        ReportInfo = reportInfo;
        reportDateTime = dateTime;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Report - {ReportID}\r\nFrom {ReporterId} at {reportDateTime}\r\n{ReportInfo}";
    }
}