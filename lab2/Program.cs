using System;
using System.IO;

namespace CatAndMouse
{
    class Program
    {
        static void Main(string[] args)
        {
            // Работаем с файлами в папке запущенного приложения
            string fileName = "1.ChaseData.txt";
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string inputPath = File.Exists(fileName) ? fileName : (File.Exists(fullPath) ? fullPath : fileName);

            Game.InputFile = inputPath;
            Game.OutFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "1.PursuitLog.txt");

            // Меню выбора режима игры
            Console.WriteLine("=== Конструктор игры Cat and Mouse ===");
            Console.WriteLine("1 - Кот и Мышь (обычная игра)");
            Console.WriteLine("2 - Только Кот");
            Console.WriteLine("3 - Только Мышь");
            Console.WriteLine("4 - Без игроков");
            Console.WriteLine("5 - Без поля (boardSize = 0)");
            Console.Write("Выберите вариант [1-5]: ");

            string? choice = Console.ReadLine()?.Trim();

            bool withCat = true, withMouse = true, withBoard = true;
            int boardSize = ReadBoardSize(Game.InputFile);

            switch (choice)
            {
                case "1":
                    withCat = true; withMouse = true; withBoard = true;
                    break;
                case "2":
                    withCat = true; withMouse = false; withBoard = true;
                    break;
                case "3":
                    withCat = false; withMouse = true; withBoard = true;
                    break;
                case "4":
                    withCat = false; withMouse = false; withBoard = true;
                    break;
                case "5":
                    withCat = false; withMouse = false; withBoard = false;
                    boardSize = 0;
                    break;
                default:
                    Console.WriteLine("Неверный ввод, запускается стандартный режим (1).");
                    break;
            }

            if (withBoard && boardSize <= 0)
            {
                Console.WriteLine($"Ошибка: Файл {Game.InputFile} не найден или размер поля равен 0.");
                return;
            }

            // Создаем экземпляр игры с выбранными флагами
            Game game = new Game(boardSize, withCat, withMouse, withBoard);
            game.Run();

            Console.WriteLine();
            Console.WriteLine($"Готово! Результаты сохранены в файл: {Game.OutFile}");
        }

        private static int ReadBoardSize(string filePath)
        {
            if (!File.Exists(filePath)) return 0;

            using (var reader = new StreamReader(filePath))
            {
                string? line = reader.ReadLine();
                if (line != null && int.TryParse(line.Trim(), out int size))
                    return size;
            }
            return 0;
        }
    }
}