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
        //La funcion además de añadir la imagen a la BD tambíen la obtiene, habiendo dos funciones en una
        // this.dbContext.Images.Add(url);
        // dbContext.SaveChanges();
        return GetImage(url);
    }

    public void DeleteImage(int id)
    {
        this.dbContext.Images.Where(image => image.Id == id).ExecuteDelete();
    }

    public Image? GetImage(string url)
    {
        return this.dbContext.Images.FirstOrDefault(image => image.Url == url);
    }

    public void UpdateImage(string url)
    {
        //Pasar dos parametros: url a buscar (url vieja) y url de la nueva imagen
        throw new NotImplementedException();
    }
}