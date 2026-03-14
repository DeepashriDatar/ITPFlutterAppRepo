using System.ComponentModel.DataAnnotations;

namespace InTimeProAPI.Models;

public class Employee
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(50)]
    public string Role { get; set; } = "Employee";

    [MaxLength(20)]
    public string? Phone { get; set; }

    public string? AvatarUrl { get; set; }

    // Auth fields
    public string? PasswordHash { get; set; }
    public string? GoogleId { get; set; }
    public string? MicrosoftId { get; set; }
    public string AuthProvider { get; set; } = "email"; // email | google | microsoft

    public bool IsActive { get; set; } = true;
    public int FailedLoginCount { get; set; } = 0;
    public DateTime? LockoutEndUtc { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
