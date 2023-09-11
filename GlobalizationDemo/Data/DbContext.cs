namespace GlobalizationDemo.Data;

internal sealed class DbContext : IDbContext, IDisposable
{
    public DbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        Connection = new SqliteConnection(connectionString);
    }

    public IDbConnection Connection { get; }

    public void Dispose()
    {
        if (Connection.State is ConnectionState.Open)
        {
            Connection.Close();
        }

        Connection.Dispose();
    }
}
