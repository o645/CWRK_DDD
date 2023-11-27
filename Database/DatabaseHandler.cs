using System.Data.SQLite;
using System.IO.Compression;

namespace CWRK_DDD.Database;

/// <summary>
/// Class for handling all requests to the database.
/// </summary>
public class DatabaseHandler
{
    /// <summary>
    /// Connection to the SQLite database.
    /// </summary>
    private SQLiteConnection _connection;

    /// <summary>
    /// Create a new Database Handler.
    /// </summary>
    /// <param name="connectionString">SQLite connection string.</param>
    public DatabaseHandler(string connectionString)
    {
        _connection = new SQLiteConnection(connectionString);
        _connection.Open();
    }

    /// <summary>
    /// Closes the connection on deconstruction.
    /// </summary>
    public void Deconstruct()
    {
        _connection.Close();
    }

    /// <summary>
    /// Enums for the difference databases.
    /// </summary>
    public enum Database
    {
        Users,
        Meetings,
        Reports,
        AssignedUsers
    }
    
    /// <summary>
    /// Status of a Query that doesn't return.
    /// </summary>
    public enum QueryStatus
    {
        Failed,
        Success
    }
    

    /// <summary>
    /// Fields in the User table
    /// </summary>
    public enum UserDatabaseFields
    {
        UserID,
        Name,
        Password,
        AccountType
    }

    /// <summary>
    /// Fields in the Meetings table
    /// </summary>
    public enum MeetingDatabaseFields
    {
        MeetingID,
        Sender,
        Recipent,
        Date
    }

    /// <summary>
    /// Fields in the Reports table
    /// </summary>
    public enum ReportDatabaseFields
    {
        ReportID,
        ReporterID,
        ReportInfo,
        DateOfReport
    }

    /// <summary>
    /// Fields in the User Assignment table
    /// </summary>
    public enum AssignmentDatabaseFields
    {
        AssignmentID,
        UserA,
        UserB,
        Type
    }

    /// <summary>
    /// Query the User table for a single result using the UserID as a primary key.
    /// </summary>
    /// <param name="PrimaryKey">User's ID</param>
    /// <param name="Field">Field to return value from</param>
    /// <returns>Value of field.</returns>
    public string QuerySingleUserField(string PrimaryKey, UserDatabaseFields Field) =>
        QuerySingleField(Database.Users,UserDatabaseFields.UserID.ToString(), PrimaryKey, Field.ToString());
    public string QuerySingleMeetingField(string PrimaryKey, MeetingDatabaseFields Field) =>
        QuerySingleField(Database.Meetings,MeetingDatabaseFields.MeetingID.ToString(), PrimaryKey, Field.ToString());
    public string QuerySingleReportField(string PrimaryKey, ReportDatabaseFields Field) =>
        QuerySingleField(Database.Reports,ReportDatabaseFields.ReportID.ToString(), PrimaryKey, Field.ToString());
    
    /// <summary>
    /// Sends a SELECT Query to a database.
    /// </summary>
    /// <param name="databaseWanted">Database to perform command on</param>
    /// <param name="PrimaryKeyField">Name of primary key field.</param>
    /// <param name="PrimaryKey">Value of primary key to search by</param>
    /// <param name="Field">Field to return value of.</param>
    /// <returns>Value of field for that primary key.</returns>
    public string QuerySingleField(Database databaseWanted, string PrimaryKeyField, string PrimaryKey, string Field)
    {
        var command = _connection.CreateCommand(); 
        command.CommandText =
                    $@"SELECT {Field} FROM {databaseWanted.ToString()} WHERE {PrimaryKeyField} = '{PrimaryKey}'";
        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var Value = reader.GetValue(0);
            return Value.ToString();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns multiple matching fields
    /// </summary>
    /// <param name="database">Database to perform operation on</param>
    /// <param name="wantedField">Field to query</param>
    /// <param name="keyField">Key's field</param>
    /// <param name="key">Key's value</param>
    /// <returns></returns>
    public List<string> QueryMultiple(Database database, string wantedField, string keyField, string key)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $@"SELECT {wantedField} FROM {database.ToString()} WHERE {keyField} = {key}";
        var reader = command.ExecuteReader();
        List<string> results = new();
        while (reader.Read())
        {
            var Value = reader.GetValue(0);
            results.Add(Value.ToString());
        }

        return results;
    }
    
    public bool QueryForExistance(Database databaseWanted,string primaryKeyField, string primaryKey)
    {
        var command = _connection.CreateCommand();
        command.CommandText =
            $@"SELECT count(*) FROM {databaseWanted.ToString()} WHERE {primaryKeyField} = '{primaryKey}'";
        var reader = command.ExecuteScalar().ToString();
        if (reader is not null && reader is string result)
        {
            return int.Parse(result) > 0;
        }

        return false;
    }

    public int QueryCount(Database database, string primaryKeyField, string primaryKey)
    {
        //TODO: Return Count
        return 0;
        
    }

    public QueryStatus AddNewUser(UserAccounts.User user)
    {
        return AddNewUser(user.ID, user.Name, user._hashedPassword, user.AccountType);
    }

    public QueryStatus RemoveUser(string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $@"DELETE FROM Users WHERE UserID = '{userId}';";
        try
        {
            command.ExecuteNonQuery();
            return QueryStatus.Success;
        }
        catch
        {
            return QueryStatus.Failed;
        }
    }

    public QueryStatus AddNewUser(string userid, string name, string password, UserAccounts.AccountType accountType)
    {
        var command = _connection.CreateCommand();
        
        command.CommandText = $@"INSERT INTO Users VALUES ('{userid}','{name}','{password}','{(int)accountType}')";
        try
        {
            command.ExecuteNonQuery();
            return QueryStatus.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return QueryStatus.Failed;
        }
    }
    
    public QueryStatus EditDatabase(Database database, string PrimaryKeyField,string PrimaryKey, string Field, string NewValue)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $@"UPDATE {database.ToString()} SET {Field} = '{NewValue}' WHERE {PrimaryKeyField} = '{PrimaryKey}'";
        try
        {
            command.ExecuteNonQuery();
            return QueryStatus.Success;
        }
        catch
        {
            return QueryStatus.Failed;
        }
    }
}