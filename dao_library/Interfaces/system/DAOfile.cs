using entity_library;

public interface DAOFile
{
    File CreateFile(string url);
    File? GetFile(string url);
    void UpdateFile(string url);
    void DeleteFile(int idFile);
}

    


