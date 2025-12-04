using Microsoft.EntityFrameworkCore;

public class EFDAOFile : DAOFile
{
    private AppDbContext dbContext;
    public EFDAOFile(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public File CreateFile(string url)
    {
        return GetFile(url);
    }

    public void DeleteFile(int idFile)
    {
        this.dbContext.Files.Where(file => file.Id == idFile).ExecuteDelete();
    }

    public File? GetFile(string url)
    {
        return this.dbContext.Files.FirstOrDefault(file => file.Url == url);
    }

    public void UpdateFile(string url)
    {
        throw new NotImplementedException();
    }
}