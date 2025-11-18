using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;
using Microsoft.AspNetCore.Hosting; 

namespace apiPost.Controllers
{
    [ApiController]
    [Route("Post")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private DAOFactory df;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostController(ILogger<PostController> logger, DAOFactory df, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            this.df = df;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult GetPosts([FromQuery] GetAllPostRequestDTO request)
        {
            try
            {
                if (this.df.CreateDAOUser().GetUser(request.idUserLogger) == null)
                {
                    return Unauthorized("Invalid user.");
                }
                
                int pageNumber = request.pageNumber <= 0 ? 1 : request.pageNumber;
                int pageSize = request.pageSize <= 0 ? 1 : request.pageSize;

                List<Post> posts;

                posts = this.df.CreateDAOPost().GetPosts(
                    request.idUserConsultado,
                    request.idUserLogger,
                    request.isMyPosts,
                    pageNumber,
                    pageSize
                );
                
                var postsAll = posts
                    .Select(post => new GetPostResponseDTO
                    {
                        id = post.Id,
                        idUser = post.IdUser,
                        userName = post.UserName,
                        userAvatar = post.UserAvatar(),
                        title = post.Title,
                        content = post.Content,
                        commentsCount = post.GetCountComments(),
                        likesCount = post.GetCountLike(),
                        dislikesCount = post.GetCountDislike(),
                        fileUrl = post.GetUrlImage() ?? "",
                        comments = post.GetComments(request.pageNumberComments, request.pageSizeComments), 
                        userInteraction = GetUserInteraction(post, request.idUserLogger)

                    })
                    .ToList();

                var response = new GetAllPostResponseDTO
                {
                    posts = postsAll,
                    
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // ... (Manejo de errores) ...
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message,
                });
            }
        }

        [HttpGet("{id}/{idUserLogger}")]
        public IActionResult GetPostById([FromRoute] GetPostRequestDTO request)
        {
            int id = request.id;
            int idUserLogger = request.idUserLogger;

      
        
            try
            {
                // Validamos que el usuario que hace la petición exista
                if (this.df.CreateDAOUser().GetUser(idUserLogger) == null)
                {
                    return Unauthorized("Invalid user.");
                }

                var post = this.df.CreateDAOPost().GetPostById(id);

                if (post == null)
                {
                    return NotFound(new { message = "Post not found" });
                }

                // Mapeamos la entidad Post al DTO de respuesta
                var postResponse = new GetPostResponseDTO
                {
                    id = post.Id,
                    idUser = post.IdUser,
                    userName = post.UserName,
                    userAvatar = post.UserAvatar(),
                    title = post.Title,
                    content = post.Content,
                    commentsCount = post.GetCountComments(),
                    likesCount = post.GetCountLike(),
                    dislikesCount = post.GetCountDislike(),
                    fileUrl = post.GetUrlImage() ?? "",
                    comments = post.GetComments(request.pageNumberComments, request.pageSizeComments), 
                    userInteraction = GetUserInteraction(post, idUserLogger) 
                };

                return Ok(postResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting post with id {PostId}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        
        }

        // Método auxiliar para obtener la interacción del usuario
        private UserInteractionResponseDTO GetUserInteraction(Post post, int userId)
        {
            var userInteraction = post.Interactions.FirstOrDefault(i => i.User.Id == userId);
            
            if (userInteraction == null)
            {
                return new UserInteractionResponseDTO 
                { 
                    interactionId = null, 
                    type = null 
                };
            }
            
            return new UserInteractionResponseDTO
            {
                interactionId = userInteraction.Id,
                type = (int)userInteraction.InteractionType
            };
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult PostCreate([FromForm] PostPostRequestDTO request)
        {
            var user = this.df.CreateDAOUser().GetUser(request.idUser);
            if (user == null)
            {
                return Unauthorized("Invalid user.");
            }

            string finalFileUrl = null;

            if (request.File != null && request.File.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "posts_files");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.File.CopyTo(fileStream);
                }
                
                finalFileUrl = $"/posts_files/{uniqueFileName}";
            }
            else if (!string.IsNullOrEmpty(request.fileUrl))
            {
                finalFileUrl = request.fileUrl;
            }

            File fileEntity = new File { Url = finalFileUrl };

            Post newPost = new Post
            {
                Title = request.title,
                Content = request.content,
                User = this.df.CreateDAOUser().GetUser(request.idUser),
                File = fileEntity,
                CreatedAt = DateTime.UtcNow
            };

            this.df.CreateDAOPost().CreatePost(newPost);

            PostPostResponseDTO response = new PostPostResponseDTO
            {
                message = "Post created successfully",
                idUser = newPost.User.Id
            };

            return StatusCode(201, response);
        }


        [HttpDelete]
        public IActionResult DeletePost([FromQuery] DeletePostRequestDTO request)
        {
            var user = this.df.CreateDAOUser().GetUser(request.idUser);
            if (user == null)
            {
                return Unauthorized("Invalid user.");
            }

            Post post = this.df.CreateDAOPost().GetPostById(request.id);
            if (post == null) return NotFound();

            if (post.User.Id != user.Id)
            {
                return Forbid("User is not the author of the post.");
            }

            this.df.CreateDAOPost().DeletePost(request.id);
            this.df.CreateDAOFile().DeleteFile(post.File.Id);

            DeletePostResponseDTO response = new DeletePostResponseDTO
            {
                message = "Post deleted successfully",
                idPost = post.Id
            };

            return Ok(response);
        }

        [HttpPut]
        public IActionResult UpdatePost([FromBody] UpdatePostRequestDTO request)
        {
            var user = this.df.CreateDAOUser().GetUser(request.idUser);
            if (user == null)
            {
                return Unauthorized("Invalid user.");
            }

            Post post = this.df.CreateDAOPost().GetPostById(request.id);
            if (post == null) return NotFound();

            if (post.User.Id != user.Id)
            {
                return Forbid("User is not the author of the post.");
            }

            post.Title = request.title;
            post.Content = request.content;
            post.File.Url = request.fileUrl;

            this.df.CreateDAOPost().UpdatePost(post);

            UpdatePostResponseDTO response = new UpdatePostResponseDTO
            {
                message = "Post updated successfully"
            };

            return Ok(response);
        }
    }
}