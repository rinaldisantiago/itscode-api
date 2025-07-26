using entity_library;

public class Interaction
{
    private int id;
    private Post? post;
    private User? user;
    private InteractionType? interactionType;


    public int Id { get { return this.id; } set { this.id = value; } }
    public InteractionType? Type { get { return this.interactionType; } set { this.interactionType = value; } }

    public Post? Post { get { return this.post; } set { this.post = value; } }
    public User? User { get { return this.user; } set { this.user = value; } }
}