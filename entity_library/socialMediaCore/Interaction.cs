public class Interaction
{
    private int idInteraction;
    private int idPost;
    private int idPerson;
    private InteractionType type;
    private DateTime timestamp;

    public int IdInteraction { get { return this.idInteraction; } set { this.idInteraction = value; } }
    public InteractionType Type { get { return this.type; } set { this.type = value; } }
    public DateTime Timestamp { get { return this.timestamp; } set { this.timestamp = value; } }
    public int IdPost { get { return this.idPost; } set { this.idPost = value; } }
    public int IdPerson { get { return this.idPerson; } set { this.idPerson = value; } }
}