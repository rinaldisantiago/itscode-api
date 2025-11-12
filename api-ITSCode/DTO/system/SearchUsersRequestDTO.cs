public class SearchUsersRequestDTO
{
    public string searchTerm { get; set; }
    public int idUserLogger { get; set; }
    public int pageNumber { get; set; }
    public int pageSize { get; set; }
}