using Application.UseCases.Word.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab13_Avilcape.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Word Reports")]
public class WordController : ControllerBase
{
    private readonly IMediator _mediator;
    private const string WordContentType =
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public WordController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Reporte de clientes en Word.</summary>
    [HttpGet("clients")]
    [SwaggerOperation(
        Summary     = "Reporte Word de Clientes",
        Description = "Genera un Word con todos los clientes y su total de órdenes.",
        OperationId = "GetClientWordReport",
        Tags        = new[] { "Word Reports" })]
    [SwaggerResponse(200, "Archivo Word generado correctamente")]
    [SwaggerResponse(500, "Error interno")]
    public async Task<IActionResult> DownloadClientReport()
    {
        var bytes = await _mediator.Send(new GetClientWordReportQuery());
        return File(bytes, WordContentType, "reporte_clientes.docx");
    }

    /// <summary>Reporte de productos en Word.</summary>
    [HttpGet("products")]
    [SwaggerOperation(
        Summary     = "Reporte Word de Productos",
        Description = "Genera un Word con todos los productos y sus precios.",
        OperationId = "GetProductWordReport",
        Tags        = new[] { "Word Reports" })]
    [SwaggerResponse(200, "Archivo Word generado correctamente")]
    [SwaggerResponse(500, "Error interno")]
    public async Task<IActionResult> DownloadProductReport()
    {
        var bytes = await _mediator.Send(new GetProductWordReportQuery());
        return File(bytes, WordContentType, "reporte_productos.docx");
    }
}