using System;

namespace CatAndMouse
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.InputFile = "1.ChaseData.txt";
            Game.OutFile = "1.PursuitLog.txt";

            int boardSize = ReadBoardSize(Game.InputFile);

            if (boardSize <= 0)
            {
                Console.WriteLine("Ошибка при чтении размера поля!");
                return;
            }

            Game game = new Game(boardSize);
            game.Run();

            Console.WriteLine($"Игра успешно завершена. Результаты сохранены в {Game.OutFile}");
        }

        private static int ReadBoardSize(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return 0;

            using (var reader = new System.IO.StreamReader(filePath))
            {
                string? line = reader.ReadLine();
                if (line != null && int.TryParse(line.Trim(), out int size))
                {
                    return size;
                }
            }
            return 0;
        }
    }
}