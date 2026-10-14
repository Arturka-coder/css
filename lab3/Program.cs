using System;
using System.IO;
using System.Xml.Serialization;
using laba_3.Models;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        string outputPath = "output.xml";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Файл {inputPath} не найден в корне проекта!");
            return;
        }

        string rawText = File.ReadAllText(inputPath);
        Text text = new Text(rawText);

        Console.WriteLine("ВЫПОЛНЕНИЕ ТРЕБОВАНИЙ ТЗ (1 - 6)\n");

        // ПУНКТ 1
        Console.WriteLine("1. Предложения в порядке возрастания количества слов:");
        var sortedSentences = text.GetSortedByWordCount();
        foreach (var s in sortedSentences)
        {
            Console.WriteLine($"   (Слов: {s.Words.Count}) {s}");
        }
        Console.WriteLine();

        // ПУНКТ 2
        Console.WriteLine("2. Вывод исходных предложений текста:");
        for (int i = 0; i < text.Sentences.Count; i++)
        {
            Console.WriteLine($"   [{i}] {text.Sentences[i]}");
        }
        Console.WriteLine();

        // ПУНКТ 3
        int targetLength = 4;
        Console.WriteLine($"3. Уникальные слова длиной {targetLength} в вопросительных предложениях:");
        var uniqueWords = text.GetUniqueWordsInQuestions(targetLength);
        Console.WriteLine($"   {string.Join(", ", uniqueWords)}");
        Console.WriteLine();

        // ПУНКТ 4
        int removeLength = 3;
        Console.WriteLine($"4. Удаление слов длиной {removeLength}, начинающихся с согласной:");
        text.RemoveWordsByLengthAndConsonant(removeLength);
        Console.WriteLine("   (Успешно выполнено)");
        Console.WriteLine();

        // ПУНКТ 5
        Console.WriteLine("5. Замена слов длины 5 в предложении [0] на '[ЗАТЕРТО]':");
        text.ReplaceWordsInSentence(0, 5, "[ЗАТЕРТО]");
        Console.WriteLine("   (Успешно выполнено)");
        Console.WriteLine();

        // ПУНКТ 6: Загрузка стоп-слов из файлов stopwords_ru.txt и stopwords_en.txt
        Console.WriteLine("6. Удаление стоп-слов из текста:");
        HashSet<string> stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists("stopwords_ru.txt"))
        {
            foreach (var line in File.ReadAllLines("stopwords_ru.txt"))
            {
                if (!string.IsNullOrWhiteSpace(line))
                    stopWords.Add(line.Trim().ToLower());
            }
        }

        if (File.Exists("stopwords_en.txt"))
        {
            foreach (var line in File.ReadAllLines("stopwords_en.txt"))
            {
                if (!string.IsNullOrWhiteSpace(line))
                    stopWords.Add(line.Trim().ToLower());
            }
        }

        text.RemoveStopWords(stopWords);
        Console.WriteLine($"   (Загружено стоп-слов: {stopWords.Count}. Стоп-слова успешно удалены)");
        Console.WriteLine();

        // ПУНКТ 7: Сериализация в XML
        Console.WriteLine("ПУНКТ 7: ЭКСПОРТ В XML-ДОКУМЕНТ");

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Text));
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                serializer.Serialize(writer, text);
            }
            Console.WriteLine($"[УСПЕХ] Файл успешно сохранен в корень проекта:\n{Path.GetFullPath(outputPath)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Ошибка сериализации: {ex.Message}");
        }
    }
}