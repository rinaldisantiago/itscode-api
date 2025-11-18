using Microsoft.EntityFrameworkCore;

public class EFDAOComment : DAOComment
{
    private AppDbContext dbContext;
    public EFDAOComment(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateComment(Comment comment)
    {
        this.dbContext.Coments.Add(comment);
        dbContext.SaveChanges();

    }

    public Comment? GetCommentById(int id)
    {
        return dbContext.Coments

            .FirstOrDefault(c => c.Id == id);
    }

    public List<Comment> GetCommentsByPostId(int postId, int pageNumber, int pageSize)
    {
        return dbContext.Coments
            .Where(c => c.Post != null && c.Post.Id == postId)
            .Skip((pageNumber - 1) * pageSize)  
            .Take(pageSize)
            .ToList();
    }

    public void DeleteComment(int id)
    {
        this.dbContext.Coments.Where(comment => comment.Id == id).ExecuteDelete();
    }

    public void UpdateComment(Comment comment)
    {
        var existingComment = dbContext.Coments.Find(comment.Id);
        if (existingComment != null)
        {
            existingComment.Content = comment.Content;
            dbContext.SaveChanges();
        }
    }
}