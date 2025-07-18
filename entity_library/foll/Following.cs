using entity_library;

public class Following
{
    private int idFollowing;
    private int followerId;
    private int followedId;
    private User? user;


    public int IdFollowing { get { return this.idFollowing; } set { this.idFollowing = value; } }
    public int FollowerId { get { return this.followerId; } set { this.followerId = value; } }
    public int FollowedId { get { return this.followedId; } set { this.followedId = value; } }
    public User? User { get { return this.user; } set { this.user = value; } }

}