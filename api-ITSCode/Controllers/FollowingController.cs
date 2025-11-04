using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;

namespace apiFollowing.Controllers
{
    [ApiController]
    [Route("Following")]
    public class FollowingController : ControllerBase
    {
        private readonly ILogger<FollowingController> _logger;
        private DAOFactory df;

        public FollowingController(ILogger<FollowingController> logger, DAOFactory df)
        {
            _logger = logger;
            this.df = df;
        }

        [HttpPost]
        public IActionResult FollowUser([FromQuery] FollowRequestDTO request)
        {
            User userFollowing = this.df.CreateDAOUser().GetUser(request.userFollowingId);
            User userFollowed = this.df.CreateDAOUser().GetUser(request.userFollowedId);

            if (userFollowing == null || userFollowed == null)
            {
                return NotFound("User not found");
            }

            Following following = new Following
            {
                UserFollowing = userFollowing,
                UserFollowed = userFollowed
            };

            this.df.CreateDAOFollowing().CreateFollowing(following);

            FollowResponseDTO response = new FollowResponseDTO
            {
                message = "Followed successfully",
                userFollowing = userFollowing.UserName,
                userFollowed = userFollowed.UserName
            };

            return Ok(new { response });
        }
        

        [HttpDelete]
        public IActionResult UnfollowUser([FromQuery] UnfollowRequestDTO request)
        {
            User? userFollowing = this.df.CreateDAOUser().GetUser(request.userFollowingId);
            User? userFollowed = this.df.CreateDAOUser().GetUser(request.userFollowedId);

            if (userFollowing == null || userFollowed == null)
            {
                return NotFound("User not found");
            }

            this.df.CreateDAOFollowing().DeleteFollowing(request.userFollowingId, request.userFollowedId);

            UnfollowResponseDTO response = new UnfollowResponseDTO
            {
                message = "Unfollow successfully",
                userFollowing = userFollowing.UserName,
                userFollowed = userFollowed.UserName
            };

            return Ok(response);
        }
    } 
}