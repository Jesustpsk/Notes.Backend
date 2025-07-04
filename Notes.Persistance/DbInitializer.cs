namespace Notes.Persistance;

public class DbInitializer
{
    public static async Task InitializeAsync(NotesDbContext context)
    {
        await context.Database.EnsureCreatedAsync();
    }
}