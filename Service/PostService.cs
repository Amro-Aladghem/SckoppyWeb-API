using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using DTOs;
using Microsoft.EntityFrameworkCore;


namespace Service
{
    public class PostService
    {
        private readonly AppDbContext _context;
        
       
        public PostService(AppDbContext context)
        {
            this._context = context;
        }

        public PostDTO TransferPostEntityToDTO(Post postEntity)
        {
            return new PostDTO
            {
                PostId = postEntity.PostId,
                Title = postEntity.Title,
                Body = postEntity.Body,
                PostImageLink = postEntity.PostImageLink,
                CreatedDateTime = postEntity.CreatedDateTime,
                LikesCount = postEntity.LikesCount,
                CommentCount = postEntity.CommentCount,
                LastUpdatedDateTime = postEntity.LastUpdatedDate,

                author = new UserDTO
                {
                    UserId = postEntity.User.UserId,
                    UserName = postEntity.User.Username,
                    ProfileImageLink = postEntity.User.ProfileImageLink,
                    Email = postEntity.User.Email,
                    CommentsCount = postEntity.User.CommentsCount,
                    PostCount = postEntity.User.PostCount
                },

                Tags = postEntity.PostTags.Select(t => new TagDTO
                {
                    TagId = t.TagId,
                    Name = t.Tag.Name,
                    Description = t.Tag.Description
                }).ToList(),

                Comments = postEntity.Comments.Select(c => new CommentDTO
                {
                    CommentId = c.CommentId,
                    Body = c.Body,
                    CreatedAt = c.CreateAt,
                    UserId=c.UserId,
                    PostId=c.PostId,
                    author = new UserDTO
                    {
                        UserId = c.User.UserId,
                        UserName = c.User.Username,
                        ProfileImageLink = c.User.ProfileImageLink,
                        Email = c.User.Email,
                        CommentsCount = c.User.CommentsCount,
                        PostCount = c.User.PostCount
                    },


                }).ToList()

            };
        }

        public int  AddNewPost(CreatePostDTO NewPostInfo,string ? ImageURL, string token)
        {

            var activetoken = TokenService.ReturnActiveTokenIfExists(_context,NewPostInfo.UserId,token);   
            
            if (activetoken == null)
            {
                throw new Exception("Error with your sending data, please check data!");
            }

            var NewPost = new Post()
            {
                Title = NewPostInfo.Title,
                Body = NewPostInfo.Body,
                PostImageLink = ImageURL,
                UserId = NewPostInfo.UserId
            };
            
            _context.Posts.Add(NewPost);
            _context.SaveChanges();

            var user = _context.Users.Find(NewPost.UserId);
            user.PostCount++;
            _context.SaveChanges();

            return NewPost.PostId;
        }

        public bool UpdatePost(int PostId,UpdatedPostDTO NewPostInfo,string token)
        {

            var Post = _context.Posts.Find(PostId);

            if(Post==null)
            {
                throw new Exception("No Post With this Id!");
            }

            if(TokenService.ReturnActiveTokenIfExists(_context,Post.UserId,token)==null)
            {
                throw new Exception("You don't have permission to update post this post!");
            }

            if (string.IsNullOrEmpty(Post.Title) && string.IsNullOrEmpty(Post.PostImageLink) 
                && string.IsNullOrEmpty(NewPostInfo.body))
            {
                throw new Exception("Your post doesn't hava any content!");
            }

             Post.Body = NewPostInfo.body;
             Post.LastUpdatedDate=DateTime.Now;
             _context.SaveChanges();

             return true;
        }

        public bool DeletePost(int PostId,string token)
        {

                var postentity = _context.Posts.Where(p => p.PostId == PostId)
                                               .FirstOrDefault();

                if (postentity == null)
                {
                    throw new Exception("No PostId match you sending Id!");
                }

                if (TokenService.ReturnActiveTokenIfExists(_context, postentity.UserId, token) == null)
                {
                    throw new Exception("You don't have permission to delete post this post!");
                }

                _context.Posts.Remove(postentity);
                _context.SaveChanges();

                var user = _context.Users.Find(postentity.UserId);
                user.PostCount--;
                _context.SaveChanges();

                return true;
         
        }

        public PostDTO GetPostByID(int PostID)
        {

            var PostEntity = _context.Posts.Where(p => p.PostId == PostID)
                                           .Include(p => p.User)
                                           .Include(p => p.Comments)
                                           .ThenInclude(c=>c.User)
                                           .Include(p => p.PostTags)
                                           .ThenInclude(pt => pt.Tag)
                                           .FirstOrDefault();
                                           
            
             if (PostEntity is null)
             {
                 throw new Exception("No Post With this ID !");
             }
            
            
             PostDTO PostEntityDTO = TransferPostEntityToDTO(PostEntity);
            
             return PostEntityDTO;
            
           
        }

        public PaginatedResultDTO<PostDTO> GetPosts(int PageNumber,int Limit)
        {

                var Posts = _context.Posts
                                .OrderByDescending(p => p.PostId)
                                .Skip((PageNumber - 1) * Limit)
                                .Take(Limit)
                                .Select(p=> new PostDTO
                                {
                                    PostId = p.PostId,
                                    Title = p.Title,
                                    Body = p.Body,
                                    PostImageLink = p.PostImageLink,
                                    CreatedDateTime = p.CreatedDateTime,
                                    LikesCount = p.LikesCount,
                                    CommentCount = p.CommentCount,

                                    author = new UserDTO
                                    {
                                        UserId = p.User.UserId,
                                        UserName = p.User.Username,
                                        ProfileImageLink = p.User.ProfileImageLink,
                                        Email = p.User.Email
                                    }

                                }).ToList();



                int PostsCount = _context.Posts.Count();
                bool hasNextPage = (PageNumber * Limit) < PostsCount;

            return new PaginatedResultDTO<PostDTO>
            {
                Posts = Posts,
                PreviousPage = PageNumber - 1 != 0 ? PageNumber - 1 : null,
                CurrentPage = PageNumber,
                NextPage = hasNextPage ? PageNumber + 1 : (int?)null,
                TotalCount = PostsCount

            };

        }

        public List<PostDTO>  GetUserPosts(int UserId)
        {
            var postlist = _context.Posts.Where(p => p.UserId == UserId)
                                         .Select(p => new PostDTO
                                         {
                                             PostId = p.PostId,
                                             Title = p.Title,
                                             Body = p.Body,
                                             PostImageLink = p.PostImageLink,
                                             CreatedDateTime = p.CreatedDateTime,
                                             LikesCount = p.LikesCount,
                                             CommentCount = p.CommentCount,

                                             author = new UserDTO
                                             {
                                                 UserId = p.User.UserId,
                                                 UserName = p.User.Username,
                                                 ProfileImageLink = p.User.ProfileImageLink,
                                                 Email = p.User.Email
                                             }

                                         }).ToList();

            return postlist;
        }


    }
}
