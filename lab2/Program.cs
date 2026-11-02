using System;
using System.IO;

namespace CatAndMouse
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "1.ChaseData.txt";

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Файл {inputFile} не найден!");
                return;
            }

            int boardSize = 0;
            string[] lines = File.ReadAllLines(inputFile);
            if (lines.Length > 0)
            {
                int.TryParse(lines[0].Trim(), out boardSize);
            }

            Game.InputFile = inputFile;
            Game game = new Game(boardSize);
            game.Run();

            Console.WriteLine($"Готово! Результаты сохранены в файл: {Game.OutFile}");
        }
    }
}