public class CommentDTO
{
    public int id { get; set; }
    public int userId { get; set; }
    public int postId { get; set; }
    public string content { get; set; }
    public DateTime createdAt { get; set; }
}