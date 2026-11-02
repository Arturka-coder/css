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

        Console.WriteLine("1. Предложения в порядке возрастания количества слов:");
        var sortedSentences = text.GetSortedByWordCount();
        foreach (var s in sortedSentences)
        {
            Console.WriteLine($"   (Слов: {s.Words.Count}) {s}");
        }
        Console.WriteLine();
 
        Console.WriteLine("2. Предложения в порядке возрастания их длины (в символах):");
        var sortedByLength = text.GetSortedByLength();
        foreach (var s in sortedByLength)
        {
            Console.WriteLine($"   (Длина: {s.ToString().Length} симв.) {s}");
        }
        Console.WriteLine();

        int targetLength = 4;
        Console.WriteLine($"3. Уникальные слова длиной {targetLength} в вопросительных предложениях:");
        var uniqueWords = text.GetUniqueWordsInQuestions(targetLength);
        if (uniqueWords.Count > 0)
        {
            Console.WriteLine($"   {string.Join(", ", uniqueWords)}");
        }
        else
        {
            Console.WriteLine("   В тексте не найдено вопросительных предложений или слов такой длины");
        }
        Console.WriteLine();

        int removeLength = 3;
        Console.WriteLine($"4. Удаление слов длиной {removeLength}, начинающихся с согласной:");
        text.RemoveWordsByLengthAndConsonant(removeLength);
        for (int i = 0; i < text.Sentences.Count; i++)
        {
            Console.WriteLine($"   [{i}] {text.Sentences[i]}");
        }
        Console.WriteLine();

        Console.WriteLine("5. Замена слов длины 5 в предложении [0] на '[ЗАТЕРТО]':");
        if (text.Sentences.Count > 0)
        {
            text.ReplaceWordsInSentence(0, 5, "[ЗАТЕРТО]");
            Console.WriteLine($"   {text.Sentences[0]}");
        }
        Console.WriteLine();

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
        for (int i = 0; i < text.Sentences.Count; i++)
        {
            Console.WriteLine($"   [{i}] {text.Sentences[i]}");
        }
        Console.WriteLine();

        // Пункт 7
        Console.WriteLine("7. Экспорт в XML-документ:");
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Text));
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                serializer.Serialize(writer, text);
            }
            Console.WriteLine($"Файл успешно сохранен: {Path.GetFullPath(outputPath)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сериализации: {ex.Message}");
        }
    }
}