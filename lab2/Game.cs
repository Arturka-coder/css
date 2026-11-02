using System;
using System.IO;

namespace CatAndMouse
{
    public class Game
    {
        public static string InputFile = "1.ChaseData.txt";
        public static string OutFile = "1.PursuitLog.txt";

        public Board? Board { get; private set; }
        public Player Cat { get; }
        public Player Mouse { get; }
        public GameState CurrentState { get; private set; }

        private StreamWriter? writer;

        public Game(int boardSize)
        {
            if (boardSize > 0)
            {
                Board = new Board(boardSize);
            }

            Cat = new Player("Cat");
            Mouse = new Player("Mouse");
            CurrentState = GameState.Start;
        }

        public void Run()
        {
            using (writer = new StreamWriter(OutFile))
            {
                WriteHeader();

                // 1. Проверка на размер поля <= 0
                if (Board == null)
                {
                    WriteFooter(isBoardError: true);
                    return;
                }

                if (!File.Exists(InputFile)) return;

                string[] lines = File.ReadAllLines(InputFile);

                try
                {
                    // Пропускаем первую строку (в ней записан размер поля)
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (CurrentState == GameState.End) break;

                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line)) continue;

                        ProcessCommand(line);
                    }

                    WriteFooter();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    // 2. Обработка выхода кота/мыши за пределы поля
                    WriteFooter(playerOutOfRangeMessage: ex.Message);
                }
            }
        }

        private void ProcessCommand(string line)
        {
            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            char command = parts[0][0];

            if (command == 'P')
            {
                PrintState();
                return;
            }

            if ((command == 'C' || command == 'M') && parts.Length > 1 && int.TryParse(parts[1], out int steps))
            {
                if (command == 'C')
                    Cat.Move(steps, Board!);
                else
                    Mouse.Move(steps, Board!);

                CheckCatch();
            }
        }

        private void CheckCatch()
        {
            if (Cat.State == State.Playing &&
                Mouse.State == State.Playing &&
                Cat.Location == Mouse.Location)
            {
                Mouse.Lose();
                Cat.Win();
                CurrentState = GameState.End;
            }
        }

        private int GetDistance()
        {
            return Math.Abs(Cat.Location - Mouse.Location);
        }

        private void WriteHeader()
        {
            writer!.WriteLine("Cat and Mouse");
            writer.WriteLine();

            if (Board == null)
                writer.WriteLine("(нет поля)");
            else
                writer.WriteLine("Cat  Mouse  Distance");

            writer.WriteLine("-------------------");
        }

        private void PrintState()
        {
            if (Board == null)
            {
                writer!.WriteLine("(нет поля)");
                return;
            }

            string catStr = Cat.State == State.NotInGame ? "??" : Cat.Location.ToString();
            string mouseStr = Mouse.State == State.NotInGame ? "??" : Mouse.Location.ToString();

            if (Cat.State == State.Playing && Mouse.State == State.Playing)
            {
                writer!.WriteLine($"{catStr,3} {mouseStr,5} {GetDistance(),9}");
            }
            else
            {
                writer!.WriteLine($"{catStr,3} {mouseStr,5}");
            }
        }

        private void WriteFooter(bool isBoardError = false, string? playerOutOfRangeMessage = null)
        {
            writer!.WriteLine("-------------------");
            writer.WriteLine();
            writer.WriteLine();

            if (isBoardError)
            {
                writer.WriteLine("Поле не создано — игра не состоялась.");
                return;
            }

            if (!string.IsNullOrEmpty(playerOutOfRangeMessage))
            {
                writer.WriteLine($"{playerOutOfRangeMessage}. Игра не состоялась.");
                return;
            }

            writer.WriteLine("Distance traveled:   Mouse    Cat");
            writer.WriteLine($"                       {Mouse.DistanceTraveled,2}      {Cat.DistanceTraveled,2}");
            writer.WriteLine();

            if (Mouse.State == State.Loser)
                writer.WriteLine($"Mouse caught at: {Mouse.Location,2}");
            else
                writer.WriteLine("Mouse evaded Cat");
        }
    }
}