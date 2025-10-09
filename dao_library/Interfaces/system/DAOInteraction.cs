using entity_library;

public interface DAOInteraction
{
    // Define methods for managing interactions in the system
    Interaction CreateInteraction(Interaction interaction);
    Interaction? GetInteractionById(int id);
    void DeleteInteraction(int id);

    Interaction? GetInteractionByPostAndUser(int postId, int userId);
}