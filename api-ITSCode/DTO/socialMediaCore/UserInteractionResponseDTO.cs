public class UserInteractionResponseDTO
{
    public int? interactionId { get; set; }       // null si no hay interacción
    public int? type { get; set; }                // 1=Like, 2=Dislike, null=Sin interacción
}