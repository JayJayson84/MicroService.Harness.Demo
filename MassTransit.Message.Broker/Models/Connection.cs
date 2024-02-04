namespace MassTransit.Message.Broker.Models;

internal static class Connection
{
    static Connection()
    {
        Credentials = new Credentials(
            Resources.Settings.Connection.Username,
            Resources.Settings.Connection.Password
        );
    }

    public static string Host => string.IsNullOrEmpty(VirtualHost) || VirtualHost.Equals("/") ? $"rabbitmq://{HostName}" : $"rabbitmq://{HostName}/{VirtualHost}";
    public static string HostName => Resources.Settings.Connection.HostName;
    public static string VirtualHost => Resources.Settings.Connection.VirtualHost;
    public static string Queue => Resources.Settings.Connection.Queue;
    public static Credentials Credentials { get; }
}
