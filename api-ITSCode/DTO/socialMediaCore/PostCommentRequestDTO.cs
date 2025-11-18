public class PostCommentRequestDTO
{
    public int postId { get; set; }
    public int userId { get; set; }
    public string? content { get; set; } = "";
   
}