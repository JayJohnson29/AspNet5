using MC;

namespace IndeService.Service;

public interface IBatchService
{
    Task<bool> PostFileAsync(IterationExtract extract);
}