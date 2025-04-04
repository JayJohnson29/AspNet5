using MC;

namespace IndeService.Repository
{
    public interface IBatchFileRepository
    {
        Task<Response<string>> SendLetterRequestAsync(string body);
        Task<Response<string>> SendXmlFile(string body);
    }
}