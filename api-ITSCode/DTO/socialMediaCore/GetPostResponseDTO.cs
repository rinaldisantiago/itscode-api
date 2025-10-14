public class UserInteractionDTO
{
    public int interactionId { get; set; }
    public int interactionType { get; set; } // 1 para Like, 2 para Dislike
}

public class GetPostResponseDTO
{
    public int id { get; set; }
    public int idUser { get; set; }
    public string title { get; set; } = "";
    public string content { get; set; } = "";

    public int likesCount { get; set; } = 0;
    public int dislikesCount { get; set; } = 0;
    public int commentsCount { get; set; } = 0;
    public string fileUrl { get; set; } = "";
    public string userName { get; set; } = "";
    public string userAvatar { get; set; } = "";

<<<<<<< HEAD
    public int idPost { get; set; }

    public UserInteractionDTO? userInteraction { get; set; } 

    public List<object> comments { get; set; }
=======
    public List<object> comments { get; set; } 
    public UserInteractionResponseDTO UserInteraction { get; set; }
>>>>>>> 36de3654731949cc15c25c596bdaa8d0ed742172

}
