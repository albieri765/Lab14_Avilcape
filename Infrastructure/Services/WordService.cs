using Domain.ports.services;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

namespace Infrastructure.Services;

public class WordService : IWordService
{
    private readonly AppDbContext _context;

    public WordService(AppDbContext context)
    {
        _context = context;
    }

    // ✅ REPORTE 1 — Clientes con total de órdenes
    public async Task<byte[]> GenerateClientReportAsync()
    {
        var clients = await _context.Clients
            .Include(c => c.Orders)
            .ToListAsync();

        var document = new Document();
        var section  = document.AddSection();

        // Título
        var title = section.AddParagraph();
        title.AppendText("Reporte de Clientes");
        title.Format.HorizontalAlignment = HorizontalAlignment.Center;
        title.ApplyStyle(BuiltinStyle.Heading1);

        // Tabla
        var table = section.AddTable(true);
        table.ResetCells(clients.Count + 1, 4);

        // Encabezados
        table.Rows[0].Cells[0].AddParagraph().AppendText("ID");
        table.Rows[0].Cells[1].AddParagraph().AppendText("Nombre");
        table.Rows[0].Cells[2].AddParagraph().AppendText("Email");
        table.Rows[0].Cells[3].AddParagraph().AppendText("Total Órdenes");

        // Estilo encabezado
        for (int col = 0; col < 4; col++)
        {
            var cell = table.Rows[0].Cells[col];
            cell.CellFormat.BackColor = System.Drawing.Color.LightBlue;
            foreach (Paragraph p in cell.Paragraphs)
            {
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                foreach (DocumentObject obj in p.ChildObjects)
                    if (obj is TextRange tr)
                        tr.CharacterFormat.Bold = true;
            }
        }

        // Datos
        int row = 1;
        foreach (var client in clients)
        {
            table.Rows[row].Cells[0].AddParagraph().AppendText(client.ClientId.ToString());
            table.Rows[row].Cells[1].AddParagraph().AppendText(client.Name);
            table.Rows[row].Cells[2].AddParagraph().AppendText(client.Email);
            table.Rows[row].Cells[3].AddParagraph().AppendText(client.Orders.Count.ToString());
            row++;
        }

        using var stream = new MemoryStream();
        document.SaveToStream(stream, FileFormat.Docx);
        return stream.ToArray();
    }

    // ✅ REPORTE 2 — Productos con precios
    public async Task<byte[]> GenerateProductReportAsync()
    {
        var products = await _context.Products.ToListAsync();

        var document = new Document();
        var section  = document.AddSection();

        // Título
        var title = section.AddParagraph();
        title.AppendText("Reporte de Productos");
        title.Format.HorizontalAlignment = HorizontalAlignment.Center;
        title.ApplyStyle(BuiltinStyle.Heading1);

        // Tabla
        var table = section.AddTable(true);
        table.ResetCells(products.Count + 1, 4);

        // Encabezados
        table.Rows[0].Cells[0].AddParagraph().AppendText("ID");
        table.Rows[0].Cells[1].AddParagraph().AppendText("Nombre");
        table.Rows[0].Cells[2].AddParagraph().AppendText("Descripción");
        table.Rows[0].Cells[3].AddParagraph().AppendText("Precio");

        // Estilo encabezado
        for (int col = 0; col < 4; col++)
        {
            var cell = table.Rows[0].Cells[col];
            cell.CellFormat.BackColor = System.Drawing.Color.LightGreen;
            foreach (Paragraph p in cell.Paragraphs)
            {
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                foreach (DocumentObject obj in p.ChildObjects)
                    if (obj is TextRange tr)
                        tr.CharacterFormat.Bold = true;
            }
        }

        // Datos
        int row = 1;
        foreach (var product in products)
        {
            table.Rows[row].Cells[0].AddParagraph().AppendText(product.ProductId.ToString());
            table.Rows[row].Cells[1].AddParagraph().AppendText(product.Name);
            table.Rows[row].Cells[2].AddParagraph().AppendText(product.Description ?? "");
            table.Rows[row].Cells[3].AddParagraph().AppendText($"${product.Price:F2}");
            row++;
        }

        using var stream = new MemoryStream();
        document.SaveToStream(stream, FileFormat.Docx);
        return stream.ToArray();
    }
}