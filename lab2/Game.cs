using System;
using System.IO;

namespace CatAndMouse
{
    public class Game
    {
        public static string InputFile = "";
        public static string OutFile = "";

        public Board? Board { get; }
        public Player? Cat { get; }
        public Player? Mouse { get; }
        public bool WithBoard { get; }
        public bool WithCat { get; }
        public bool WithMouse { get; }
        public GameState CurrentState { get; private set; }

        private StreamWriter? writer;

        public Game(int boardSize, bool withCat = true, bool withMouse = true, bool withBoard = true)
        {
            WithBoard = withBoard && boardSize > 0;
            WithCat = withCat && WithBoard;
            WithMouse = withMouse && WithBoard;

            if (WithBoard) Board = new Board(boardSize);
            if (WithCat) Cat = new Player("Cat");
            if (WithMouse) Mouse = new Player("Mouse");

            CurrentState = GameState.Start;
        }

        public void Run()
        {
            using (var reader = new StreamReader(InputFile))
            using (writer = new StreamWriter(OutFile))
            {
                WriteHeader();

                if (!WithBoard)
                {
                    writer.WriteLine("Поле не создано — игра невозможна.");
                    WriteFooter();
                    return;
                }

                string? sizeLine = reader.ReadLine();
                if (sizeLine == null || !int.TryParse(sizeLine.Trim(), out int declaredSize) || declaredSize <= 0)
                {
                    writer.WriteLine("Ошибка: некорректный размер поля в первой строке.");
                    WriteFooter();
                    return;
                }

                while (CurrentState != GameState.End)
                {
                    string? line = reader.ReadLine();
                    if (line == null)
                    {
                        CurrentState = GameState.End;
                        break;
                    }

                    line = line.Trim();
                    if (line.Length == 0) continue;

                    ProcessCommand(line);
                }

                WriteFooter();
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

            if (command != 'M' && command != 'C') return;
            if (parts.Length < 2) return;
            if (!int.TryParse(parts[1], out int steps)) return;

            if (command == 'C')
            {
                if (!WithCat) return;

                if (Cat!.State == State.NotInGame && (steps < 1 || steps > Board!.Size)) return;

                Cat.Move(steps, Board!);
                CheckCatch();
                return;
            }

            if (command == 'M')
            {
                if (!WithMouse) return;

                if (Mouse!.State == State.NotInGame && (steps < 1 || steps > Board!.Size)) return;

                Mouse.Move(steps, Board!);
                CheckCatch();
            }
        }

        private void CheckCatch()
        {
            if (!WithCat || !WithMouse) return;

            if (Cat!.State == State.Playing &&
                Mouse!.State == State.Playing &&
                Cat.Location == Mouse.Location)
            {
                Mouse.Lose();
                Cat.Win();
                CurrentState = GameState.End;
            }
        }

        private int GetDistance()
        {
            if (Cat == null || Mouse == null) return 0;
            return Math.Abs(Cat.Location - Mouse.Location);
        }

        private void WriteHeader()
        {
            writer!.WriteLine("Cat and Mouse");
            writer.WriteLine();

            if (!WithBoard) writer.WriteLine("(нет поля)");
            else if (WithCat && WithMouse) writer.WriteLine("Cat Mouse  Distance");
            else if (WithCat) writer.WriteLine("Cat  Distance");
            else if (WithMouse) writer.WriteLine("Mouse  Distance");
            else writer.WriteLine("(нет игроков)");

            writer.WriteLine("-------------------");
        }

        private void PrintState()
        {
            if (!WithBoard)
            {
                writer!.WriteLine("(нет поля)");
                return;
            }

            if (!WithCat && !WithMouse)
            {
                writer!.WriteLine("(нет игроков)");
                return;
            }

            if (WithCat && WithMouse)
            {
                string catStr = Cat!.State == State.NotInGame ? "??" : Cat.Location.ToString();
                string mouseStr = Mouse!.State == State.NotInGame ? "??" : Mouse.Location.ToString();

                if (Cat.State != State.NotInGame && Mouse.State != State.NotInGame)
                    writer!.WriteLine($"{catStr,3} {mouseStr,5} {GetDistance(),9}");
                else
                    writer!.WriteLine($"{catStr,3} {mouseStr,5}");
            }
            else if (WithCat)
            {
                string catStr = Cat!.State == State.NotInGame ? "??" : Cat.Location.ToString();
                writer!.WriteLine($"{catStr,3}");
            }
            else
            {
                string mouseStr = Mouse!.State == State.NotInGame ? "??" : Mouse.Location.ToString();
                writer!.WriteLine($"{mouseStr,3}");
            }
        }

        private void WriteFooter()
        {
            writer!.WriteLine("-------------------");
            writer.WriteLine();
            writer.WriteLine();

            if (!WithBoard)
            {
                writer.WriteLine("Поле не создано — игра не состоялась.");
                return;
            }

            if (WithCat && WithMouse)
            {
                writer.WriteLine("Distance traveled:   Mouse    Cat");
                writer.WriteLine($"                       {Mouse!.DistanceTraveled,3}     {Cat!.DistanceTraveled,3}");
                writer.WriteLine();

                if (Mouse.State == State.Loser)
                    writer.WriteLine($"Mouse caught at: {Mouse.Location,2}");
                else
                    writer.WriteLine("Mouse evaded Cat");
            }
            else if (WithMouse)
            {
                writer.WriteLine("Distance traveled:   Mouse");
                writer.WriteLine($"                       {Mouse!.DistanceTraveled,3}");
                writer.WriteLine();
                writer.WriteLine("Mouse evaded Cat");
            }
            else if (WithCat)
            {
                writer.WriteLine("Distance traveled:   Cat");
                writer.WriteLine($"                       {Cat!.DistanceTraveled,3}");
                writer.WriteLine();
                writer.WriteLine("Mouse evaded Cat");
            }
            else
            {
                writer.WriteLine("Игроков нет — игра не состоялась.");
            }
        }
    }
}