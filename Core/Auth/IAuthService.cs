using Core.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
        Task RegisterAsync(RegisterDto dto);
    }
}
