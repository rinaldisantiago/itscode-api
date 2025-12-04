using entity_library;

public interface DAOFollowing
{
    void CreateFollowing(Following following);
    List<Following> GetAllFollowings();
    List<int> GetFollowedUserIds(int userId);
    void DeleteFollowing(int idUserFollowing, int idUserFollowed);
}
    


