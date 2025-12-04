public interface DAORole
{
    void CreateRole(Role role);
    Role? GetRoleById(int id);
    IEnumerable<Role> GetAllRoles();
    void UpdateRole(Role role);
    void DeleteRole(int id);

    
}
    


