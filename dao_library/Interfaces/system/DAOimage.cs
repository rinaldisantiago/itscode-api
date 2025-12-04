using entity_library;

public interface DAOImage
{
    Image CreateImage(string url);
    Image? GetImage(string url);
    void UpdateImage(string url);
    void DeleteImage(int id);
}

    


