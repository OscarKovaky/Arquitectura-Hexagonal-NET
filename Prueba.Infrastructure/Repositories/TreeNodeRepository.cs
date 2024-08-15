using Dapper;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;
using Prueba.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.Infrastructure.Repositories;

  public class TreeNodeRepository : ITreeNodeRepository
    {
        private readonly DatabaseContext _context;

        public TreeNodeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TreeNode>> GetTreeByUserAsync(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "SELECT * FROM public.GetUserNodes(@p_userid)";
                var parameters = new { p_userid = userId };
        
                var treeNodes = await connection.QueryAsync<TreeNode>(sql, parameters);
                return treeNodes;
            }
        }


        public async Task<IEnumerable<TreeNode>> GetTreeAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "SELECT * FROM public.GetTreeNodes()";
                var treeNodes = await connection.QueryAsync<TreeNode>(sql);
                return treeNodes;
            }
        }

        public async Task<TreeNode> GetNodeByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_id", id, DbType.Int32);
                var treeNode = await connection.QuerySingleOrDefaultAsync<TreeNode>(
                    "SELECT * FROM public.GetTreeNodeById(@p_id)", 
                    parameters
                );
                return treeNode;
            }
        }

        public async Task AddNodeAsync(TreeNode node)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_parentid", node.ParentId, DbType.Int32);
                parameters.Add("p_name", node.Name, DbType.String);
                parameters.Add("p_userid", node.UserId, DbType.Int32);
                parameters.Add("p_isfile", node.IsFile, DbType.Boolean);
                parameters.Add("p_path", node.Path, DbType.String);

                node.Id = await connection.ExecuteScalarAsync<int>(
                    "SELECT CreateTreeNode(@p_parentid, @p_name, @p_userid, @p_isfile, @p_path)",
                    parameters
                );
            }
        }

        public async Task UpdateNodeAsync(TreeNode node)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_id", node.Id, DbType.Int32);
                parameters.Add("p_parentid", node.ParentId, DbType.Int32);
                parameters.Add("p_name", node.Name, DbType.String);
                parameters.Add("p_userid", node.UserId, DbType.Int32);  // Assuming `UserId` is part of `TreeNode`
                await connection.ExecuteAsync(
                    "CALL public.UpdateTreeNode(@p_id, @p_parentid, @p_name, @p_userid)", 
                    parameters
                );
            }
        }

        public async Task DeleteNodeAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_node_id", id, DbType.Int32);

                await connection.ExecuteAsync("SELECT DeleteSingleNode(@p_node_id)", parameters);
            }
        }
        
        public async Task DeleteNodeChildrenAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_node_id", id, DbType.Int32);

                await connection.ExecuteAsync("SELECT DeleteTreeNodeAndChildren(@p_node_id)", parameters);
            }
        }
    }   
