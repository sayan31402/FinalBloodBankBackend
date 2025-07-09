using UserMicroservice.Models.DTO;

namespace UserMicroservice.Services.DAO
{
    public interface IAuthRepo
    {
        AuthResponseModel? Login(LoginModel loginModel);
    }
}
