namespace Domain.ports.services;
public interface IWordService
{
    Task<byte[]> GenerateClientReportAsync();
    Task<byte[]> GenerateProductReportAsync();
}