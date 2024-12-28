using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using DTOs;


namespace Service
{
    public class CommentService
    {

        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            this._context = context;
        }

        public int AddNewComment(CreateCommentDTO createCommentDTO,string token,int PostId)
        {

           var activetoken = TokenService.ReturnActiveTokenIfExists(_context, createCommentDTO.UserId, token);

           if (activetoken == null)
           {
               throw new Exception("Unauthenticated");
           }

           var newComment = new Comment()
           {
               Body = createCommentDTO.body,
               UserId =createCommentDTO.UserId,
               PostId = PostId
           };

           _context.Comments.Add(newComment);
           _context.SaveChanges();

            var post = _context.Posts.Find(newComment.PostId);
            post.CommentCount++;
            _context.SaveChanges();

            var user = _context.Users.Find(newComment.UserId);
            user.CommentsCount++;
            _context.SaveChanges();

           return newComment.CommentId;

        }

        public CommentDTO GetCommentById (int CommentId)
        {
            var Comment = _context.Comments.Where(c=>c.CommentId==CommentId)
                                           .Select(c=>new CommentDTO
                                           {
                                               CommentId = c.CommentId,
                                               UserId = c.UserId,
                                               PostId = c.PostId,
                                               Body = c.Body,
                                               CreatedAt = c.CreateAt
                                           })
                                           .FirstOrDefault();

            return Comment;
        }

    }
}
