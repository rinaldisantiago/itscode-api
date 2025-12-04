using Microsoft.AspNetCore.Http; // Necesario para IFormFile

public class PostPostRequestDTO
{
    public int idUser { get; set; }
    public string? title { get; set; } = "";
    public string? content { get; set; } = "";
    public string? fileUrl { get; set; }
    public IFormFile? File { get; set; } 
}