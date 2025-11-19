public class UpdatePostRequestDTO
{
    public int id { get; set; }
    public int idUser { get; set; }
    public string? title { get; set; } = "";
    public string? content { get; set; } = "";
    public string? fileUrl { get; set; }

}