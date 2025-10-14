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


        // [HttpGet("{id}")]
        // public IActionResult Post([FromQuery] GetPostByIdRequestDTO request)
        // {
        //     Post post = this.df.CreateDAOPost().GetPostById(request.id);
        //     if (post == null) return null;

        //     GetPostByIdResponseDTO response = new GetPostByIdResponseDTO
        //     {
        //         post = post
        //     };

        //     return Ok(response);
        // }


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
        public IActionResult GetPosts([FromQuery] GetAllPostRequestDTO request)
        {
            try
            {
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

                // 🔑 PUNTO CRÍTICO: Mapeo de Posts y la Interacción
                var postsAll = posts
                    .Select(post =>
                    {
<<<<<<< HEAD
                        // **1. BUSCAR INTERACCIÓN DEL USUARIO LOGUEADO**
                        // Asumimos que request.idUserLogger tiene el ID del usuario actual.
                        Interaction? userCurrentInteraction = this.df.CreateDAOInteraction()
                            .GetInteractionByPostAndUser(post.Id, request.idUserLogger);

                        // **2. CREAR EL DTO DE RESPUESTA**
                        var postDTO = new GetPostResponseDTO
                        {
                            // 🛑 SOLUCIÓN AL NaN: Aseguramos que el ID del Post se incluya en la respuesta.
                            // Esto corresponde a 'post.idPost' que usa tu frontend.
                            idPost = post.Id, // <-- Usamos la propiedad 'Id' de la entidad Post

                            // Propiedades existentes
                            idUser = post.IdUser,
                            userName = post.UserName,
                            userAvatar = post.UserAvatar(),
                            title = post.Title,
                            content = post.Content,
                            commentsCount = post.GetCountComments(),
                            likes = post.GetCountLike(),
                            dislikes = post.GetCountDislike(),
                            fileUrl = post.GetUrlImage() ?? "",
                            comments = post.GetComments(),

                            // **3. ASIGNAR EL ESTADO DE INTERACCIÓN (SOLUCIÓN AL 400)**
                            userInteraction = userCurrentInteraction != null ? new UserInteractionDTO
                            {
                                interactionId = userCurrentInteraction.Id,
                                interactionType = (int)userCurrentInteraction.InteractionType
                            } : null
                        };
                        return postDTO;
=======
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

>>>>>>> 36de3654731949cc15c25c596bdaa8d0ed742172
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
                // ... (Manejo de errores) ...
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
            File file = new File
            {
                Url = request.fileUrl
            };

            Post newPost = new Post
            {
                Title = request.title,
                Content = request.content,
                User = this.df.CreateDAOUser().GetUser(request.idUser),
                File = file,
                CreatedAt = DateTime.Now
            };

            this.df.CreateDAOPost().CreatePost(newPost);

            PostPostResponseDTO response = new PostPostResponseDTO
            {
                message = "Post created successfully",
                idUser = newPost.User.Id
            };

            return Ok(response);
        }


        [HttpDelete]
        public IActionResult DeletePost([FromQuery] DeletePostRequestDTO request)
        {
            Post post = this.df.CreateDAOPost().GetPostById(request.id);
            if (post == null) return NotFound();

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
            Post post = this.df.CreateDAOPost().GetPostById(request.id);
            if (post == null) return NotFound();

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