using entity_library;

public interface DAOUser
{
    User? GetUser(int idUser);
    // List<User> GetAll();
    void CreateUser(User user);
    User UpdateUser(User user);
    void DeleteUser(int idUser);
    bool GetUserByUsername(string username);
    bool GetUserByEmail(string email);
    User Login(string userName);
    List<User> GetSugerencias(int idUserLogger, int page, int pageSize, List<int> followingIds);
    List<User> SearchUsers(string searchTerm, int idUserLogger, int pageNumber, int pageSize);
    List<User> GetUsers( int pageNumber, int pageSize);
    bool PutUserRole(int idUser, int idRole);
}
    

