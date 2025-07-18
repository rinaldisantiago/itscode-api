public class File
{
    private int idFile;
    private string fileName = "";
    private string filePath = "";

    public int IdFile { get { return this.idFile; } set { this.idFile = value; } }
    public string FileName { get { return this.fileName; } set { this.fileName = value; } }
    public string FilePath { get { return this.filePath; } set { this.filePath = value; } }

}