using Microsoft.EntityFrameworkCore;

public class EFDAOImage : DAOImage
{
    private AppDbContext dbContext;
    public EFDAOImage(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Image CreateImage(string url)
    {
        return GetImage(url);
    }

    public void DeleteImage(string url)
    {
        this.dbContext.Images.Where(image => image.Url == url).ExecuteDelete();
    }

    public Image? GetImage(string url)
    {
        return this.dbContext.Images.FirstOrDefault(image => image.Url == url);
    }

    public void UpdateImage(string url)
    {
        throw new NotImplementedException();
    }
}