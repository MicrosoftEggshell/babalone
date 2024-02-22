namespace EVAL.Babalone.Persistence;

public static class BabaloneMauiCommon
{
    public static string Pathify(object? obj)
    {
        if (obj is string path)
            path = path.Trim();
        else
            path = obj?.ToString()?.Trim() ?? string.Empty;

        return Path.IsPathRooted(path) ? path : Path.Combine(FileSystem.AppDataDirectory, path);
    }
}
