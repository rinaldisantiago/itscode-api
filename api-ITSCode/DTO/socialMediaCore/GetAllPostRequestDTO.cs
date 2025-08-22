public class GetAllPostResquestDTO
{
    public long pageSize { get; set; }
    public long pageNumber { get; set; }
    public string query { get; set; }
    public string orderBy { get; set; }
    public string direction { get; set;}
}