using MediatR;
using Domain.ports.services;

namespace Application.UseCases.Word.Queries;

public class GetProductWordReportQuery : IRequest<byte[]> { }

public class GetProductWordReportQueryHandler : IRequestHandler<GetProductWordReportQuery, byte[]>
{
    private readonly IWordService _wordService;

    public GetProductWordReportQueryHandler(IWordService wordService)
    {
        _wordService = wordService;
    }

    public async Task<byte[]> Handle(GetProductWordReportQuery request, CancellationToken ct)
        => await _wordService.GenerateProductReportAsync();
}