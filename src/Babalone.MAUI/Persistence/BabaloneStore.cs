using static EVAL.Babalone.Persistence.BabaloneMauiCommon;

namespace EVAL.Babalone.Persistence;

public class BabaloneStore : IStore
{
    public async Task<IEnumerable<string>> GetFilesAsync()
    {
        return await Task.Run(() => Directory.GetFiles(FileSystem.AppDataDirectory)
            .Select(Path.GetFileName)
            .OfType<string>() // a GetFileName [return: NotNullIfNotNull(nameof(path))] annotation cucca miatt erre nem is lenne szükség de buta a compiler
            .Where(f => f.EndsWith(".baba")));
    }

    public async Task<DateTime> GetModifiedTimeAsync(string name)
    {
        FileInfo info = new(Pathify(name));

        return await Task.Run(() => info.LastWriteTime);
    }
}
