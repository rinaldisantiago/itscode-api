using Microsoft.EntityFrameworkCore;

public class EFDAOFollowing : DAOFollowing
{
    private AppDbContext dbContext;
    public EFDAOFollowing(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public void CreateFollowing(Following following)
    {
        this.dbContext.Followings.Add(following);
        dbContext.SaveChanges();
    }

    public void DeleteFollowing(int idUserFollowing, int idUserFollowed)
    {
        this.dbContext.Followings.Where(following => following.UserFollowing.Id == idUserFollowing && following.UserFollowed.Id == idUserFollowed).ExecuteDelete();
    }

    public List<Following> GetAllFollowings()
    {
        return this.dbContext.Followings.ToList();
    }

    public List<int> GetFollowedUserIds(int userId)
    {
        throw new NotImplementedException();
    }
}