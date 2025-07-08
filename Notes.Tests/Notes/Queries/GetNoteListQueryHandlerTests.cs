using AutoMapper;
using Notes.Application.Notes.Queries.GetNote;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Persistance;
using Notes.Tests.Common;
using Shouldly;
using Xunit;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]
public class GetNoteListQueryHandlerTests
{
    private readonly NotesDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteListQueryHandlerTests(QueryTestsFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }
    
    [Fact]
    public async Task GetNoteListQueryHandler_Success()
    {
        //Arrange
        var handler = new GetNoteListQueryHandler(_context, _mapper);
        
        //Act
        var result = await handler.Handle(
            new GetNoteListQuery
            {
                UserId = NotesContextFactory.UserBId
            }, CancellationToken.None);
        
        //Assert
        result.ShouldBeOfType<NoteListVm>();
        result.Notes.Count.ShouldBe(2);
    }
}