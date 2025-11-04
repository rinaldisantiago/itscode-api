using entity_library;

public class User : Person
{
    private string userName = "";
    private string email = "";
    private string password = "";
    private Role? role;
    private Image? avatar;
    private bool isBanned = false;



    public string UserName { get { return this.userName; } set { this.userName = value; } }
    public string Email { get { return this.email; } set { this.email = value; } }
    public bool IsBanned { get { return this.isBanned; } set { this.isBanned = value; } }
    public string Password { get { return this.password; } set { this.password = value; } }
    public virtual Role? Role { get { return this.role; } set { this.role = value; } }
    public virtual Image? Avatar { get { return this.avatar; } set { this.avatar = value; } }


    public string GetRole
    {
        get
        {

            if (this.Role != null)
            {
                return this.Role.Name;
            }
            return "No role assigned";
        }
    }

    public string GetAvatar()
    {
            if (this.Avatar != null)
            {
                return this.Avatar.Url;
            }
            return "No avatar assigned";
    }

    public string encript(string password)
    {
        return password + "123456";
    }
    public void SetPassword(string password)
    {
        this.password = this.encript(password);
    }
    public bool IsPasswordValid(string password)
    {
        string hashPassword = this.encript(password);
        return hashPassword == this.Password;
    }
    

}
