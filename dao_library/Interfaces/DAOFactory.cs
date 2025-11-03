public interface DAOFactory
{
    DAOUser CreateDAOUser();
    DAOPost CreateDAOPost();
    DAORole CreateDAORole();
    DAOFollowing CreateDAOFollowing();
    DAOImage CreateDAOImage();
    DAOFile CreateDAOFile();
    DAOComment CreateDAOComment();
    DAOInteraction CreateDAOInteraction();
    DAOBan CreateDAOBan();
}