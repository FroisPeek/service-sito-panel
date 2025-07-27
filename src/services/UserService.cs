using leapcert_back.src.context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.Helpers;
using ServiceSitoPanel.src.dtos.users;
using ServiceSitoPanel.src.helpers;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.mappers.users;
using ServiceSitoPanel.src.model;
using static ServiceSitoPanel.src.responses.ResponseFactory;

namespace ServiceSitoPanel.src.services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserMapper _mapperUser;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, UserMapper mapperUser, JwtService jwtService, IConfiguration configuration)
        {
            _context = context;
            _mapperUser = mapperUser;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<IResponses> GetAllAsync()
        {
            ICollection<Users> users = await _context.users.ToListAsync();

            if (users.Count == 0)
                return new ErrorResponse(false, 404, "Nenhum usuario encontrado");

            return new SuccessResponse<IEnumerable<Users>>(true, 200, "Sucesso ao retornar usuarios", users);
        }

        public async Task<IResponses> PostAsync(CreateUserDto user)
        {
            if (user is null)
                return new ErrorResponse(false, 400, "Usuário não pode ser nulo");

            try
            {
                var userMapped = _mapperUser.MappUserDto(user, 1); // ! nao esquecer de alterar isso para criacao do tenant
                await _context.users.AddAsync(userMapped);
                await _context.SaveChangesAsync();

                return new SuccessResponse<Users>(true, 201, "Usuário criado com sucesso", userMapped);
            }
            catch (Exception ex)
            {
                return new ErrorResponse(false, 500, $"Erro ao criar usuário: {ex.Message}");
            }
        }

        public async Task<IResponses> Authenticate(LoginUserDto user, HttpContext context)
        {
            var existedUser = await _context.users
                .FirstOrDefaultAsync(u => u.name == user.username);

            if (existedUser is null)
                return new ErrorResponse(false, 404, "Nenhum usuario encontrado");

            if (existedUser.password != user.password)
                return new ErrorResponse(false, 400, "Senha incorreta");

            CreateUserSessionDTO userSession = new CreateUserSessionDTO(
                existedUser.id.ToString(),
                existedUser.name,
                existedUser.tenant_id.ToString()
            );

            ReadUserSessionDTO loggedUser = _jwtService.GenerateJwtToken(userSession);

            SetTokensInsideCookie(loggedUser.Token, context);

            return new SuccessResponse<ReadUserSessionDTO>(true, 200, "Usuário logado com sucesso!", loggedUser);
        }

        public IResponses ValidateToken(string token)
        {
            bool validationToken = _jwtService.ValidateToken(token);

            if (validationToken == false)
            {
                return new ErrorResponse(false, 400, "Token inválido");
            }

            return new SuccessResponse(true, 200, "Token validado com sucesso");
        }

        public void SetTokensInsideCookie(string token, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", token, new CookieOptions
            {
                Expires = DateTime.Now.NowInBrasilia().AddHours(5),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Domain = _configuration["Jwt:Domain"]
            });
        }
    }
}