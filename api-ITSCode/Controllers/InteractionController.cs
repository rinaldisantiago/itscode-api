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
            try
            {
                Post? post = this.df.CreateDAOPost().GetPostById(request.postId);
                User? user = this.df.CreateDAOUser().GetUser(request.userId);

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
                    PostId = request.postId, 
                    UserId = request.userId,  
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

            catch (Exception ex)
            {
                return StatusCode(500, 
                    new { 
                        message = "Error interno del servidor.",
                        error = ex.Message 
                    });
            }
        }

        [HttpDelete]
        public IActionResult DeleteInteraction([FromBody] DeleteInteractionRequestDTO request)
        {
            try
            {

                Interaction? interactionToDelete = df.CreateDAOInteraction().GetInteractionById(request.interactionId);
                if (interactionToDelete == null)
                {
                    return NotFound(new { message = "Interaction not found" });
                }


                this.df.CreateDAOInteraction().DeleteInteraction(request.interactionId);


                if (request.interactionType.HasValue && request.interactionType != (int)interactionToDelete.InteractionType)
                {
                    Interaction newInteraction = new Interaction
                    {
                        PostId = interactionToDelete.PostId,
                        UserId = interactionToDelete.UserId,
                        InteractionType = (InteractionType)request.interactionType.Value
                    };
                    this.df.CreateDAOInteraction().CreateInteraction(newInteraction);


                    return Ok(new { message = "Interaction updated successfully", interactionId = newInteraction.Id });
                }


                DeleteInteractionResponseDTO response = new DeleteInteractionResponseDTO
                {
                    message = "Interaction deleted successfully",
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