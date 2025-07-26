using entity_library;

public interface DAOFollowing
{
    // Define methods for managing user followings in the system
    void CreateFollowing(Following following);
    Following? GetFollowingById(int id);
    IEnumerable<Following> GetAllFollowings();
    void UpdateFollowing(Following following);
    void DeleteFollowing(int id);
}
    


