using Prueba.Core.Dtos;

namespace Prueba.Core.Interfaces;

public interface ITreeService
{
    Task<IEnumerable<TreeNodeDto>> GetUserNodes(int userId);
    Task<IEnumerable<TreeNodeDto>> GetTreeAsync();
    Task<TreeNodeDto> CreateNodeAsync(TreeNodeDto newNode);
    Task UpdateNodeAsync(TreeNodeDto updatedNode);
    Task DeleteNodeAsync(int id);

    Task DeleteNodeChildrenAsync(int id);
    Task<TreeNodeDto> CreateFileNodeAsync(string fileName, string filePath, int parentId);
}