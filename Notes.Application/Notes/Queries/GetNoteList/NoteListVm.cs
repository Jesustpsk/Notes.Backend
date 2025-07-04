namespace Notes.Application.Notes.Queries.GetNoteList;

public class NoteListVm
{
    public Task<List<NoteLookupDto>> Notes { get; set; }
}