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
                int pageSize = request.pageSize <= 0 ? 1 : request.pageSize; //TODO: ver si es necesario la condicional

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
                        comments = post.GetComments(),
                        UserInteraction = GetUserInteraction(post, request.idUserLogger)

                    })
                    .ToList();

                var response = new GetAllPostResponseDTO
                {
                    Posts = postsAll
                };

                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message,

                });
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
                    InteractionId = null, 
                    Type = null 
                };
            }
            
            return new UserInteractionResponseDTO
            {
                InteractionId = userInteraction.Id,
                Type = (int)userInteraction.InteractionType
            };
        }


        [HttpPost]
        public IActionResult PostCreate([FromQuery] PostPostRequestDTO request)
        {
            var user = this.df.CreateDAOUser().GetUser(request.idUser);
            if (user == null)
            {
                return Unauthorized("Invalid user.");
            }

            File file = new File
            {
                Url = request.fileUrl
            };

            Post newPost = new Post
            {
                Title = request.title,
                Content = request.content,
                User = user,
                File = file,
                CreatedAt = DateTime.Now
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