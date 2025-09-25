using Microsoft.EntityFrameworkCore;

public class EFDAORole : DAORole
{
    private AppDbContext dbContext;
    public EFDAORole(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateRole(Role role)
    {
        this.dbContext.Roles.Add(role);
        dbContext.SaveChanges();
    }

    public void DeleteRole(int id)
    {
        this.dbContext.Roles.Where(role => role.Id == id).ExecuteDelete();
    }

    public IEnumerable<Role> GetAllRoles()
    {
        throw new NotImplementedException();
    }

    public Role? GetRoleById(int id)
    {
        return this.dbContext.Roles.FirstOrDefault(role => role.Id == id);
    }

    public void UpdateRole(Role role)
    {
        this.dbContext.Roles.Update(role);
    }
}