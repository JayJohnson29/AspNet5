namespace Sms.Service;

public interface IDatabaseRestoreService
{
    Task<Tuple<bool, int>> RunAsync(AppConfig config);
}