namespace Domain.ports.services;

public interface IExcelService
{
    Task<byte[]> GenerateClientReportAsync();
    Task<byte[]> GenerateProductReportAsync();
}