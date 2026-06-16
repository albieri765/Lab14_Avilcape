
using Domain.ports.services;
using MediatR;

namespace Application.UseCases.Excel.Queries;

public class GetProductReportQuery : IRequest<byte[]> { }

public class GetProductReportQueryHandler : IRequestHandler<GetProductReportQuery, byte[]>
{
    private readonly IExcelService _excelService;

    public GetProductReportQueryHandler(IExcelService excelService)
    {
        _excelService = excelService;
    }

    public async Task<byte[]> Handle(GetProductReportQuery request, CancellationToken cancellationToken)
        => await _excelService.GenerateProductReportAsync();
}