public class File
{
    private int id;
    private string path = "";
    private string url = "";

    public int Id { get { return this.id; } set { this.id = value; } }
    public string Path { get { return this.path; } set { this.path = value; } }
    public string Url { get { return this.url; } set { this.url = value; } }

}