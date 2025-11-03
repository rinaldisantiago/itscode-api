
using Microsoft.EntityFrameworkCore;

public class EFDAOBan : DAOBan
{
    private AppDbContext dbContext;
    public EFDAOBan(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateBan(Ban ban)
    {
        this.dbContext.Bans.Add(ban);
        dbContext.SaveChanges();
    }

    public void DeleteBan(int idBan)
    {
        this.dbContext.Bans.Where(ban => ban.Id == idBan).ExecuteDelete();
    }

    public List<Ban> GetBans(int pageNumber, int pageSize)
    {
        IQueryable<Ban> query = this.dbContext.Bans;

        return query.OrderByDescending(b => b.BanDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
    }

    public Ban? GetBanById(int idBan)
    {
        return this.dbContext.Bans.FirstOrDefault(ban => ban.Id == idBan);
    }

    public void UpdateBan(Ban ban)
    {
        throw new NotImplementedException();
    }
}