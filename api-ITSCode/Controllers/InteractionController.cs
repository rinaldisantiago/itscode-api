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

       

        [HttpPost]
        public IActionResult CreateInteraction([FromBody] PostInteractionRequestDTO request)
        {
            Post? post = df.CreateDAOPost().GetPostById(request.postId);
            User? user = df.CreateDAOUser().GetUser(request.userId);
            Interaction? interactionType = df.CreateDAOInteraction().GetInteractionById(request.interactionType);

            Interaction interaction = new Interaction
            {
                Post = post,
                User = user,
                InteractionType = request.interactionType,
            };

            this.df.CreateDAOInteraction().CreateInteraction(interaction);
            PostInteractionResponseDTO response = new PostInteractionResponseDTO
            {
                message = "Interaction created successfully",
            };
            return Ok(response);
        }







    }
}