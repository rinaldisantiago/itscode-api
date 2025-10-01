using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;


namespace apiComment.Controllers
{
    [ApiController]
    [Route("Comment")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private DAOFactory df;

        public CommentController(ILogger<CommentController> logger, DAOFactory df)
        {
            _logger = logger;
            this.df = df;
        }

        [HttpPost("create")]
        public IActionResult CreateComment([FromBody] PostCommentRequestDTO request)
        {
            try
            {
                var user = this.df.CreateDAOUser().GetUser(request.userId);
                var post = this.df.CreateDAOPost().GetPostById(request.postId);

                if (user == null || post == null)
                {
                    return NotFound("User or Post not found");
                }

                Comment newComment = new Comment
                {
                    User = user,
                    Post = post,
                    Content = request.content,
                    CreatedAt = DateTime.Now
                };

                this.df.CreateDAOComment().CreateComment(newComment);
                PostCommentResponseDTO response = new PostCommentResponseDTO
                {
                    message = "Comment created successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment");
                return StatusCode(500, "Internal server error");
            }
        }



        
    }
}