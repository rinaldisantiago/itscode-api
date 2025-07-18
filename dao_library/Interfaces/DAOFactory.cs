public interface DAOFactory
{
    DAOUser CreateDAOUser();
    DAOPost CreateDAOPost();
    DAORole CreateDAORole();
    DAOBan CreateDAOBan();
    DAOFollowing CreateDAOFollowing();
    DAOCommet CreateDAOComment();
    
}