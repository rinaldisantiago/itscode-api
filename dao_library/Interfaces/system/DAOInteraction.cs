using entity_library;

public interface DAOInteraction
{
    void CreateInteraction(Interaction interaction);
    Interaction? GetInteractionById(int id);
    void DeleteInteraction(int id);

    Interaction? GetInteractionByPostAndUser(int postId, int userId);
}