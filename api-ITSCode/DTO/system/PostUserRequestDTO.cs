using Microsoft.AspNetCore.Http; 

public class PostUserRequestDTO
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int? RoleId { get; set; }
    public string? URLAvatar { get; set; } = "";
    public IFormFile? Image { get; set; } 
}