using FluentValidation;

namespace Notes.Application.Notes.Commands.CreateNote;

public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(c => 
            c.Title).NotEmpty().MaximumLength(250)
            .WithMessage("Title length must be between 1 and 250");
        RuleFor(c =>
            c.UserId).NotEqual(Guid.Empty)
            .WithMessage("UserId cannot be empty");
    }
}