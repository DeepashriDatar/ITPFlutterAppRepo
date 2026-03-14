using System;

namespace InTimePro.Api.Models
{
    public class RefreshTokenRecord
    {
        public string Token { get; set; }

        public Guid EmployeeId { get; set; }

        public DateTime ExpiresAtUtc { get; set; }

        public bool Revoked { get; set; }
    }
}
