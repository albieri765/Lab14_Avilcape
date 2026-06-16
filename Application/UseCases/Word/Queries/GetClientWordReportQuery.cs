using MediatR;
using Domain.ports.services;

namespace Application.UseCases.Word.Queries;

public class GetClientWordReportQuery : IRequest<byte[]> { }

public class GetClientWordReportQueryHandler : IRequestHandler<GetClientWordReportQuery, byte[]>
{
    private readonly IWordService _wordService;

    public GetClientWordReportQueryHandler(IWordService wordService)
    {
        _wordService = wordService;
    }

    public async Task<byte[]> Handle(GetClientWordReportQuery request, CancellationToken ct)
        => await _wordService.GenerateClientReportAsync();
}