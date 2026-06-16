using ClosedXML.Excel;
using Domain.ports.services;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ExcelService : IExcelService
{
    private readonly AppDbContext _context;

    public ExcelService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<byte[]> GenerateClientReportAsync()
    {
        var clients = await _context.Clients
            .Include(c => c.Orders)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet      = workbook.Worksheets.Add("Clientes");

        // Encabezados
        var header = worksheet.Row(1);
        header.Style.Font.Bold            = true;
        header.Style.Fill.BackgroundColor = XLColor.LightBlue;
        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "Total Órdenes";

        // Datos
        int row = 2;
        foreach (var client in clients)
        {
            worksheet.Cell(row, 1).Value = client.ClientId;
            worksheet.Cell(row, 2).Value = client.Name;
            worksheet.Cell(row, 3).Value = client.Email;
            worksheet.Cell(row, 4).Value = client.Orders.Count;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    // ✅ REPORTE 2 — Productos con precios
    public async Task<byte[]> GenerateProductReportAsync()
    {
        var products = await _context.Products.ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet      = workbook.Worksheets.Add("Productos");

        // Encabezados
        var header = worksheet.Row(1);
        header.Style.Font.Bold            = true;
        header.Style.Fill.BackgroundColor = XLColor.LightGreen;
        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Descripción";
        worksheet.Cell(1, 4).Value = "Precio";

        // Datos
        int row = 2;
        foreach (var product in products)
        {
            worksheet.Cell(row, 1).Value = product.ProductId;
            worksheet.Cell(row, 2).Value = product.Name;
            worksheet.Cell(row, 3).Value = product.Description;
            worksheet.Cell(row, 4).Value = (double)product.Price;
            worksheet.Cell(row, 4).Style.NumberFormat.Format = "$#,##0.00";
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}