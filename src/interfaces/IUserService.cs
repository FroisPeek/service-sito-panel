using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.dtos.users;

namespace ServiceSitoPanel.src.interfaces
{
    public interface IUserService
    {
        Task<IResponses> GetAllAsync();
        Task<IResponses> PostAsync(CreateUserDto user);
        Task<IResponses> Authenticate(LoginUserDto user, HttpContext context);
        IResponses ValidateToken(string token);
        void DeleteTokenCookie(HttpContext context);
    }
}