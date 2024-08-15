using Prueba.Core.Entities;

namespace Prueba.Core.Interfaces;

public interface ITreeNodeRepository
{
    Task<IEnumerable<TreeNode>> GetTreeByUserAsync(int userId);
    Task<IEnumerable<TreeNode>> GetTreeAsync();
    Task<TreeNode> GetNodeByIdAsync(int id);
    Task AddNodeAsync(TreeNode node);
    Task UpdateNodeAsync(TreeNode node);
    Task DeleteNodeAsync(int id);

    Task DeleteNodeChildrenAsync(int id);
}