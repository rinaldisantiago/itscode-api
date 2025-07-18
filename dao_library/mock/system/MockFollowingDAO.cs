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
            // Seguimientos de prueba
            _Followings.Add(new Following { IdFollowing = 1, FollowerId = 1, FollowedId = 2 });
            _Followings.Add(new Following { IdFollowing = 2, FollowerId = 2, FollowedId = 1 });
        }


        public Following GetFollowingById(int id)
        {
            return _Followings.FirstOrDefault(f => f.IdFollowing == id);
        }

        public void Save(Following following)
        {
            following.IdFollowing = _Followings.Max(f => f.IdFollowing) + 1;
            _Followings.Add(following);
        }

        public void UpdateFollowing(Following following)
        {
            var existingFollowing = GetFollowingById(following.IdFollowing);
            if (existingFollowing != null)
            {
                existingFollowing.FollowerId = following.FollowerId;
                existingFollowing.FollowedId = following.FollowedId;
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
            Save(following);
        }
    }
}
