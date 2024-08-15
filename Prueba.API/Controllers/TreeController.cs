using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Commands;
using Prueba.Application.Queries;

namespace Prueba.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class TreeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TreeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTree()
        {
            var tree = await _mediator.Send(new GetTreeQuery());
            return Ok(tree);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTreeUser(int id)
        {
            var tree = await _mediator.Send(new GetTreeQueryUser(id));
            return Ok(tree);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNode([FromBody] CreateTreeNodeCommand command)
        {
            var createdNodeId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTree), new { id = createdNodeId }, createdNodeId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNode([FromBody] UpdateTreeNodeCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        // Eliminar solo un nodo
        [HttpDelete("single/{id}")]
        public async Task<IActionResult> DeleteSingleNode(int id)
        {
            var command = new DeleteTreeNodeCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        // Eliminar un nodo y todos sus hijos
        [HttpDelete("cascade/{id}")]
        public async Task<IActionResult> DeleteNodeAndChildren(int id)
        {
            var command = new DeleteTreeNodeAndChildrenCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, int parentId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine("Uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var command = new CreateTreeNodeCommand
            {
                Name = fileName,
                IsFile = true,
                Path = filePath,
                ParentId = parentId
            };

            var newFileNodeId = await _mediator.Send(command);
            return Ok(newFileNodeId);
        }
    }