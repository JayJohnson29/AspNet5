namespace IndeService.Repository;

public interface IIntegrationInstanceRepository
{
     Task<List<IntegrationInstanceConfiguration>> GetAsync();
}