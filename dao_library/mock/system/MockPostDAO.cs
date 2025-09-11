using entity_library;
using System.Collections.Generic;
using System.Linq;


namespace dao_library
{
    public class MockPostDAO : DAOPost
    {
        private List<Post> _Posts;
        private List<Comment> _Comments = new List<Comment>();
        private List<Interaction> _Interactions = new List<Interaction>();

        public MockPostDAO()
        {
            _Posts = new List<Post>();
            MockUserDAO userDAO = new MockUserDAO();
            User user1 = userDAO.GetUser(1);
            User user2 = userDAO.GetUser(2);

            MockFileDAO fileDAO = new MockFileDAO();
            File file1 = fileDAO.GetFile("https://definicion.de/wp-content/uploads/2012/01/imagen-vectorial.png");
            File file2 = fileDAO.GetFile("http://example.com/image2.jpg");


            _Posts.Add(new Post { Id = 1, Title = "Mi Primer post", Content = "Este es mi primer posteo de prueba", User = user1, Comments = new List<Comment>(), Interactions = new List<Interaction>(), File = file1 });
            _Posts.Add(new Post { Id = 2, Title = "Un hermoso dia de lluvia", Content = "Hoy me toco caminar hasta mi casa bajo la lluvia", User = user2, Comments = new List<Comment>(), Interactions = new List<Interaction>(), File = file2 });


            Post post1 = GetPostById(1);
            Post post2 = GetPostById(2);

            Comment comment1 = new Comment();
            comment1.Id = 1;
            comment1.Content = "¡Gran post!";
            comment1.User = user2;
            comment1.Post = post1;

            Comment comment2 = new Comment();
            comment2.Id = 2;
            comment2.Content = "Me encanta la lluvia.";
            comment2.User = user1;
            comment2.Post = post2;

            Comment comment3 = new Comment();
            comment3.Id = 3;
            comment3.Content = "Muy interesante.";
            comment3.User = user2;
            comment3.Post = post1;

            Comment comment4 = new Comment();
            comment4.Id = 4;
            comment4.Content = "Gracias por compartir.";
            comment4.User = user1;
            comment4.Post = post2;

            _Comments.Add(comment1);
            _Comments.Add(comment2);
            _Comments.Add(comment3);
            _Comments.Add(comment4);

            Interaction interaction1 = new Interaction
            {
                Id = 1,
                Post = post1,
                User = user2,
                InteractionType = InteractionType.Like
            };

            Interaction interaction2 = new Interaction
            {
                Id = 2,
                Post = post1,
                User = user1,
                InteractionType = InteractionType.Dislike
            };

            Interaction interaction3 = new Interaction
            {
                Id = 3,
                Post = post2,
                User = user1,
                InteractionType = InteractionType.Like
            };
            
            Interaction interaction4 = new Interaction
            {
                Id = 4,
                Post = post2,
                User = user2,
                InteractionType = InteractionType.Like
            };

            _Interactions.Add(interaction1);
            _Interactions.Add(interaction2);
            _Interactions.Add(interaction3);
            _Interactions.Add(interaction4);

        }


        public Post GetPostById(int id)
        {
            
            var post = _Posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                List<Comment> postComments = _Comments.Where(c => c.Post.Id == id).ToList();
                post.Comments = postComments;
                List<Interaction> postInteractions = _Interactions.Where(i => i.Post.Id == id).ToList();
                post.Interactions = postInteractions;
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

        // public List<Post> getAll()
        // {
        //     foreach (var post in _Posts)
        //     {
        //         List<Comment> postComments = _Comments
        //         .Where(c => c.Post.Id == post.Id)
        //         .ToList();
        //         post.Comments = postComments;

        //         List<Interaction> postInteractions = _Interactions
        //         .Where(i => i.Post.Id == post.Id)
        //         .ToList();
        //         post.Interactions = postInteractions;
        //     }

        //     return _Posts;
        // }

        public List<Post> GetPosts(List<int> userIds)
        {
            var posts = _Posts
                .Where(p => userIds.Contains(p.User.Id))  // filtra por la lista de usuarios
                .ToList();

            foreach (var post in posts)
            {
                post.Comments = _Comments
                    .Where(c => c.Post.Id == post.Id)
                    .ToList();

                post.Interactions = _Interactions
                    .Where(i => i.Post.Id == post.Id)
                    .ToList();
            }

            return posts;
        }




        public void CreatePost(Post post)
        {
            post.Id = _Posts.Max(u => u.Id) + 1;
            _Posts.Add(post);
        }
    }


}


