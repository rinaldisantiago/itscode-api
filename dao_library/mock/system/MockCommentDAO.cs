public class MockCommentDAO : DAOComment
{
    public void CreateComment(Comment comment)
    {
        throw new NotImplementedException();
    }

    public void DeleteComment(int id)
    {
        throw new NotImplementedException();
    }

    public Comment? GetCommentById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Comment> GetCommentsByPostId(int postId)
    {
        throw new NotImplementedException();
    }
}