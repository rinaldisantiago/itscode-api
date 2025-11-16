using Microsoft.AspNetCore.Http; 

public class PostUserRequestDTO
{
    public string fullName { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public int? roleId { get; set; } = 2;
    public string? urlAvatar { get; set; } = "";
    public IFormFile? image { get; set; } 
}