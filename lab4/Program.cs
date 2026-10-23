using System.Text;
using lab4.Models;

namespace lab4;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            // Путь к файлу input.txt прямо в папке проекта lab4
            var inputPath = "input.txt";
            
            if (!File.Exists(inputPath))
            {
                PrintError($"Файл '{inputPath}' не найден в корне проекта. Положите input.txt в папку lab4.");
                return;
            }

            var text = Text.ParseFile(inputPath);
            PrintConcordance(text.BuildConcordance());
        }
        catch (FileNotFoundException exception)
        {
            PrintError($"Файл не найден: {exception.FileName ?? exception.Message}");
        }
        catch (UnauthorizedAccessException)
        {
            PrintError("Нет доступа к указанному файлу.");
        }
        catch (IOException exception)
        {
            PrintError($"Ошибка при работе с файлом: {exception.Message}");
        }
        catch (Exception exception)
        {
            PrintError(exception.Message);
        }
    }

    private static void PrintConcordance(
        Dictionary<string, (int Count, SortedSet<int> Lines)> concordance)
    {
        if (concordance.Count == 0)
        {
            Console.WriteLine("Текст не содержит слов.");
            return;
        }

        var columnWidth = concordance.Keys.Max(word => word.Length) + 4;

        foreach (var (word, entry) in concordance.OrderBy(
                    pair => pair.Key,
                    StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine(
                $"{word.PadRight(columnWidth, '.')}{entry.Count}: {string.Join(' ', entry.Lines)}");
        }
    }

    private static void PrintError(string message) =>
        Console.Error.WriteLine($"Ошибка: {message}");
}