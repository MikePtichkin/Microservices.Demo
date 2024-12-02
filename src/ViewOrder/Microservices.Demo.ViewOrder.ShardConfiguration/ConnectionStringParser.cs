namespace Microservices.Demo.ViewOrder.ShardConfiguration;

public record ConnectionStringPostgres(string HostAndPort, string Database, string User, string? Password);

public static class ConnectionStringParser
{
    public static ConnectionStringPostgres Parse(string connectionString)
    {
        var parameters = connectionString.Split(';');
        string host = string.Empty;
        string port = string.Empty;
        string database = string.Empty;
        string user = string.Empty;
        string? password = null;

        foreach (var param in parameters)
        {
            var keyValue = param.Split('=');
            if (keyValue.Length != 2) continue;

            var key = keyValue[0].Trim();
            var value = keyValue[1].Trim();

            switch (key)
            {
                case "Host":
                    host = value;
                    break;
                case "Port":
                    port = value;
                    break;
                case "Database":
                    database = value;
                    break;
                case "Username":
                    user = value;
                    break;
                case "Password":
                    password = value == "null" ? null : value;
                    break;
            }
        }

        return new ConnectionStringPostgres($"{host}:{port}", database, user, password);
    }
}

