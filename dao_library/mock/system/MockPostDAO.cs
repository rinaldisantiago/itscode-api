using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockPostDAO : DAOPost
    {
        private  List<Post> _Posts = new List<Post>();

        public MockPostDAO()
        {
            // Usuarios de prueba
            _Posts.Add(new Post { IdPost = 1, Title = "Mi Primer post", Content = "Este es mi primer posteo de pruueba", IdPerson = 1, Comments = new List<Comment>(), Interactions = new List<Interaction>() });
            _Posts.Add(new Post { IdPost = 2, Title = "Un hermoso dia de lluvia", Content = "Hoy me toco caminar hasta mi casa bajo la lluvia", IdPerson = 2, Comments = new List<Comment>(), Interactions = new List<Interaction>() });
        }


        public Post GetPostById(int id)
        {
            return _Posts.FirstOrDefault(u => u.IdPost == id);
        }
        public void Save(Post post)
        {
            post.IdPost = _Posts.Max(u => u.IdPost) + 1;
            _Posts.Add(post);
        }

        public void UpdatePost(Post post)
        {
            var existingPost = GetPostById(post.IdPost);
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
            Save(post);
        }
    }
}
