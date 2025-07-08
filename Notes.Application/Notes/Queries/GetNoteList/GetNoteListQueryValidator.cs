using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNoteList;

public class GetNoteListQueryValidator : AbstractValidator<GetNoteListQuery>
{
    public GetNoteListQueryValidator()
    {
        RuleFor(g =>
            g.UserId).NotEqual(Guid.Empty)
            .WithMessage("UserId cannot be empty");
    }
}