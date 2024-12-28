using Database;
using Database.Models;
using DTOs;
using System.IdentityModel.Tokens.Jwt;
using SecurityLayer;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class UserService
    {
        private readonly AppDbContext _context;

        private readonly string DefaultImageURL = "https://res.cloudinary.com/dlu3aolnh/image/upload/v1735059932/udjkg3nflplvydkrngf0.png";

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO?> CheckUsernameAndPassword(LoginDTO loginDTO)
        {

            var User = await _context.Users
                               .Where(x => x.Username == loginDTO.username)
                               .FirstOrDefaultAsync();

            if (User == null)
            {
                return null;
            }

            if ( SecurityLayer.SecurityLayer.Decrypt(User.Password) != loginDTO.password)
            {
                return null;
            }

            UserDTO userDTO = new UserDTO()
            {
                UserId = User.UserId,
                UserName = User.Username,
                Email = User.Email,
                ProfileImageLink = User.ProfileImageLink,
                CommentsCount = User.CommentsCount,
                PostCount = User.PostCount
            };



            return userDTO;
        }

        public string CreateUserToken(int LoggedInUserId)
        {
            string NewGeneratedToken = Guid.NewGuid().ToString();

            var newToken = new Token()
            {
                CreatedToken = NewGeneratedToken,
                UserId = LoggedInUserId
            };

            _context.Tokens.Add(newToken);

            return _context.SaveChanges() > 0 ? NewGeneratedToken : string.Empty;
        }

        public UserDTO AddNewUser(RegisterDTO registerDTO, string? ProfileImgaeURL = null)
        {
            if (registerDTO == null)
            {
                throw new ArgumentNullException("There is no Data in UserInfo Object");
            }

            string EncryptedPassword = SecurityLayer.SecurityLayer.Encrypt(registerDTO.password);

            var NewUser = new User()
            {
                Username = registerDTO.username,
                Email = registerDTO.email,
                Password = EncryptedPassword,
                ProfileImageLink = ProfileImgaeURL == null ? DefaultImageURL : ProfileImgaeURL
            };

            _context.Users.Add(NewUser);
            _context.SaveChanges();

            return new UserDTO()
            {
               UserId = NewUser.UserId,
               UserName=NewUser.Username,
               Email = NewUser.Email,
               ProfileImageLink=NewUser.ProfileImageLink
            };
        }

        public bool LogoutUser(int UserId, string token)
        {
            try
            {
                var activeToken = TokenService.ReturnActiveTokenIfExists(_context, UserId, token);

                if (activeToken == null)
                {
                    return false;
                }

                activeToken.IsActive = false;
                _context.SaveChanges();

                var loginhistory = _context.LoginHistories.Where(L => L.UserId == UserId && L.LoginStatus != true)
                                                           .FirstOrDefault();
                if (loginhistory != null)
                {
                    loginhistory.LoginStatus = false;
                    _context.SaveChanges();
                }
       

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception($"Somthing Error while Logout Process!, {ex}");
            }

        }

        public bool UpdateUserInfo(UpdatedUserInfoDTO NewUserInfo, string token)
        {

            var activeToken = TokenService.ReturnActiveTokenIfExists(_context, NewUserInfo.UserId, token);

            if (activeToken == null)
            {
                return false;
            }

            var UpdatedUser = _context.Users.Find(NewUserInfo.UserId);


            if (UpdatedUser == null)
            {
                return false;
            };


            if (!string.IsNullOrEmpty(NewUserInfo.Email))
                UpdatedUser.Email = NewUserInfo.Email;

            if (!string.IsNullOrEmpty(NewUserInfo.ProfileImageLink))
                UpdatedUser.ProfileImageLink = NewUserInfo.ProfileImageLink;

            _context.SaveChanges();

            return true;

        }

        public UserDTO? GetUserById(int UserId)
        {

            var UserDTO = _context.Users.Where(u => u.UserId == UserId)
                          .Select(u => new UserDTO
                          {
                              UserId = u.UserId,
                              UserName = u.Username,
                              Email = u.Email,
                              ProfileImageLink = u.ProfileImageLink,
                              CommentsCount = u.CommentsCount,
                              PostCount = u.PostCount

                          }).FirstOrDefault();

            return UserDTO;
        }

        public async Task<string> HandleUserLoginAsync(int UserID)
        {
            string token = Guid.NewGuid().ToString();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var NewToken = new Token()
                {
                    CreatedToken = token,
                    UserId = UserID
                };

                _context.Tokens.Add(NewToken);
                await _context.SaveChangesAsync();

                var user = _context.Users.Find(UserID);
                if (user != null)
                {
                    user.LastUsedToken = token;
                    await _context.SaveChangesAsync();
                }

                var loginHistory = new LoginHistory()
                {
                    UserId = UserID,
                    LoginStatus = true
                };

                _context.LoginHistories.Add(loginHistory);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return token;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }


        }

        public bool IsValidEmail (string Email)
        {
            string [] validemailprefix = new string[] {"@gmail.com","@yahoo.com","@outlook.com" };

            foreach(var prefix in  validemailprefix)
            {
                if(Email.EndsWith(prefix))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
