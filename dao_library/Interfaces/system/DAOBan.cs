using entity_library;

public interface DAOBan
{
    // Define methods for managing bans in the system
    void CreateBan(Ban ban);
    Ban? GetBanById(int id);
    IEnumerable<Ban> GetAllBans();
    void UpdateBan(Ban ban);
    void DeleteBan(int id);

    
}
    


