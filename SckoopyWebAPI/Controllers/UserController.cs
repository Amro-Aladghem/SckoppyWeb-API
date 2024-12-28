using Azure.Identity;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Reflection.Metadata.Ecma335;

namespace SckoopyWebAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly FileUploadService _fileUploadService;

        public UserController(UserService userService,FileUploadService fileUploadService)
        {
            _userService = userService;
            _fileUploadService = fileUploadService;
        }


        [HttpPost("Login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.username) || string.IsNullOrEmpty(loginDTO.password))
            {
                return BadRequest(new { message = "missing username or password" });
            }

            UserDTO ? userDTO = await _userService.CheckUsernameAndPassword(loginDTO);

            if(userDTO == null)
            {
                return Unauthorized(new { message = "password Or username  is not correct" });
            }


            string Token = await _userService.HandleUserLoginAsync(userDTO.UserId);

            return Ok(new { userDTO, token = Token });

        }

        [HttpPost("Register", Name = "Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser ([FromForm] RegisterDTO registerDTO , IFormFile ? Image)
        {
            string ? ProfileImageURL = null;
            

            if (string.IsNullOrEmpty(registerDTO.username) || string.IsNullOrEmpty(registerDTO.password) ||
                string.IsNullOrEmpty(registerDTO.email))
            {
                return BadRequest(new
                {
                    message = "missing Item",
                    username = registerDTO.username == "" ? "missing username" : registerDTO.username,
                    password = registerDTO.password == "" ? "missing password" : registerDTO.password,
                    email = registerDTO.email == "" ? "missing email" : registerDTO.email
                });
            }

            if(!_userService.IsValidEmail(registerDTO.email))
            {
                return BadRequest(new { message = "email is not valid, email should end with {@gmail.com,@yahoo.com,@outlook.com" });
            }

            try
            {
               
                if (Image != null)
                {
                    using (var stream = Image.OpenReadStream())
                    {
                        ProfileImageURL = await _fileUploadService.UploadImageAsync(stream, Image.FileName);
                    }
                }

                UserDTO userDTO = _userService.AddNewUser(registerDTO,ProfileImageURL);


                string Token = await _userService.HandleUserLoginAsync(userDTO.UserId);

                return Created("", new { userDTO, token = Token });

            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [HttpPost("Logout", Name = "Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public  IActionResult LogoutUser(LogoutDTO logoutDTO)
        {
            bool isLogout;


            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }

            var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            if(logoutDTO.UserId <= 0)
            {
                return BadRequest("missing User Id");
            }

            try
            {
                isLogout = _userService.LogoutUser((int)logoutDTO.UserId, token);

                return isLogout ? Ok(new { message = "Logout Successfully" }) : BadRequest(new { message = "token is not valid" });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("users/{id}", Name = "users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetUserById(int id)
        {
            UserDTO ? userDTO;

            if(id <= 0)
            {
                return BadRequest("Invalid User Id");
            }

            try
            { 
                userDTO = _userService.GetUserById(id);
                if(userDTO ==null)
                {
                    return NotFound(new { message = "No User Found With this Id!" });
                }

                return Ok(new {data=userDTO});
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("updateProfile", Name = "updateProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateUserProfile(UpdatedUserInfoDTO newUserInfo)
        {
            bool IsUpdated;

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return BadRequest("Authorization header is missing");
            }

            var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            if(newUserInfo.UserId <=0)
            {
                return BadRequest("Invalid User Id");
            }

            try
            {
                IsUpdated = _userService.UpdateUserInfo(newUserInfo, token);

                UserDTO ? UpdatedUser = IsUpdated ? _userService.GetUserById(newUserInfo.UserId) : null;

                return IsUpdated ?
                        Ok(new { message = "User Updated Succesfully", UpdatedUser })
                        : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message); 
            }
        }


    }
}
