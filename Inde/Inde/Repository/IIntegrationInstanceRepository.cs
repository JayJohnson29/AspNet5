namespace Inde.Repository;

public interface IIntegrationInstanceRepository
{
     Task<List<IntegrationInstanceConfiguration>> GetAsync();
}