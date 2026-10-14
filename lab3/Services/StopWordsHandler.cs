namespace laba_3.Services;

public static class StopWordsHandler
{
    public static HashSet<string> LoadStopWords(params string[] filePaths)
    {
        var stopSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in filePaths)
        {
            if (File.Exists(path))
            {
                foreach (var line in File.ReadLines(path))
                {
                    var trimmed = line.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        stopSet.Add(trimmed);
                    }
                }
            }
        }

        return stopSet;
    }
}