using WEB.DTOs;
using WEB.Models;
using WEB.Models.Common;

namespace WEB.Interface.IRepository
{
    public interface IUserRepository
    {
        public Task<Response> GetUserById(int id);
        public Task<Response> UserLogin(UserDTO model);
        public Task UserRegister(UserDTO model);
        public Task<bool> FindUser(string username);
    }
}
