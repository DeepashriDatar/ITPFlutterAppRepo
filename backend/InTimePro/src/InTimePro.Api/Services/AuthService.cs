using InTimePro.Api.Dtos;
using InTimePro.Api.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InTimePro.Api.Services
{
    public class AuthService : IAuthService
    {
        private static readonly List<Employee> Employees = new List<Employee>
        {
            new Employee
            {
                Id = Guid.NewGuid(),
                Email = "employee@intimepro.com",
                Name = "Default Employee",
                Department = "Engineering",
                Role = "Employee",
                IsActive = true,
                PasswordHash = Hash("Password@123")
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                Email = "admin@intimepro.com",
                Name = "Default Admin",
                Department = "Administration",
                Role = "Admin",
                IsActive = true,
                PasswordHash = Hash("Admin@123")
            }
        };

        private static readonly ConcurrentDictionary<string, RefreshTokenRecord> RefreshTokens =
            new ConcurrentDictionary<string, RefreshTokenRecord>(StringComparer.Ordinal);

        public AuthResultDto Login(AuthDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
            {
                return null;
            }

            var provider = (request.Provider ?? "email").Trim().ToLowerInvariant();
            var employee = Employees.FirstOrDefault(e =>
                e.Email.Equals(request.Email.Trim(), StringComparison.OrdinalIgnoreCase) && e.IsActive);

            if (employee == null)
            {
                return null;
            }

            if (provider == "email")
            {
                if (string.IsNullOrWhiteSpace(request.Password) || employee.PasswordHash != Hash(request.Password))
                {
                    return null;
                }
            }
            else if (provider != "google" && provider != "microsoft")
            {
                return null;
            }

            var tokenPair = CreateTokenPair(employee.Id);

            return new AuthResultDto
            {
                User = new UserDto
                {
                    Id = employee.Id,
                    Email = employee.Email,
                    Name = employee.Name,
                    Department = employee.Department,
                    Role = employee.Role
                },
                Tokens = tokenPair
            };
        }

        public RefreshResultDto Refresh(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return null;
            }

            if (!RefreshTokens.TryGetValue(refreshToken, out var record))
            {
                return null;
            }

            if (record.Revoked || record.ExpiresAtUtc <= DateTime.UtcNow)
            {
                return null;
            }

            var accessToken = CreateAccessToken(record.EmployeeId);
            return new RefreshResultDto
            {
                AccessToken = accessToken,
                ExpiresIn = 3600
            };
        }

        public bool Logout(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            if (!RefreshTokens.TryGetValue(refreshToken, out var record))
            {
                return false;
            }

            record.Revoked = true;
            return true;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return Employees;
        }

        private static TokenPairDto CreateTokenPair(Guid employeeId)
        {
            var accessToken = CreateAccessToken(employeeId);
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + "." + Guid.NewGuid().ToString("N");

            RefreshTokens[refreshToken] = new RefreshTokenRecord
            {
                Token = refreshToken,
                EmployeeId = employeeId,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
                Revoked = false
            };

            return new TokenPairDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600
            };
        }

        private static string CreateAccessToken(Guid employeeId)
        {
            var payload = employeeId + ":" + DateTime.UtcNow.AddHours(1).Ticks;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
        }

        private static string Hash(string text)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text ?? string.Empty));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
