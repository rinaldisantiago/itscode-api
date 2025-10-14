public class UserInteractionResponseDTO
{
    public int? InteractionId { get; set; }       // null si no hay interacción
    public int? Type { get; set; }                // 1=Like, 2=Dislike, null=Sin interacción
}