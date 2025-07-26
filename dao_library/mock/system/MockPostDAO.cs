using entity_library;
using System.Collections.Generic;
using System.Linq;


namespace dao_library
{
    public class MockPostDAO : DAOPost
    {
        private List<Post> _Posts;
        private List<Comment> _Comments = new List<Comment>();

        public MockPostDAO()
        {
            _Posts = new List<Post>();
            MockUserDAO userDAO = new MockUserDAO();
            User user1 = userDAO.GetUser(1);
            User user2 = userDAO.GetUser(2);

            MockFileDAO fileDAO = new MockFileDAO();
            File file1 = fileDAO.GetFile("http://example.com/image1.jpg");
            File file2 = fileDAO.GetFile("http://example.com/image2.jpg");

            // Usuarios de prueba
            _Posts.Add(new Post { Id = 1, Title = "Mi Primer post", Content = "Este es mi primer posteo de prueba",User = user1, Comments = new List<Comment>(), Interactions = new List<Interaction>(), File = file1 });
            _Posts.Add(new Post { Id = 2, Title = "Un hermoso dia de lluvia", Content = "Hoy me toco caminar hasta mi casa bajo la lluvia", User = user2, Comments = new List<Comment>(), Interactions = new List<Interaction>(), File = file2 });


            Post post1 = GetPostById(1);
            Post post2 = GetPostById(2);

            Comment comment1 = new Comment();
            comment1.Id = 1;
            comment1.Content = "¡Gran post!";
            comment1.Post = post1;

            Comment comment2 = new Comment();
            comment2.Id = 2;
            comment2.Content = "Me encanta la lluvia.";
            comment2.Post = post2;

            Comment comment3 = new Comment();
            comment3.Id = 3;
            comment3.Content = "Muy interesante.";
            comment3.Post = post1;

            Comment comment4 = new Comment();
            comment4.Id = 4;
            comment4.Content = "Gracias por compartir.";
            comment4.Post = post2;

            _Comments.Add(comment1);
            _Comments.Add(comment2);
            _Comments.Add(comment3);
            _Comments.Add(comment4);

        }


        public Post GetPostById(int id)
        {
            var post = _Posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                // Filtrar comentarios que pertenecen a este post
                var postComments = _Comments.Where(c => c.Post.Id == id).ToList();
                post.Comments = postComments;
            }
            return post;
        }


        public void UpdatePost(Post post)
        {
            var existingPost = GetPostById(post.Id);
            if (existingPost != null)
            {
                existingPost.Title = post.Title;
                existingPost.Content = post.Content;
            }
        }

        public void DeletePost(int id)
        {
            var post = GetPostById(id);
            if (post != null)
            {
                _Posts.Remove(post);
            }
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _Posts;
        }

        public void CreatePost(Post post)
        {
            post.Id = _Posts.Max(u => u.Id) + 1;
            _Posts.Add(post);
        }
    }


}


