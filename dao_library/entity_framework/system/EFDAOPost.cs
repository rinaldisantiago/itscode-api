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


    // public List<Post> GetPosts(int? idUserConsultado, int? idUserLogger, bool isMyPosts)
    // {
    //     throw new NotImplementedException();
    // }

    public List<Post> GetPosts(int idUserConsultado, int idUserLogger, bool isMyPosts)
    {
        IQueryable<Post> query = this.dbContext.Posts
            .Include(p => p.User).ThenInclude(u => u.Avatar)  
            .Include(p => p.File)                  
            .Include(p => p.Comments)             
            .Include(p => p.Interactions);        

        if (idUserLogger != 0)
        {
            if (isMyPosts)
            {
                
                query = query.Where(p => p.User.Id == idUserLogger);
            }
            else
            {
               
                var followingIds = this.dbContext.Followings
                    .Where(f => f.UserFollowing.Id == idUserLogger)
                    .Select(f => f.UserFollowed.Id);


                query = query.Where(p => followingIds.Contains(p.User.Id));
            }
        }
        else if (idUserConsultado != 0)
        {
            
            query = query.Where(p => p.User.Id == idUserConsultado);
        }

        return query
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
    }



    public void UpdatePost(Post post)
    {
        this.dbContext.Posts.Update(post);
    }
}