using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;

namespace apiPost.Controllers
{
    [ApiController]
    [Route("Post")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private DAOFactory df;

        public PostController(ILogger<PostController> logger, DAOFactory df)
        {
            _logger = logger;
            this.df = df;
        }


        [HttpGet("get")]
        public IActionResult Post([FromQuery] int id)
        {
            Post post = this.df.CreateDAOPost().GetPostById(id);
            if (post == null) return null;

            GetPostResponseDTO response = new GetPostResponseDTO
            {
                IdUser = post.IdUser,
                title = post.Title,
                content = post.Content,
                userName = post.UserName,
                userAvatar = post.userAvatar,
                commentsCount = post.GetCountComments(),
                likes = post.GetCountLike(),
                dislikes = post.GetCountDislike(),
                fileUrl = post.GetUrlImage() ?? "",
                comments = post.GetComments()
            };

            return Ok(response);
        }


        // [HttpGet("getAll")]
        // public IActionResult getAll([FromQuery] GetAllPostRequestDTO request)
        // {
        //     List<Post> allPosts = this.df.CreateDAOPost().getAll();
        //     List<GetPostResponseDTO> posts = new List<GetPostResponseDTO>();

        //     foreach (Post post in allPosts)
        //     {
        //         GetPostResponseDTO getPost = new GetPostResponseDTO
        //         {
        //             title = post.Title,
        //             content = post.Content,
        //             userName = post.UserName,
        //             userAvatar = post.userAvatar,
        //             commentsCount = post.GetCountComments(),
        //             likes = post.GetCountLike(),
        //             dislikes = post.GetCountDislike(),
        //             fileUrl = post.GetUrlImage() ?? "",
        //             comments = post.GetComments()
        //         };

        //         posts.Add(getPost);
        //     }

        //     GetAllPostResponseDTO response = new GetAllPostResponseDTO
        //     {
        //         Posts = posts
        //     };

        //     return Ok (response);
        // }

        [HttpGet]
        public IActionResult getAllPosts([FromQuery] GetAllPostRequestDTO request)
        {
            int pageNumber = request.pageNumber <= 0 ? 1 : request.pageNumber;
            
            
            List<GetPostResponseDTO> postsAll = new List<GetPostResponseDTO>();

            List<Post> posts = new List<Post>();

            if (request.idUserLogger != 0)
            {
                //TODO: menejar token previamente
                if (request.isMyPosts)
                {
                    // Traer solo los posts del usuario logueado
                    posts = this.df.CreateDAOPost().GetPosts(new List<int> { request.idUserLogger });
                }
                else
                {
                    // Traer los posts de los usuarios seguidos
                    var followedIds = this.df.CreateDAOFollowing().GetFollowedUserIds(request.idUserLogger);
                    posts = this.df.CreateDAOPost().GetPosts(followedIds);
                }

            } else{
                // Traer todos los posts
                posts = this.df.CreateDAOPost().GetPosts(new List<int> { request.idUserConsultado});
            }
            

            foreach (Post post in posts)
            {
                GetPostResponseDTO getPosts = new GetPostResponseDTO
                {
                    IdUser = post.IdUser,
                    userName = post.UserName,
                    userAvatar = post.userAvatar,
                    title = post.Title,
                    content = post.Content,
                    commentsCount = post.GetCountComments(),
                    likes = post.GetCountLike(),
                    dislikes = post.GetCountDislike(),
                    fileUrl = post.GetUrlImage() ?? "",
                    comments = post.GetComments()
                };

                postsAll.Add(getPosts);
            }

            postsAll = postsAll
            .OrderByDescending(p => p.commentsCount) 
            .Skip((int)((pageNumber - 1) * 10))
            .Take(10)
            .ToList();


           GetAllPostResponseDTO response = new GetAllPostResponseDTO
            {
                Posts = postsAll
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult PostCreate([FromQuery] PostPostRequestDTO request)
        {
            File file = new File
            {
                Url = request.fileUrl
            };

            Post newPost = new Post
            {
                Title = request.title,
                Content = request.content,
                User = this.df.CreateDAOUser().GetUser(1),
                File = file
            };

            this.df.CreateDAOPost().CreatePost(newPost);

            PostPostResponseDTO response = new PostPostResponseDTO
            {
                message = "Post created successfully",
                IdUser = newPost.User.Id
            };

            return Ok(response);
        }


        [HttpDelete]
        public IActionResult DeletePost([FromQuery] DeletePostRequestDTO request)
        {
            Post post = this.df.CreateDAOPost().GetPostById(request.id);
            if (post == null) return NotFound();

            this.df.CreateDAOPost().DeletePost(request.id);

            DeletePostResponseDTO response = new DeletePostResponseDTO
            {
                message = "Post deleted successfully",
                idPost = post.Id
            };

            return Ok(response);
        }
    }
}