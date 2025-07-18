using entity_library;

public interface DAOCommet
{
    // Define methods for managing comments in the system
    void CreateComment(Comment comment);
    Comment? GetCommentById(int id);
    IEnumerable<Comment> GetAllComments();
    void UpdateComment(Comment comment);
    void DeleteComment(int id);
    void Save(Comment comment);
    
}