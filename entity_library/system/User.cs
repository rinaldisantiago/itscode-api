using entity_library;

public class User : Person
{
    private string username = "";
    private string email = "";
    private string password = "";
    private Role? role;
    // private List<Post>? posts;



    public string Username
    { get { return this.username; } set { this.username = value; } }
    public string Email { get { return this.email; } set { this.email = value; } }
    public string Password { get { return this.password; } set { this.password = value; } }
    public Role? Role { get { return this.role; } set { this.role = value; } }

    // public List<Post>? Posts { get { return this.posts; } set { this.posts = value; } }

}
