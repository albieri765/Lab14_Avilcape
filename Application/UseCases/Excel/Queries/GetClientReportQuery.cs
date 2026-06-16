using Domain.ports.services;
using MediatR;

namespace Application.UseCases.Excel.Queries;

public class GetClientReportQuery : IRequest<byte[]> { }

public class GetClientReportQueryHandler : IRequestHandler<GetClientReportQuery, byte[]>
{
    private readonly IExcelService _excelService;

    public GetClientReportQueryHandler(IExcelService excelService)
    {
        _excelService = excelService;
    }

    public async Task<byte[]> Handle(GetClientReportQuery request, CancellationToken cancellationToken)
        => await _excelService.GenerateClientReportAsync();
}