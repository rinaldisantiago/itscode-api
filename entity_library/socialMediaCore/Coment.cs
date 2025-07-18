using Microsoft.VisualBasic;

public class Comment
{
    private int idComment;
    private int postId;
    private string? content;


    public int IdComment { get => this.idComment; set => this.idComment = value; }
    public string? Content { get => this.content; set => this.content = value; }
    public int PostId { get => this.postId; set => this.postId = value; }
}