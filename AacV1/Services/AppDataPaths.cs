namespace AacV1.Services;

public static class AppDataPaths
{
    public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AacV1");
    public static string Settings => Path.Combine(Root, "settings.json");
    public static string Phrases => Path.Combine(Root, "phrases.json");
    public static string History => Path.Combine(Root, "history.json");
    public static string Favorites => Path.Combine(Root, "favorites.json");
    public static string Dictionary => Path.Combine(Root, "dictionary.json");
    public static string Env => Path.Combine(Root, "env.json");
    public static string Log => Path.Combine(Root, "logs", "app.log");
}
