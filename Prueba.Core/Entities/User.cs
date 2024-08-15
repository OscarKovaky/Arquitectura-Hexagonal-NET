using System.ComponentModel.DataAnnotations;

namespace Prueba.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }

    [MinLength(6)]
    [MaxLength(100)]
    public string PasswordHash { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Role { get; set; }
}