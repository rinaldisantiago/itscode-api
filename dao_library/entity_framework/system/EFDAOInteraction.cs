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

    
}