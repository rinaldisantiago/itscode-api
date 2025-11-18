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

        [HttpPost]
        public IActionResult CreateComment([FromBody] PostCommentRequestDTO request)
        {
            try
            {
                var user = this.df.CreateDAOUser().GetUser(request.userId);
                var post = this.df.CreateDAOPost().GetPostById(request.postId);

                if (user == null || post == null)
                {
                    return NotFound(new { message = "User or Post not found" });
                }

                if(String.IsNullOrWhiteSpace(request.content))
                {
                    return BadRequest(new { message = "El comentario no puede estar vacío." });
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

        [HttpDelete]
        public IActionResult DeleteComment([FromQuery] DeleteCommentRequestDTO request)
        {
            Comment comment = this.df.CreateDAOComment().GetCommentById(request.id);
            if (comment == null) return NotFound();

            this.df.CreateDAOComment().DeleteComment(request.id);

            DeleteCommentResponseDTO response = new DeleteCommentResponseDTO
            {
                message = "Comment deleted successfully"
            };

            return Ok(response);
        }

        [HttpPut]
        public IActionResult UpdateComment([FromBody] UpdateCommentRequestDTO request)
        {
            Comment comment = this.df.CreateDAOComment().GetCommentById(request.id);
            if (comment == null) return NotFound();

            comment.Content = request.content;
            this.df.CreateDAOComment().UpdateComment(comment);

            UpdateCommentResponseDTO response = new UpdateCommentResponseDTO
            {
                message = "Comment updated successfully"
            };

            return Ok(response);
        }
    }
}