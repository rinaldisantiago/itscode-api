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
            try
            {   
                User? user = this.df.CreateDAOUser().GetUser(request.idUser);
                if(user is null) return Unauthorized(new { message = "Usuario inválido." });

                Comment? comment = this.df.CreateDAOComment().GetCommentById(request.id);
                if (comment is null) return NotFound(new { message = "Comentario no encontrado." });

                Post? post = this.df.CreateDAOPost().GetPostById(request.idPost);
                if (post is null) return NotFound(new { message = "Post no encontrado." });

                bool isCommentOwner = comment.User?.Id.Equals(user.Id) ?? false;
                bool isPostOwner = post.User?.Id.Equals(user.Id) ?? false;

                if (!isCommentOwner && !isPostOwner)
                {
                    return StatusCode(403, new { message = "No tienes permiso para realizar esta acción." });
                }

                this.df.CreateDAOComment().DeleteComment(request.id);

                DeleteCommentResponseDTO response = new DeleteCommentResponseDTO
                {
                    message = "Comentario eliminado exitosamente."
                };

                return Ok(response);
            }

            catch (Exception ex)
            {
                return StatusCode(500, 
                    new { 
                        message = "Error interno del servidor.",
                        error = ex.Message 
                    });
            }
        }

        [HttpPut]
        public IActionResult UpdateComment([FromBody] UpdateCommentRequestDTO request)
        {
            try
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

            catch (Exception ex)
            {
                return StatusCode(500, 
                    new { 
                        message = "Error interno del servidor.",
                        error = ex.Message 
                    });
            }
        }

        [HttpGet]
        public IActionResult GetCommentsByPostId([FromQuery] GetCommentRequestDTO request)
        {
            try
            {
                var comments = this.df.CreateDAOComment().GetCommentsByPostId(request.postId, request.pageNumber, request.pageSize);
                var response = new GetCommentResponseDTO
                {
                    comments = comments.Select(c => new CommentDTO
                    {
                        id = c.Id,
                        userId = c.User.Id,
                        postId = c.Post.Id,
                        content = c.Content,
                        createdAt = c.CreatedAt,
                        username = c.User.UserName,
                        avatarUrl = c.User.Avatar != null ? c.User.Avatar.Url : null
                    }).ToList()
                };

                return Ok(response);
            }

            catch (Exception ex)
            {
                return StatusCode(500, 
                    new { 
                        message = "Error interno del servidor.",
                        error = ex.Message 
                    });
            }
        }
    }
}