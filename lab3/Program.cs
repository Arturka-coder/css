using System.Text;
using laba_3.Models;
using laba_3.Services;

namespace laba_3;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            // Определяем корень проекта
            string baseDir = AppContext.BaseDirectory;
            string projectRoot = baseDir;
            if (baseDir.Contains("bin"))
            {
                var dir = new DirectoryInfo(baseDir);
                for (int i = 0; i < 3 && dir.Parent != null; i++)
                {
                    dir = dir.Parent;
                }
                projectRoot = dir.FullName;
            }

            string inputPath = Path.Combine(projectRoot, "input.txt");
            if (!File.Exists(inputPath)) inputPath = "input.txt";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Файл 'input.txt' не найден в корне проекта.");
                return;
            }

            // Загрузка и парсинг текста
            var text = TextParser.ParseFile(inputPath);
            Console.WriteLine($"Загружено предложений: {text.Sentences.Count}\n");

            // 1. Предложения по возрастанию количества слов
            Console.WriteLine("1. Предложения по возрастанию количества слов:");
            foreach (var s in text.OrderByWordCount())
            {
                Console.WriteLine($"   {s.WordCount} слов: {s}");
            }

            // 2. Предложения по возрастанию длины (символов)
            Console.WriteLine("\n2. Предложения по возрастанию длины (символов):");
            foreach (var s in text.OrderByLength())
            {
                Console.WriteLine($"   {s.FullLength} симв.: {s}");
            }

            // 3. Слова заданной длины в вопросительных предложениях
            int targetLength = 4;
            Console.WriteLine($"\n3. Слова длиной {targetLength} в вопросительных предложениях:");
            var questionWords = text.FindWordsInQuestions(targetLength);
            Console.WriteLine(questionWords.Count == 0 
                ? "   Слова не найдены." 
                : $"   {string.Join(", ", questionWords.Select(w => w.Value))}");

            // 4. Удаление слов заданной длины, начинающихся с согласной
            Console.WriteLine($"\n4. Удаление слов длиной {targetLength}, начинающихся с согласной:");
            int removedConsonants = text.RemoveWordsStartingWithConsonant(targetLength);
            Console.WriteLine($"   Удалено слов: {removedConsonants}");
            Console.WriteLine($"   Результат: {text}");

            // 5. Замена слов длины 3 в первом предложении
            Console.WriteLine("\n5. Замена слов длины 3 в первом предложении на '[REPLACED]':");
            if (text.Sentences.Count > 0)
            {
                int replacedCount = text.ReplaceWordsInSentence(1, 3, "[REPLACED]");
                Console.WriteLine($"   Заменено слов: {replacedCount}");
                Console.WriteLine($"   Результат: {text.Sentences[0]}");
            }

            // 6. Удаление стоп-слов
            Console.WriteLine("\n6. Удаление стоп-слов:");
            string ruStopPath = Path.Combine(projectRoot, "stopwords_ru.txt");
            string enStopPath = Path.Combine(projectRoot, "stopwords_en.txt");
            var stopWords = StopWordsHandler.LoadStopWords(
                File.Exists(ruStopPath) ? ruStopPath : "stopwords_ru.txt", 
                File.Exists(enStopPath) ? enStopPath : "stopwords_en.txt"
            );
            int removedStopWords = text.RemoveStopWords(stopWords);
            Console.WriteLine($"   Удалено стоп-слов: {removedStopWords}");
            Console.WriteLine($"   Результат: {text}");

            // 7. Экспорт в XML
            string xmlPath = Path.Combine(projectRoot, "output.xml");
            text.ExportToXml(xmlPath);
            
            Console.WriteLine($"\n7. Экспорт в XML выполнен успешно!");
            Console.WriteLine($"   Файл записан по пути: {xmlPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}