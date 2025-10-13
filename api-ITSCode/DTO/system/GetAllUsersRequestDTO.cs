public class GetAllUsersRequestDTO
{
    public int idUserLogger { get; set; }
    public string searchTerm { get; set; } = "";
    public int pageNumber { get; set; } = 0;
    public int pageSize { get; set; } = 0;
}