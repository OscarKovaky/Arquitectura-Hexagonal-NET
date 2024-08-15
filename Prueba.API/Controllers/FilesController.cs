
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using MediatR;
using Prueba.Application.Queries;

namespace Prueba.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController: ControllerBase
{   
    
    private readonly IMediator _mediator;
    private readonly string _fileDirectory = Path.Combine("C:\\", "FilesUsers");
    
    public FilesController(IMediator mediator)
    {
        _mediator = mediator;
        if (!Directory.Exists(_fileDirectory))
        {
            Directory.CreateDirectory(_fileDirectory);
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file,[FromForm] string path)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // Normalizar la ruta proporcionada por el usuario
        string sanitizedPath = NormalizePath(path);

        // Combinar la ruta raíz _fileDirectory con la ruta proporcionada por el usuario
        string fullPath = Path.Combine(_fileDirectory, sanitizedPath);

        // Asegurarse de que el directorio exista
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        // Ruta completa incluyendo el nombre del archivo
        string filePath = Path.Combine(fullPath, file.FileName);

        // Guardar el archivo en la ruta especificada
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { filePath });
    }
    
    
    // Generación y Descarga de Archivo Excel
    [HttpGet("export-users")]
    public async Task<IActionResult> ExportUsersToExcel()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Usuarios");
            worksheet.Cells["A1"].Value = "Id";
            worksheet.Cells["B1"].Value = "Username";
            worksheet.Cells["C1"].Value = "Email";
            worksheet.Cells["D1"].Value = "Role";

            int row = 2;
            foreach (var user in users)
            {
                worksheet.Cells[row, 1].Value = user.Id;
                worksheet.Cells[row, 2].Value = user.Username;
                worksheet.Cells[row, 3].Value = user.Email;
                worksheet.Cells[row, 4].Value = user.Role;
                row++;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Usuarios_{System.DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
    
    private string NormalizePath(string path)
    {
        // Reemplazar '/' por '\' para compatibilidad con Windows
        string normalizedPath = path.Replace('/', '\\');

        // Eliminar caracteres inválidos usando Regex (caracteres como * ? < > | " :)
        string sanitizedPath = Regex.Replace(normalizedPath, @"[<>:""/\\|?*]", string.Empty);

        // Retornar la ruta normalizada
        return sanitizedPath;
    }
}