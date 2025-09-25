public class GetPostResponseDTO
{
    public int idUser { get; set; }
    public string title { get; set; } = "";
    public string content { get; set; } = "";

    public int likes { get; set; } = 0;
    public int dislikes { get; set; } = 0;
    public int commentsCount { get; set; } = 0;
    public string fileUrl { get; set; } = "";
    public string userName { get; set; } = "";
    public string userAvatar { get; set; } = "";

    public List<object> comments { get; set; } 

}
