using entity_library;

public class Interaction
{
    private int id;
    private Post post;
    private User user;
    private int postId;
    private int userId;
    private InteractionType interactionType { get; set; }


    public int PostId { get; set; }
    public int UserId { get; set; }


    public int Id { get { return this.id; } set { this.id = value; } }
    public virtual InteractionType InteractionType { get { return this.interactionType; } set { this.interactionType = value; } }

    public virtual Post Post { get { return this.post; } set { this.post = value; } }
    public virtual User User { get { return this.user; } set { this.user = value; } }
}

