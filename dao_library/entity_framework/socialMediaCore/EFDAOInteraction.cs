using Microsoft.EntityFrameworkCore;

public class EFDAOInteraction : DAOInteraction
{
    private AppDbContext appDbContext;
    public EFDAOInteraction(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }
    public Interaction CreateInteraction(Interaction interaction)
    {
        var createdInteraction = this.appDbContext.Interactions.Add(interaction);
        this.appDbContext.SaveChanges();
        this.appDbContext.Entry(interaction).State = EntityState.Detached;
        return createdInteraction.Entity;
    }
    public Interaction? GetInteractionById(int id)
    {
        return this.appDbContext.Interactions.Find(id);
    }

    public void DeleteInteraction(int id)
    {
        var interaction = this.appDbContext.Interactions.Find(id);
        if (interaction != null)
        {
            this.appDbContext.Interactions.Remove(interaction);
            this.appDbContext.SaveChanges();
        }
    }
    public Interaction? GetInteractionByPostAndUser(int postId, int userId)
    {

        // return this.appDbContext.Interactions
        //     .FirstOrDefault(i => i.PostId == postId && i.UserId == userId);

        return this.appDbContext.Interactions
        .AsNoTracking()
        .Include(i => i.Post) // <-- Necesario para acceder a i.Post.Id
        .Include(i => i.User) // <-- Necesario para acceder a i.User.Id
        .FirstOrDefault(i => i.Post.Id == postId && i.User.Id == userId);
    }


}