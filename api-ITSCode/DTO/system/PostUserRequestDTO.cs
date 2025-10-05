public class PostUserRequestDTO
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int? RoleId { get; set; } = 0;
    public string? URLAvatar { get; set; } = "";
   
}