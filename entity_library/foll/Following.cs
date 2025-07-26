using entity_library;

public class Following
{
    private int id;
    private User userFollowing;
    private User userFollowed;


    public int Id { get { return this.id; } set { this.id = value; } }
    public User UserFollowing { get { return this.userFollowing; } set { this.userFollowing = value; } }
    public User UserFollowed { get { return this.userFollowed; } set { this.userFollowed = value; } }


}