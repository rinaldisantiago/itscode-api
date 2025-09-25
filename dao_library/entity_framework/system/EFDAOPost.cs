using entity_library;
using Microsoft.EntityFrameworkCore;

public class EFDAOPost : DAOPost
{
    private AppDbContext dbContext;
    public EFDAOPost(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreatePost(Post post)
    {
        this.dbContext.Posts.Add(post);
        dbContext.SaveChanges();
    }

    public void DeletePost(int id)
    {
        this.dbContext.Posts.Where(post => post.Id == id).ExecuteDelete();
    }

    public Post? GetPostById(int id)
    {
        return this.dbContext.Posts.FirstOrDefault(post => post.Id == id);
    }


    public List<Post> GetPosts(int? idUserConsultado, int? idUserLogger, bool isMyPosts)
    {
        throw new NotImplementedException();
    }

    public void UpdatePost(Post post)
    {
        this.dbContext.Posts.Update(post);
    }
}