namespace IndeService.Service;
public class BatchService(ILogger<BatchService> logger, HttpClient httpClient)
{
    public async Task<bool> PostLetterRequestAsync()
    {
        return true;
    }

    public async Task<bool> PostXmlFileAsync()
    {

        return true;
    }

}

