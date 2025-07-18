public class Image : File
{
    private string imageFormat = "";
    private int idPerson;

    public string ImageFormat { get { return this.imageFormat; } set { this.imageFormat = value; } }
    public int IdPerson { get { return this.idPerson; } set { this.idPerson = value; } }

}