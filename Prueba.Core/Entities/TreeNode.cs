using System.ComponentModel.DataAnnotations;

namespace Prueba.Core.Entities;

public class TreeNode
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsFile { get; set; }
    public string Path { get; set; }
    public int? ParentId { get; set; }
    public int UserId { get; set; } 
}