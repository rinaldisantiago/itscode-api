using entity_library;

public interface DAOfile
{
    // Define methods for managing images in the system
    File CreateFile(string url);
    File? GetFile(string url);
    void UpdateFile(string url);
    void DeleteFile(string url);
}

    


