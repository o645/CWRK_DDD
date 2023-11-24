namespace CWRK_DDD;

public class Report
{
    public UserAccounts.User reporter;
    public string info;


    public Report(UserAccounts.User reporter, string info)
    {
        this.reporter = reporter;
        this.info = info;
    }
}