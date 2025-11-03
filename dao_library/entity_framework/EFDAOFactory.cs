public class EFDAOFactory : DAOFactory
{
    private AppDbContext appDbContext;
    public EFDAOFactory(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public DAOFile CreateDAOFile()
    {
        return new EFDAOFile(this.appDbContext);
    }

    public DAOFollowing CreateDAOFollowing()
    {
        return new EFDAOFollowing(this.appDbContext);
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
        return new EFDAORole(this.appDbContext);
    }

    public DAOUser CreateDAOUser()
    {
        return new EFDAOUser(this.appDbContext);
    }

    public DAOComment CreateDAOComment()
    {
        return new EFDAOComment(this.appDbContext);
    }
    public DAOInteraction CreateDAOInteraction()
    {
        return new EFDAOInteraction(this.appDbContext);
    }

    public DAOBan CreateDAOBan()
    {
        return new EFDAOBan(this.appDbContext);
    }
}