using System.ComponentModel.DataAnnotations;

namespace Prueba.Core.Dtos;

public class UpdateUsernameDto
{
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; }
}