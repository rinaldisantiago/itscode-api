public interface DAOComment
{
    // Define methods for managing comments in the system
    void CreateComment(Comment comment);
    Comment? GetCommentById(int id);

    List<Comment> GetCommentsByPostId(int postId);

    void DeleteComment(int id);
    
}