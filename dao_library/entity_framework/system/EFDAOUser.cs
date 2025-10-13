using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

public class EFDAOUser : DAOUser
{
    private AppDbContext dbContext;
    public EFDAOUser(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateUser(User user)
    {
        this.dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }

    public void DeleteUser(int idUser)
    {
        this.dbContext.Users.Where(user => user.Id == idUser).ExecuteDelete();
    }

    public User? GetUser(int idUser)
    {
        return this.dbContext.Users.FirstOrDefault(user => user.Id == idUser);
    }

    public User? GetUserByEmail(string email)
    {
        return this.dbContext.Users.FirstOrDefault(user => user.Email == email);
    }

    public User Login(string userName, string password)
    {
        return this.dbContext.Users.FirstOrDefault(user => user.UserName == userName && user.Password == password);
    }

    public User UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    public List<User> GetSugerencias(int idUserLogger, int page, int pageSize)
    {
        var user = this.dbContext.Users.FirstOrDefault(u => u.Id == idUserLogger);
        if (user == null) return new List<User>();

        // usuarios que ya sigue
        var followingIds = this.dbContext.Followings
            .Where(f => f.UserFollowing.Id == idUserLogger)
            .Select(f => f.UserFollowed.Id)
            .ToList();

        // sugerencias (excluye al mismo user y a los que ya sigue)
        var query = this.dbContext.Users
            .Where(u => u.Id != idUserLogger && !followingIds.Contains(u.Id));

        // paginación
        var sugerencias = query
            .OrderBy(u => u.UserName) // opcional: orden alfabético o por algún criterio
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return sugerencias;
    }

    public List<User> SearchUsers(string searchTerm, int idUserLogger ,int pageNumber, int pageSize)
    {
        var user = this.dbContext.Users.FirstOrDefault(u => u.Id == idUserLogger);
        if (user == null) return new List<User>();

        return this.dbContext.Users
        .Where(u => u.Id != idUserLogger && 
                    (u.UserName.Trim().ToLower().Contains(searchTerm.Trim().ToLower()) || 
                     u.FullName.Trim().ToLower().Contains(searchTerm.Trim().ToLower())))
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();
    }
}
