namespace Prueba.Core.Dtos;

public class TreeNodeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsFile { get; set; }
    public string Path { get; set; }
    public int? ParentId { get; set; }
    
    public int UserId { get; set; } // Referencia al usuario
    public List<TreeNodeDto> Children { get; set; } = new List<TreeNodeDto>();
}