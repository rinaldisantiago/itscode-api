public class GetUserResponseDTO
{
    public int id { get; set; }
    public string fullName { get; set; } = "";
    public string userName { get; set; } = "";
    public string email { get; set; } = "";
    public string urlAvatar { get; set; } = "";
    public bool isFollowing { get; set; }
}
