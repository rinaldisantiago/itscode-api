using Microsoft.VisualBasic;

namespace entity_library;

public class Post
{
    private int idPost;
    private int idPerson;
    private string title;
    private string content;
    private List<Interaction>? interactions;
    private File? file;
    private List<Comment>? comments;


    public int IdPost { get { return this.idPost; } set { this.idPost = value; } }
    public string Title { get { return this.title; } set { this.title = value; } }
    public string Content { get { return this.content; } set { this.content = value; } }
    public List<Interaction>? Interactions { get { return this.interactions; } set { this.interactions = value; } }
    public File? File { get { return this.file; } set { this.file = value; } }
    public List<Comment>? Comments { get { return this.comments; } set { this.comments = value; } }
    public int IdPerson { get { return this.idPerson;} set { this.idPerson = value; }}
}
