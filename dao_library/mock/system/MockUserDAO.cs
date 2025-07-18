using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockUserDAO : DAOUser
    {
        private readonly List<User> _users = new List<User>();

        public MockUserDAO()
        {
            // Usuarios de prueba
            _users.Add(new User { IdPerson = 1, FullName = "Herhandez Carlos", Username = "Carlos", Email = "carlos@example.com", Password = "password123", Role = new Role { IdRole = 1 } });
            _users.Add(new User { IdPerson = 2, FullName = "Hernandez Joaquin", Username = "Joaco", Email = "joaco@example.com", Password = "password456", Role = new Role { IdRole = 2 } });
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetUser(int idUser)
        {
            return _users.FirstOrDefault(u => u.IdPerson == idUser);
        }

        public void Save(User user)
        {
            user.IdPerson = _users.Max(u => u.IdPerson) + 1;
            _users.Add(user);
        }

        public void CreateUser(User user)
        {
            Save(user);
        }

        public void UpdateUser(User user)
        {
            var existingUser = GetUser(user.IdPerson);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
            }
        }
        
        public void DeleteUser(int idUser)
        {
            var user = GetUser(idUser);
            if (user != null)
            {
                _users.Remove(user);
            }
        }
    }
}
