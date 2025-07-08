using AutoMapper;
using Notes.Application.Notes.Queries.GetNote;
using Notes.Persistance;
using Notes.Tests.Common;
using Shouldly;
using Xunit;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]
public class GetNoteQueryHandlerTests
{
    private readonly NotesDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteQueryHandlerTests(QueryTestsFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetNoteQueryHandler_Success()
    {
        //Arrange
        var handler = new GetNoteQueryHandler(_context, _mapper);
        
        //Act
        var result = await handler.Handle(
            new GetNoteQuery
            {
                UserId = NotesContextFactory.UserBId,
                Id = Guid.Parse("AB429DB1-3BAE-47A6-BDD7-6E3DAC7B0662")
            }, CancellationToken.None);

        //Assert
        result.ShouldBeOfType<NoteVm>();
        result.Title.ShouldBe("Title2");
        result.CreationDate.ShouldBe(DateTime.Today);
    }
}