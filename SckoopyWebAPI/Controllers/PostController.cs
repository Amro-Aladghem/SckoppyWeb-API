using DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace SckoopyWebAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly CommentService _commentService;
        private readonly FileUploadService _fileUploadService;
        private readonly AppSettingsService _appSettingsService;

        public PostController(PostService postService, CommentService commentService,
            FileUploadService fileUploadService, AppSettingsService appSettingsService)
        {
            _postService = postService;
            _commentService = commentService;
            _fileUploadService = fileUploadService;
            _appSettingsService = appSettingsService;
        }

        [HttpPost("posts",Name ="AddPost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<IActionResult> AddNewPost([FromForm] CreatePostDTO createPostDTO ,IFormFile Image)
        {
            string ? ImageURL = null;

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }

            var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            if(createPostDTO.UserId <=0)
            {
                return BadRequest("you are missing to add userId");
            }

            try
            {
                if(Image != null)
                {
                    using (var stream = Image.OpenReadStream())
                    {
                        ImageURL = await _fileUploadService.UploadImageAsync(stream, Image.FileName);
                    }
                }

                int NewPostId = _postService.AddNewPost(createPostDTO,ImageURL,token);
                PostDTO postDTO = _postService.GetPostByID(NewPostId);

                return Created("",new {data=postDTO});
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete("posts/{postid}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeletePost(int postid)
        {

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }

            var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            if (postid <= 0)
            {
                return BadRequest("Invalid or missing post id!");
            }

            try
            {
                bool IsDeleted = _postService.DeletePost(postid,token);

                return Ok(new { message = $"the Post ({postid}) was deleted successfully!)" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }


        [HttpPut("posts/{postid}", Name = "UpdatePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdatedPost(int postid,UpdatedPostDTO updatedPostDTO)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }

            var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            if (postid <= 0)
            {
                return BadRequest("Invalid or missing post id!");
            }

            try
            {
                bool IsUpdated = _postService.UpdatePost(postid, updatedPostDTO, token);
                PostDTO postDTO = _postService.GetPostByID(postid);

                return Ok(new {data=postDTO});
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("posts/{postid}", Name = "GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetPost(int postid)
        {
            if(postid<= 0)
            {
                return BadRequest(new { message = "Invalid post or missing post id" });
            }

            try
            {
                PostDTO post = _postService.GetPostByID(postid);
                return Ok(new { data = post });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpGet("posts", Name = "GetPosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetPosts(int page =1 ,int limit = 5)
        {
            try
            {
                PaginatedResultDTO<PostDTO> paginatedPosts = _postService.GetPosts(page, limit);
                var baseURL = _appSettingsService.baseURL;

                paginatedPosts.NextURL = paginatedPosts.NextPage != null ?
                                         $"{baseURL}/posts?page={page+1}&limit={limit}" : null;

                return Ok(new {data=paginatedPosts});

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);    
            }
        }

        [HttpGet("users/{userid}/posts", Name = "GetUserPosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserPosts(int userid)
        {
            if(userid <=0)
            {
                return BadRequest(new { message = "Invalid or missing User id !" });
            }

            try
            {
                List<PostDTO> postList = _postService.GetUserPosts(userid);

                return postList != null ? Ok(new { data = postList }) :
                    StatusCode(StatusCodes.Status500InternalServerError, "There is no Posts");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("posts/{postid}/comment", Name = "CreateComment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult AddNewComment(int postid,CreateCommentDTO createCommentDTO)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }

            var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            if (postid <= 0)
            {
                return BadRequest("Invalid or missing post id!");
            }

            if(string.IsNullOrEmpty(createCommentDTO.body))
            {
                return BadRequest("The body must not be null or empty !");
            }

            try
            {
                int NewCommentId = _commentService.AddNewComment(createCommentDTO, token, postid);
                CommentDTO comment = _commentService.GetCommentById(NewCommentId);

                return Created("",new { data = comment });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
