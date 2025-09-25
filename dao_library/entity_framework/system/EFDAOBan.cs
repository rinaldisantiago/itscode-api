
public class EFDAOBan : DAOBan
{
    private AppDbContext dbContext;
    public EFDAOBan(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateBan(Ban ban)
    {
        throw new NotImplementedException();
    }

    public void DeleteBan(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Ban> GetAllBans()
    {
        throw new NotImplementedException();
    }

    public Ban? GetBanById(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateBan(Ban ban)
    {
        throw new NotImplementedException();
    }
}