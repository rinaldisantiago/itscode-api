public class PutUserRequestDTO
{
    public int id { get; set; }
    public string? fullName { get; set; } = "";
    public string? userName { get; set; } = "";
    public string? email { get; set; } = "";
    public string? password { get; set; } = "";
    public string? urlAvatar { get; set; } = "";    
    public IFormFile? image { get; set; }
}
