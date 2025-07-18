using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockRoleDAO : DAORole
    {
        private readonly List<Role> _Roles = new List<Role>();

        public MockRoleDAO()
        {
            // Roles de prueba
            _Roles.Add(new Role { IdRole = 1, RoleName = "Admin" });
            _Roles.Add(new Role { IdRole = 2, RoleName = "User" });
        }

        public List<Role> GetAll()
        {
            return _Roles;
        }

        public Role GetRoleById(int id)
        {
            return _Roles.FirstOrDefault(r => r.IdRole == id);
        }

        public void Save(Role role)
        {
            role.IdRole = _Roles.Max(r => r.IdRole) + 1;
            _Roles.Add(role);
        }

        public void UpdateRole(Role role)
        {
            var existingRole = GetRoleById(role.IdRole);
            if (existingRole != null)
            {
                existingRole.RoleName = role.RoleName;
            }
        }

        public void DeleteRole(int id)
        {
            var role = GetRoleById(id);
            if (role != null)
            {
                _Roles.Remove(role);
            }
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _Roles;
        }

        public void CreateRole(Role role)
        {
            Save(role);
        }
        
    }
}
