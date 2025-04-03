namespace IndeService;

public class McLoginResponse
{
    public McUser Data { get; set; }
    public string Message { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}