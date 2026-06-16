using Application.UseCases.Excel.Queries;
using Infrastructure;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Excel Reports")]
public class ExcelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _context;
    private const string ExcelContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public ExcelController(IMediator mediator, AppDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet("clients")]
    [SwaggerOperation(Summary = "Reporte de Clientes", OperationId = "GetClientReport", Tags = new[] { "Excel Reports" })]
    [SwaggerResponse(200, "Archivo Excel generado correctamente")]
    [SwaggerResponse(500, "Error interno")]
    public async Task<IActionResult> DownloadClientReport()
    {
        var bytes = await _mediator.Send(new GetClientReportQuery());
        return File(bytes, ExcelContentType, "reporte_clientes.xlsx");
    }

    [HttpGet("products")]
    [SwaggerOperation(Summary = "Reporte de Productos", OperationId = "GetProductReport", Tags = new[] { "Excel Reports" })]
    [SwaggerResponse(200, "Archivo Excel generado correctamente")]
    [SwaggerResponse(500, "Error interno")]
    public async Task<IActionResult> DownloadProductReport()
    {
        var bytes = await _mediator.Send(new GetProductReportQuery());
        return File(bytes, ExcelContentType, "reporte_productos.xlsx");
    }

    [HttpPost("seed")]
    [SwaggerOperation(Summary = "Insertar datos de prueba", OperationId = "SeedData", Tags = new[] { "Excel Reports" })]
    public async Task<IActionResult> SeedData()
    {
        if (await _context.Clients.AnyAsync())
            return BadRequest("Ya existen datos en la base de datos.");

        var clients = new List<Client>
        {
            new() { Name = "Juan Pérez", Email = "juan.perez@gmail.com" },
            new() { Name = "María García", Email = "maria.garcia@gmail.com" },
            new() { Name = "Carlos López", Email = "carlos.lopez@gmail.com" },
            new() { Name = "Ana Martínez", Email = "ana.martinez@gmail.com" },
            new() { Name = "Luis Torres", Email = "luis.torres@gmail.com" }
        };

        var products = new List<Product>
        {
            new() { Name = "Laptop HP", Description = "Laptop 15 pulgadas Intel Core i5", Price = 2500.00m },
            new() { Name = "Mouse Logitech", Description = "Mouse inalámbrico ergonómico", Price = 85.00m },
            new() { Name = "Teclado Mecánico", Description = "Teclado RGB switches azules", Price = 320.00m },
            new() { Name = "Monitor Samsung", Description = "Monitor 24 pulgadas Full HD", Price = 750.00m },
            new() { Name = "Auriculares Sony", Description = "Bluetooth cancelación de ruido", Price = 450.00m }
        };

        await _context.Clients.AddRangeAsync(clients);
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var orders = new List<Order>
        {
            new() { ClientId = clients[0].ClientId, OrderDate = DateTime.UtcNow },
            new() { ClientId = clients[0].ClientId, OrderDate = DateTime.UtcNow },
            new() { ClientId = clients[1].ClientId, OrderDate = DateTime.UtcNow },
            new() { ClientId = clients[2].ClientId, OrderDate = DateTime.UtcNow },
            new() { ClientId = clients[3].ClientId, OrderDate = DateTime.UtcNow }
        };

        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        return Ok("Datos insertados correctamente ✅");
    }
}