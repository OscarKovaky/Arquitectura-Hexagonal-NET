using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Queries.Categorias;
using Prueba.Core.Dtos;

namespace Prueba.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
    {
        // Creamos la query que será manejada por MediatR
        var query = new GetCategoriasQuery();

        // Enviamos la query a través de MediatR y obtenemos el resultado
        var categorias = await _mediator.Send(query);

        // Retornamos el resultado en la respuesta
        return Ok(categorias);
    }
}