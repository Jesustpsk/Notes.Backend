using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNote;

public class GetNoteQueryValidator : AbstractValidator<GetNoteQuery>
{
    public GetNoteQueryValidator()
    {
        RuleFor(g =>
            g.Id).NotEqual(Guid.Empty)
            .WithMessage("Id cannot be empty");
        RuleFor(g =>
            g.UserId).NotEqual(Guid.Empty)
            .WithMessage("UserId cannot be empty");
    }
}