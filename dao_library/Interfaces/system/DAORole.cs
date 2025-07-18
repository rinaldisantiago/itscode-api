public interface DAORole
{
    // Define methods for managing roles in the system
    void CreateRole(Role role);
    Role? GetRoleById(int id);
    IEnumerable<Role> GetAllRoles();
    void UpdateRole(Role role);
    void DeleteRole(int id);
    void Save(Role role);
    
}
    


