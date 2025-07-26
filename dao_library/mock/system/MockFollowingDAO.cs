using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockFollowingDAO : DAOFollowing
    {
        private  List<Following> _Followings = new List<Following>();

        public MockFollowingDAO()
        {
            MockUserDAO userDAO = new MockUserDAO();
            User user1 = userDAO.GetUser(1);
            User user2 = userDAO.GetUser(2);
            // Seguimientos de prueba
            _Followings.Add(new Following { Id = 1, UserFollowed = user1, UserFollowing = user2 });
            _Followings.Add(new Following { Id = 2, UserFollowed = user2, UserFollowing = user1 });
        }


        public Following GetFollowingById(int id)
        {
            return _Followings.FirstOrDefault(f => f.Id == id);
        }

        public void UpdateFollowing(Following following)
        {
            var existingFollowing = GetFollowingById(following.Id);
            if (existingFollowing != null)
            {
                existingFollowing.UserFollowed = following.UserFollowed;
                existingFollowing.UserFollowing = following.UserFollowing;
            }
        }

        public void DeleteFollowing(int id)
        {
            var following = GetFollowingById(id);
            if (following != null)
            {
                _Followings.Remove(following);
            }
        }

        public IEnumerable<Following> GetAllFollowings()
        {
            return _Followings;
        }

        public void CreateFollowing(Following following)
        {
            following.Id = _Followings.Max(f => f.Id) + 1;
            _Followings.Add(following);
        }
    }
}
