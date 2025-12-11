using MySql.Data.MySqlClient;
using MySqlConnector;
using System;

public class DBConnection
{
    private DBConnection()
    {
    }

    public string Server { get; set; }
    public string DatabaseName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public MySql.Data.MySqlClient.MySqlConnection Connection { get; set; }

    private static DBConnection _instance = null;
    public static DBConnection Instance()
    {
        if (_instance == null)
            _instance = new DBConnection();
        return _instance;
    }

    public bool IsConnected()
    {
        if (Connection == null)
        {
            if (String.IsNullOrEmpty(DatabaseName))
                return false;
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
            Connection = new MySql.Data.MySqlClient.MySqlConnection(connstring);
            Connection.Open();
        }

        return true;
    }

    public void Close()
    {
        Connection.Close();
    }
}