public class GetAllPostRequestDTO
{
    public int idUserLogger { get; set; } = 0;
    public int idUserConsultado { get; set; } = 0;
    //falta token
    public bool isMyPosts { get; set; }
    public int pageNumber { get; set; }

}