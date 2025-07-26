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
            _Roles.Add(new Role { Id = 1, Name = "Admin" });
            _Roles.Add(new Role { Id = 2, Name = "User" });
        }

        public List<Role> GetAll()
        {
            return _Roles;
        }

        public Role GetRoleById(int id)
        {
            return _Roles.FirstOrDefault(r => r.Id == id);
        }

        public void UpdateRole(Role role)
        {
            var existingRole = GetRoleById(role.Id);
            if (existingRole != null)
            {
                existingRole.Name = role.Name;
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
            role.Id = _Roles.Max(r => r.Id) + 1;
            _Roles.Add(role);
        }
        
    }
}
