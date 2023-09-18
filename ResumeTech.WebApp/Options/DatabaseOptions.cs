using ResumeTech.Common.Utility;

namespace ResumeTech.Application.Options; 

public class DatabaseOptions {
    private const string ConnectionStringFormat = "Host={0};Port=5432;Database={1};Username={2};Password={3};Include Error Detail=true";

    public string Host { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string ToConnectionString() {
        var host = Host.OrElseThrow("Missing Database Host");
        var name = Name.OrElseThrow("Missing Database Name");
        var username = Username.OrElseThrow("Missing Database Username");
        var password = Password.OrElseThrow("Missing Database Password");
        return string.Format(ConnectionStringFormat, host, name, username, password);
    }
}