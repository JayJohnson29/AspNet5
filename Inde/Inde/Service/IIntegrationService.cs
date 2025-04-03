namespace Inde.Service;

public interface IIntegrationService
{
    Task<bool> RunAsync(CancellationToken cancellationToken);
}