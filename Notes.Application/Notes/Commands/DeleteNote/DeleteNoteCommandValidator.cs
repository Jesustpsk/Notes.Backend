using FluentValidation;

namespace Notes.Application.Notes.Commands.DeleteNote;

public class DeleteNoteCommandValidator : AbstractValidator<DeleteNoteCommand>
{
    public DeleteNoteCommandValidator()
    {
        RuleFor(d =>
            d.Id).NotEqual(Guid.Empty)
            .WithMessage("Id cannot be empty");
        RuleFor(d =>
            d.UserId).NotEqual(Guid.Empty)
            .WithMessage("UserId cannot be empty");
    }
}