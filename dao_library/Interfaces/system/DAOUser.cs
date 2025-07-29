using entity_library;

public interface DAOUser
{
    User? GetUser(int idUser);
    // List<User> GetAll();
    void CreateUser(User user);
    User UpdateUser(User user);
    void DeleteUser(int idUser);
    User? GetUserByEmail(string email);
    User GetUserByUsernameAndPassword(string userName, string password);
}

