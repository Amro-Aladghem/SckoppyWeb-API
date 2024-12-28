using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using DTOs;

namespace Service
{
    public class TokenService
    {

        public static Token? ReturnActiveTokenIfExists(AppDbContext _context,int userId, string token)
        {
            return _context.Tokens.FirstOrDefault(t => t.UserId == userId && t.IsActive && t.CreatedToken == token);
        }
    }
}
