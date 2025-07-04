using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNote;

public class GetNoteQueryHandler : IRequestHandler<GetNoteQuery, NoteVm>
{
    private readonly INotesDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteQueryHandler(INotesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NoteVm> Handle(GetNoteQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Notes
            .FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);

        if (entity == null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Note), request.Id);
        }
        
        return _mapper.Map<NoteVm>(entity);
    }
}