namespace GlobalizationDemo.Data;

public interface IDbContext
{
    IDbConnection Connection { get; }
}
