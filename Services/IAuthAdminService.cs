using interships_Management.Models;

namespace interships_Management.Services;

public interface IAuthAdminService {
        Task<AuthModel> RegistreAdmin (RegistreAdmin model);
        Task<AuthModel> Login (LoginModel model);

        
}