using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNote;

public class NoteVm : IMapWith<Note>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? EditDate { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Note, NoteVm>()
            .ForMember(noteVm => noteVm.Title,
                opt => opt.MapFrom(note => note.Title))
            .ForMember(noteVm => noteVm.Details,
                opt => opt.MapFrom(note => note.Details))
            .ForMember(noteVm => noteVm.CreationDate,
                opt => opt.MapFrom(s => s.CreationDate))
            .ForMember(noteVm => noteVm.EditDate,
                opt => opt.MapFrom(s => s.EditDate));
    }
}