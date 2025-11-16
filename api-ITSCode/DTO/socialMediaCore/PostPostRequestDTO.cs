using Microsoft.AspNetCore.Http; // Necesario para IFormFile

public class PostPostRequestDTO
{
    public int idUser { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public string? fileUrl { get; set; }
    
    // 👇 CAMBIO CLAVE: Propiedad para recibir el archivo del formulario
    public IFormFile? File { get; set; } // El nombre 'File' debe coincidir con el 'name' del input HTML
}