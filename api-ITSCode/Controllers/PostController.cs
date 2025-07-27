using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;

namespace apiPost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private DAOFactory df;

        public PostController(ILogger<PostController> logger, DAOFactory df)
        {
            _logger = logger;
            this.df = df;
        }



        [HttpGet("")]
        public GetPostResponseDTO getPost([FromQuery] GetPostRequestDTO request)
        {
            Post post = this.df.CreateDAOPost().GetPostById(request.Id);
            if (post == null) return null;

            GetPostResponseDTO response = new GetPostResponseDTO
            {
                title = post.Title,
                content = post.Content,
                userName = post.UserName,
                userAvatar = post.userAvatar,
                commentsCount = post.GetCountComments(),
                likes = post.GetCountLike(),
                dislikes = post.GetCountDislike(),
                fileUrl = post.File?.Url ?? "",
                comments = post.GetComments()
            };

            return response;

        }

        [HttpPost("")]
        public PostPostResponseDTO PostPost([FromQuery] PostPostRequestDTO request)
        {
            Post newPost = new Post
            {
                Title = request.title,
                Content = request.content,
                User = this.df.CreateDAOUser().GetUser(1),
                File = this.df.CreateDAOFile().GetFile(request.fileUrl)
            };

            this.df.CreateDAOPost().CreatePost(newPost);

            PostPostResponseDTO response = new PostPostResponseDTO
            {
                message = "Post created successfully",
                IdUser = newPost.User.Id
            };

            return response;

        }


        [HttpDelete("")]
        public DeletePostResponseDTO DeletePost([FromQuery] DeletePostRequestDTO request)
        {
            Post post = this.df.CreateDAOPost().GetPostById(request.id);
            if (post == null) return new DeletePostResponseDTO { message = "Post not found" };

            this.df.CreateDAOPost().DeletePost(request.id);

            return new DeletePostResponseDTO
            {
                idPost = request.id
            };
        }
    }
}