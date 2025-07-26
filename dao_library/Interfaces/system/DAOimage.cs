using entity_library;

public interface DAOImage
{
    // Define methods for managing images in the system
    Image CreateImage(string url);
    Image? GetImage(string url);
    void UpdateImage(string url);
    void DeleteImage(string url);
}

    


