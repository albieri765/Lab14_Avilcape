using Application.UseCases.Excel.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Excel Reports")]
public class ExcelController : ControllerBase
{
    private readonly IMediator _mediator;
    private const string ExcelContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public ExcelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Reporte de clientes con total de órdenes.</summary>
    [HttpGet("clients")]
    [SwaggerOperation(
        Summary     = "Reporte de Clientes",
        Description = "Genera un Excel con todos los clientes y su total de órdenes.",
        OperationId = "GetClientReport",
        Tags        = new[] { "Excel Reports" })]
    [SwaggerResponse(200, "Archivo Excel generado correctamente")]
    [SwaggerResponse(500, "Error interno")]
    public async Task<IActionResult> DownloadClientReport()
    {
        var bytes = await _mediator.Send(new GetClientReportQuery());
        return File(bytes, ExcelContentType, "reporte_clientes.xlsx");
    }

    /// <summary>Reporte de productos con precios.</summary>
    [HttpGet("products")]
    [SwaggerOperation(
        Summary     = "Reporte de Productos",
        Description = "Genera un Excel con todos los productos y sus precios.",
        OperationId = "GetProductReport",
        Tags        = new[] { "Excel Reports" })]
    [SwaggerResponse(200, "Archivo Excel generado correctamente")]
    [SwaggerResponse(500, "Error interno")]
    public async Task<IActionResult> DownloadProductReport()
    {
        var bytes = await _mediator.Send(new GetProductReportQuery());
        return File(bytes, ExcelContentType, "reporte_productos.xlsx");
    }
}