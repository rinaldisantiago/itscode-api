using entity_library;

public interface DAOFile
{
    // Define methods for managing images in the system
    File CreateFile(string url);
    File? GetFile(string url);
    void UpdateFile(string url);
    void DeleteFile(int idFile);
}

    


