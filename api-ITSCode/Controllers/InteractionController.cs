using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;


namespace apiUser.Controllers
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

            // Controlo si la interaccion corresponde a un valor del enum InteractionType
            if (!Enum.IsDefined(typeof(InteractionType), request.interactionType))
            {
                return BadRequest(new { message = "Tipo de interacción inválido." });
            }
                    

            Interaction interaction = new Interaction
            {
                Post = post,
                User = user,
                InteractionType = (InteractionType)request.interactionType
            };

            this.df.CreateDAOInteraction().CreateInteraction(interaction);

            PostInteractionResponseDTO response = new PostInteractionResponseDTO
            {
                message = "Interaction created successfully",
                interactionId = interaction.Id
            };
            return Ok(response);
        }

        [HttpDelete]
        public IActionResult DeleteInteraction([FromBody] DeleteInteractionRequestDTO request)
        {
            Interaction? interaction = df.CreateDAOInteraction().GetInteractionById(request.interactionId);
            if (interaction == null)
            {
                return NotFound(new { message = "Interaction not found" });
            }

            this.df.CreateDAOInteraction().DeleteInteraction(request.interactionId);
            DeleteInteractionResponseDTO response = new DeleteInteractionResponseDTO
            {
                message = "Interaction deleted successfully",
            };
            return Ok(response);
        }
        







    }
}