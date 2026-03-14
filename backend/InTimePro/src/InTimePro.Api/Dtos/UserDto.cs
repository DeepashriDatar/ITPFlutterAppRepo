using System;

namespace InTimePro.Api.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public string Role { get; set; }
    }
}
