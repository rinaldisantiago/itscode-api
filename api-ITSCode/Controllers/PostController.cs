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



        [HttpGet("{idPost}")]
        public IActionResult Get(int idPost)
        {
            Post? post = this.df.CreateDAOPost().GetPostById(idPost);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet("all")]
        public IActionResult GetAllPost()
        {
            IEnumerable<Post> posts = this.df.CreateDAOPost().GetAllPosts();
            if (posts == null )
            {
                return NotFound();
            }
            return Ok(posts);
        }

        
    }
}