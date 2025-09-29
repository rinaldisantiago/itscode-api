public class EFDAOFactory : DAOFactory
{
    private AppDbContext appDbContext;
    public EFDAOFactory(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public DAOFile CreateDAOFile()
    {
        throw new NotImplementedException();
    }

    public DAOFollowing CreateDAOFollowing()
    {
        throw new NotImplementedException();
    }

    public DAOImage CreateDAOImage()
    {
        throw new NotImplementedException();
    }

    public DAOPost CreateDAOPost()
    {
        return new EFDAOPost(this.appDbContext);    
    }

    public DAORole CreateDAORole()
    {
        throw new NotImplementedException();
    }

    public DAOUser CreateDAOUser()
    {
        return new EFDAOUser(this.appDbContext);
    }
}