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
            };

            return response;
            
        }
    }
}