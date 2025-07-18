using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockBanDAO : DAOBan
    {
        private readonly List<Ban> _Bans = new List<Ban>();

        public MockBanDAO()
        {
            // Bans de prueba
            _Bans.Add(new Ban { IdBan = 1, IdPerson = 1, Reason = "Spam", BanDate = DateTime.Now, UnbanDate = null });
            _Bans.Add(new Ban { IdBan = 2, IdPerson = 2, Reason = "Inappropriate content", BanDate = DateTime.Now, UnbanDate = null });
        }
        public Ban GetBanById(int id)
        {
            return _Bans.FirstOrDefault(b => b.IdBan == id);
        }

        public void Save(Ban ban)
        {
            ban.IdBan = _Bans.Max(b => b.IdBan) + 1;
            _Bans.Add(ban);
        }

        public void UpdateBan(Ban ban)
        {
            var existingBan = GetBanById(ban.IdBan);
            if (existingBan != null)
            {
                existingBan.IdPerson = ban.IdPerson;
                existingBan.Reason = ban.Reason;
                existingBan.BanDate = ban.BanDate;
                existingBan.UnbanDate = ban.UnbanDate;
            }
        }

        public void DeleteBan(int id)
        {
            var ban = GetBanById(id);
            if (ban != null)
            {
                _Bans.Remove(ban);
            }
        }

        public IEnumerable<Ban> GetAllBans()
        {
            return _Bans;
        }

        public void CreateBan(Ban ban)
        {
            Save(ban);
        }
    }
}
