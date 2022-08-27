using interships_Management.Models;

namespace interships_Management.Services;
public interface IAuthUserService{
    Task<AuthModel> RegistreUser (RegistreUser model);
    Task<AuthModel> Login (LoginModel model);


}