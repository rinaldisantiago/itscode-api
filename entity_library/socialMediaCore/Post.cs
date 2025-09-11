using Microsoft.VisualBasic;

namespace entity_library;

public class Post
{
    // private int id;
    // private User? user;
    // private string title;
    // private string content;
    // private List<Interaction>? interactions;
    // private File? file;
    // private List<Comment>? comments;


    // public int Id { get { return this.id; } set { this.id = value; } }
    // public User? User { get { return this.user; } set { this.user = value; } }
    // public string Title { get { return this.title; } set { this.title = value; } }
    // public string Content { get { return this.content; } set { this.content = value; } }
    // public List<Interaction>? Interactions { get { return this.interactions; } set { this.interactions = value; } }
    // public File? File { get { return this.file; } set { this.file = value; } }
    // public List<Comment>? Comments { get { return this.comments; } set { this.comments = value; } }


        private int id;
        public int Id { get { return this.id; } set { this.id = value; } }
        private string title = "";
        public string Title { get { return this.title; } set { this.title = value; } }
        private string content = "";
        public string Content { get { return this.content; } set { this.content = value; } }
        private User? user;
        public User? User { get { return this.user; } set { this.user = value; } }
        private DateTime CreatedAt { get; set; }
        public DateTime createdAt { get { return this.CreatedAt; } set { this.CreatedAt = value; } }

        public int IdUser
    {
        get
        {
            if (this.User != null)
            {
                return this.User.Id;
            }
            return 0;
        }
    }

        public string UserFullName
        {
            get
            {
                if (this.User != null)
                {
                    return this.User.FullName;
                }
                return "Sin usuario";
            }
        }

        public string UserName
        {
            get
            {
                if (this.User != null)
                {
                    return this.User.UserName;
                }
                return "Sin usuario";
            }
        }


        public string userAvatar
        {
            get
            {
                if (this.User != null && this.User.Avatar != null)
                {
                    return this.User.Avatar.Url;
                }
                return "No se encuentra ninguna imagen";
            }
        }


        private List<Interaction>? interactions;
        public List<Interaction>? Interactions { get { return this.interactions; } set { this.interactions = value; } }

        // public object GetInteractions()
        // {
        //     if (this.Interactions == null)
        //     {
        //         return new List<object>();
        //     }
        //     return this.Interactions.Select(interaction => new
        //     {
        //         Id = interaction.Id,
        //         UserName = interaction.User,
        //         InteractionType = interaction.InteractionType.1 ?? "Sin tipo de interacción",
                
        //     })
        //     .Cast<object>()
        //     .ToList();
        // }
        public int GetCountLike()
        {
            if (this.Interactions == null) return 0;
            return this.Interactions?.Count(interaction => interaction.InteractionType == InteractionType.Like) ?? 0;
        }

        public int GetCountDislike()
        {
            if (this.Interactions == null) return 0;
            return this.Interactions?.Count(interaction => interaction.InteractionType == InteractionType.Dislike) ?? 0;
        }


        private List<Comment>? comments;
        public List<Comment>? Comments { get { return this.comments; } set { this.comments = value; } }

        public List<object> GetComments()
        {
            if (this.Comments == null)
            {
                return new List<object>();
            }
            return this.Comments.Select(comment => new
            {
                Id = comment.Id,
                UserName = comment.User.FullName,
                userAvatar = comment.User.Avatar.Url ?? "",
                Content = comment.Content,

            })
            .Cast<object>()
            .ToList();
        }
        public int GetCountComments()
        {
            if (this.Comments == null) return 0;
            return this.Comments.Where(comment => comment.Content != null && comment.Content.Trim() != "").Count();
        }


        private File? file;
        public File? File { get { return this.file; } set { this.file = value; } }
        public long IdFile
        {
            get
            {
                if (this.File != null)
                {
                    return this.File.Id;
                }
                return 0;
            }
        }
        public string GetUrlImage()
        {
            if (this.File != null && this.File.Url != null)
            {
                return this.File.Url;
            }
            return "No se encuentra ninguna imagen";
        }
}
