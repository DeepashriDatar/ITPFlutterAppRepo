using System;

namespace InTimePro.Api.Models
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public string PasswordHash { get; set; }
    }
}
