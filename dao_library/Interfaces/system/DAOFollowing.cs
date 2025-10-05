using entity_library;

public interface DAOFollowing
{
    // Define methods for managing user followings in the system
    void CreateFollowing(Following following);
    List<Following> GetAllFollowings();
    List<int> GetFollowedUserIds(int userId);
    void DeleteFollowing(int idUserFollowing, int idUserFollowed);
}
    


