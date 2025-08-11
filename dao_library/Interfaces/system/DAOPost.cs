using entity_library;

public interface DAOPost
{
    // Define methods for managing posts in the system
    void CreatePost(Post post);
    Post? GetPostById(int id);
    List<Post> getAll();
    void UpdatePost(Post post);
    void DeletePost(int id);
    
}
    


