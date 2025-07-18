using entity_library;

public interface DAOUser
{
    User? GetUser(int idUser);
    List<User> GetAll();
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int idUser);
    void Save(User user);
}

