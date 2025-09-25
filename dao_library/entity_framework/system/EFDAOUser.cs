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
}