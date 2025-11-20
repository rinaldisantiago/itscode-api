using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;


namespace apiInteraction.Controllers
{
    [ApiController]
    [Route("Interaction")]
    public class InteractionController : ControllerBase
    {
        private readonly ILogger<InteractionController> _logger;
        private DAOFactory df;

        public InteractionController(ILogger<InteractionController> logger, DAOFactory df)
        {
            _logger = logger;
            this.df = df;
        }



        [HttpPost]
        public IActionResult CreateInteraction([FromBody] PostInteractionRequestDTO request)
        {
            Post post = this.df.CreateDAOPost().GetPostById(request.postId);
            User user = this.df.CreateDAOUser().GetUser(request.userId);

            if (post == null || user == null)
            {
                return BadRequest(new { message = "Post o usuario no encontrado." });
            }

            if (!Enum.IsDefined(typeof(InteractionType), request.interactionType))
            {
                return BadRequest(new { message = "Tipo de interacción inválido." });
            }

            Interaction? existingInteraction = this.df.CreateDAOInteraction()
            .GetInteractionByPostAndUser(request.postId, request.userId); 

            if (existingInteraction != null)
            {
                // ... (Tu lógica existente para 400 Bad Request y DELETE) ...
                if ((int)existingInteraction.InteractionType == request.interactionType)
                {
                    return BadRequest(new { message = "Ya existe esta interacción. El frontend debe llamar a DELETE para anularla." }); 
                }
                else
                {
                    this.df.CreateDAOInteraction().DeleteInteraction(existingInteraction.Id);
                }
            }

            Interaction interaction = new Interaction
            {
                PostId = request.postId, // <--- Usar el ID
                UserId = request.userId,   // <--- Usar el ID
                InteractionType = (InteractionType)request.interactionType
            };

            this.df.CreateDAOInteraction().CreateInteraction(interaction);

            int newInteractionId = interaction.Id;

            PostInteractionResponseDTO response = new PostInteractionResponseDTO
            {
                message = "Interaction created successfully",
                interactionId = newInteractionId
            };
            return Ok(response);
        }

        [HttpDelete]
        public IActionResult DeleteInteraction([FromBody] DeleteInteractionRequestDTO request)
        {
            // 1. Buscamos la interacción que se quiere borrar
            Interaction? interactionToDelete = df.CreateDAOInteraction().GetInteractionById(request.interactionId);
            if (interactionToDelete == null)
            {
                return NotFound(new { message = "Interaction not found" });
            }

            // 2. Borramos la interacción existente
            this.df.CreateDAOInteraction().DeleteInteraction(request.interactionId);

            // 3. 🚀 LÓGICA CLAVE: Verificamos si el usuario está cambiando de opinión
            // Si el tipo de interacción que se quiere crear es diferente al que se borró,
            // creamos la nueva interacción.
            if (request.interactionType.HasValue && request.interactionType != (int)interactionToDelete.InteractionType)
            {
                Interaction newInteraction = new Interaction
                {
                    PostId = interactionToDelete.PostId,
                    UserId = interactionToDelete.UserId,
                    InteractionType = (InteractionType)request.interactionType.Value
                };
                this.df.CreateDAOInteraction().CreateInteraction(newInteraction);

                // Devolvemos el ID de la nueva interacción creada
                return Ok(new { message = "Interaction updated successfully", interactionId = newInteraction.Id });
            }

            // Si solo se está quitando el like/dislike, devolvemos un éxito simple.
            DeleteInteractionResponseDTO response = new DeleteInteractionResponseDTO
            {
                message = "Interaction deleted successfully",
            };
            return Ok(response);
        }

        // [HttpGet]
        // public IActionResult GetUserInteraction([FromQuery] int postId, [FromQuery] int userId)
        // {
        //     var post = this.df.CreateDAOPost().GetPostById(postId);
        //     if (post == null)
        //         return NotFound(new { message = "Post no encontrado." });

        //     var interaction = post.Interactions.FirstOrDefault(i => i.User.Id == userId);

        //     if (interaction == null)
        //     {
        //         return Ok(new UserInteractionResponseDTO
        //         {
        //             InteractionId = null,
        //             Type = null
        //         });
        //     }

        //     return Ok(new UserInteractionResponseDTO
        //     {
        //         InteractionId = interaction.Id,
        //         Type = (int)interaction.InteractionType
        //     });
        // }
                







    }
}