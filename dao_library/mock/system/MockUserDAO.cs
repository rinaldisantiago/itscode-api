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
            MockRoleDAO roleDAO = new MockRoleDAO();
            MockImageDAO imageDAO = new MockImageDAO();


            _users.Add(new User { Id = 1, FullName = "Herhandez Carlos", UserName = "Carlos", Email = "carlos@example.com", Password = "password123456", Role = roleDAO.GetRoleById(1), Avatar = imageDAO.GetImage("https://img2.wallspic.com/previews/4/1/9/4/7/174914/174914-dragon_ball-goku-cartel-saiyajin-super_saiyajin-x750.jpg") });
            _users.Add(new User { Id = 2, FullName = "Hernandez Joaquin", UserName = "Joaco", Email = "joaco@example.com", Password = "password456", Role = roleDAO.GetRoleById(2), Avatar = imageDAO.GetImage("http://example.com/image2.jpg") });
        }

        

        public User GetUser(int idUser)
        {
            return _users.FirstOrDefault(u => u.Id == idUser);
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }


        public void CreateUser(User user)
        {
            user.Id = _users.Max(u => u.Id) + 1;
            _users.Add(user);
        }

        public User UpdateUser(User user)
        {
            User existingUser = GetUser(user.Id);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Avatar.Url = user.Avatar.Url;
            }
            return existingUser;
        }

        public void DeleteUser(int idUser)
        {
            User user = GetUser(idUser);
            if (user != null)
            {
                _users.Remove(user);
            }
        }
        
        public User GetUserByUsernameAndPassword(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            return _users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }
    }
}
