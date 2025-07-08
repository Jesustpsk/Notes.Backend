using FluentValidation;

namespace Notes.Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(u => 
            u.UserId).NotEqual(Guid.Empty)
            .WithMessage("UserId cannot be empty");
        RuleFor(u =>
            u.Id).NotEqual(Guid.Empty)
            .WithMessage("Id cannot be empty");
        RuleFor(u =>
            u.Title).NotEmpty().MaximumLength(250)
            .WithMessage("Title length must be between 1 and 250");
    }
}