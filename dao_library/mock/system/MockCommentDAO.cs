using entity_library;
using System.Collections.Generic;
using System.Linq;

namespace dao_library
{
    public class MockCommentDAO : DAOCommet
    {
        private  List<Comment> _Comments = new List<Comment>();

        public MockCommentDAO()
        {
            // Comentarios de prueba
            _Comments.Add(new Comment { IdComment = 1, PostId = 1, Content = "¡Gran post!" });
            _Comments.Add(new Comment { IdComment = 2, PostId = 2, Content = "Me encanta la lluvia." });
        }

        public Comment GetCommentById(int id)
        {
            return _Comments.FirstOrDefault(c => c.IdComment == id);
        }

        public void Save(Comment comment)
        {
            comment.IdComment = _Comments.Max(c => c.IdComment) + 1;
            _Comments.Add(comment);
        }

        public void UpdateComment(Comment comment)
        {
            var existingComment = GetCommentById(comment.IdComment);
            if (existingComment != null)
            {
                existingComment.PostId = comment.PostId;
                existingComment.Content = comment.Content;
            }
        }

        public void DeleteComment(int id)
        {
            var comment = GetCommentById(id);
            if (comment != null)
            {
                _Comments.Remove(comment);
            }
        }

        public IEnumerable<Comment> GetAllComments()
        {
            return _Comments;
        }

        public void CreateComment(Comment comment)
        {
            Save(comment);
        }
    }
}
