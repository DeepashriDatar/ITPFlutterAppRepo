using InTimePro.Api.Dtos;
using InTimePro.Api.Models;
using System.Collections.Generic;

namespace InTimePro.Api.Services
{
    public interface IAuthService
    {
        AuthResultDto Login(AuthDto request);

        RefreshResultDto Refresh(string refreshToken);

        bool Logout(string refreshToken);

        IEnumerable<Employee> GetEmployees();
    }
}
