using entity_library;

public interface DAOBan
{
    // Define methods for managing bans in the system
    void CreateBan(Ban ban);
    Ban? GetBanById(int idBan);
    List<Ban> GetBans(int pageNumber, int pageSize);
    void UpdateBan(Ban ban);
    void DeleteBan(int idBan);
    Ban? GetBanByUserId(int userId);

    
}
    


