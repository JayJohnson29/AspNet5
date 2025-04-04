using MC;
using IndeService.Repository;

namespace IndeService.Service;
public class BatchService(ILogger<BatchService> logger, IBatchFileRepository batchFileRepository) : IBatchService
{

    public async Task<bool> PostFileAsync(IterationExtract extract)
    {

        var dto = new XmlFileDto
        {
            FileName = extract.ExtractName,
            FileData = Inde.ZipService.Compress(extract.ExtractData),
        };
        var body = Newtonsoft.Json.JsonConvert.SerializeObject(dto);

        if (extract.ExtractTypeId == 1)
        {
            var x = await batchFileRepository.SendLetterRequestAsync(body);
        }
        else if (extract.ExtractTypeId == 2)
        {
            var x = await batchFileRepository.SendXmlFile(body);
        }


        return true;
    }
}

