using dao_library;

public class MDAOFactory : DAOFactory
{
    public DAOUser CreateDAOUser()
    {
        return new MockUserDAO();
    }

    public DAOPost CreateDAOPost()
    {
        return new MockPostDAO();
    }

    public DAORole CreateDAORole()
    {
        return new MockRoleDAO();
    }

    public DAOBan CreateDAOBan()
    {
        return new MockBanDAO();
    }

    public DAOFollowing CreateDAOFollowing()
    {
        return new MockFollowingDAO();
    }

    public DAOCommet CreateDAOComment()
    {
        return new MockCommentDAO();
    }

    
}