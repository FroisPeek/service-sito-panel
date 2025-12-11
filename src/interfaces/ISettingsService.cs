using ServiceSitoPanel.src.dtos.users;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.interfaces
{
    public interface ISettingsService
    {
        Task<IResponses> ChangePassword(ChangePasswordDto changePasswordDto, int userId);
    }
}
