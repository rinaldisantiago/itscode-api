using entity_library;

public interface DAOFollowing
{
    // Define methods for managing user followings in the system
    void CreateFollowing(Following following);
    Following? GetFollowingById(int id);
    List<Following> GetAllFollowings();
    List<int> GetFollowedUserIds(int userId);
    void UpdateFollowing(Following following);
    void DeleteFollowing(int id);
}
    


