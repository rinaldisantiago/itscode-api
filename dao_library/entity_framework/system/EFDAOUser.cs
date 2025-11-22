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

    public bool GetUserByUsername(string username)
    {
        return this.dbContext.Users.Any(user => user.UserName == username.Trim());
    }

    public bool GetUserByEmail(string email)
    {
        return this.dbContext.Users.Any(user => user.Email == email.Trim().ToLower());
    }

    public User Login(string userName)
    {
        return this.dbContext.Users.FirstOrDefault(user => user.UserName == userName);
    }

    public User UpdateUser(User user)
    {
        this.dbContext.Users.Update(user);
        dbContext.SaveChanges();
        return user;
    }

    public List<User> GetSugerencias(int idUserLogger, int page, int pageSize, List<int> followingIds)
    {
        var user = this.dbContext.Users.FirstOrDefault(u => u.Id == idUserLogger);
        if (user == null) return new List<User>();


        var sugerencias = this.dbContext.Users
            .Where(u => u.Id != idUserLogger && !followingIds.Contains(u.Id))
            .OrderBy(u => u.UserName)
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

    public List<User> GetUsers(int pageNumber, int pageSize)
    {
        return this.dbContext.Users
        .OrderBy(u => u.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();  
    }
}
