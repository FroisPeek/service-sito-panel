using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.context;
using ServiceSitoPanel.src.dtos.users;
using serviceSidafWeb.Functions;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.responses;
using static ServiceSitoPanel.src.responses.ResponseFactory;

namespace ServiceSitoPanel.src.services
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDbContext _context;

        public SettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponses> ChangePassword(ChangePasswordDto changePasswordDto, int userId)
        {
            try
            {
                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
                    return new ErrorResponse(false, 400, "As senhas não conferem");

                var user = await _context.users.FirstOrDefaultAsync(u => u.id == userId);

                if (user == null)
                    return new ErrorResponse(false, 404, "Usuário não encontrado");

                if (user.password != HelperService.HashMd5(changePasswordDto.OldPassword))
                    return new ErrorResponse(false, 400, "Senha atual incorreta");

                // Update password
                user.password = HelperService.HashMd5(changePasswordDto.NewPassword);

                // Fix for Npgsql treating Unspecified dates as error
                if (user.created_at.Kind == DateTimeKind.Unspecified)
                {
                    user.created_at = DateTime.SpecifyKind(user.created_at, DateTimeKind.Utc);
                }
                
                _context.users.Update(user);
                await _context.SaveChangesAsync();

                return new SuccessResponse(true, 200, "Senha alterada com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorResponse(false, 500, $"Erro ao alterar senha: {ex.Message}");
            }
        }
    }
}
