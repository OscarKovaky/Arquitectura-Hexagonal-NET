using AutoMapper;
using Prueba.Core.Dtos;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;

namespace Prueba.Infrastructure.Services;

    public class TreeService : ITreeService
    {
        private readonly ITreeNodeRepository _treeNodeRepository;

        private readonly IMapper _mapper;

        public TreeService(ITreeNodeRepository treeNodeRepository, IMapper mapper)
        {
            _treeNodeRepository = treeNodeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TreeNodeDto>> GetUserNodes(int userId)
        {
            var nodes = await _treeNodeRepository.GetTreeByUserAsync(userId);
            return _mapper.Map<IEnumerable<TreeNodeDto>>(nodes);
        }

        public async Task<IEnumerable<TreeNodeDto>> GetTreeAsync()
        {
            var nodes = await _treeNodeRepository.GetTreeAsync();
            return _mapper.Map<IEnumerable<TreeNodeDto>>(nodes);
        }

        public async Task<TreeNodeDto> CreateNodeAsync(TreeNodeDto newNode)
        {
            var node = _mapper.Map<TreeNode>(newNode);
            await _treeNodeRepository.AddNodeAsync(node);
            newNode.Id = node.Id;
            return newNode;
        }

        public async Task UpdateNodeAsync(TreeNodeDto updatedNode)
        {
            var node = await _treeNodeRepository.GetNodeByIdAsync(updatedNode.Id);
            if (node != null)
            {
                _mapper.Map(updatedNode, node);
                await _treeNodeRepository.UpdateNodeAsync(node);
            }
        }

        public async Task DeleteNodeAsync(int id)
        {
            await _treeNodeRepository.DeleteNodeAsync(id);
        }

        public async Task DeleteNodeChildrenAsync(int id)
        {
            await _treeNodeRepository.DeleteNodeChildrenAsync(id);
        }

        public async Task<TreeNodeDto> CreateFileNodeAsync(string fileName, string filePath, int parentId)
        {
            var node = new TreeNode
            {
                Name = fileName,
                IsFile = true,
                Path = filePath,
                ParentId = parentId
            };

            await _treeNodeRepository.AddNodeAsync(node);

            return _mapper.Map<TreeNodeDto>(node);
        }
    }