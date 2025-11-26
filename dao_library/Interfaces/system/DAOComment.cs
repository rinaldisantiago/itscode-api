public interface DAOComment
{
    // Define methods for managing comments in the system
    void CreateComment(Comment comment);
    Comment? GetCommentById(int id);

    List<Comment> GetCommentsByPostId(int postId, int pageNumber, int pageSize);
    List<Comment> GetCommentsByUserId(int userId, int pageNumber, int pageSize);

    void DeleteComment(int id);
    void UpdateComment(Comment comment);

    
}