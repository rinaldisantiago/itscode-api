using entity_library;

public interface DAOBan
{
    void CreateBan(Ban ban);
    Ban? GetBanById(int idBan);
    List<Ban> GetBans(int pageNumber, int pageSize);
    void UpdateBan(Ban ban);
    void DeleteBan(int idBan);
    Ban? GetBanByUserId(User user);

    
}
    


