namespace IndeService.Service;

public interface IIntegrationService
{
    Task<bool> RunAsync(CancellationToken cancellationToken);
}