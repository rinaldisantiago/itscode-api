using entity_library;
using Microsoft.VisualBasic;

public class Comment
{
    private int id;
    private User? user;
    private Post? post;
    private string? content;
    private DateTime createdAt;


    public int Id { get => this.id; set => this.id = value; }
    public string? Content { get => this.content; set => this.content = value; }
    public virtual Post? Post { get { return this.post; } set { this.post = value; } } //TODO: verificar si mantenemos comentarios en DB cuando se elimina un post
    public virtual User? User { get { return this.user; } set { this.user = value; } } //TODO: verificar si mantenemos comentarios en DB cuando se elimina un usuario
    public virtual DateTime CreatedAt { get => this.createdAt; set => this.createdAt = value; }

    
}