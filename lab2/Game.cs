using System;
using System.IO;

namespace CatAndMouse
{
    public class Game
    {
        public static string InputFile = "";
        public static string OutFile = "";

        public Board Board { get; }
        public Player Cat { get; }
        public Player Mouse { get; }
        public GameState CurrentState { get; private set; }

        private StreamWriter? writer;

        public Game(int boardSize)
        {
            Board = new Board(boardSize);
            Cat = new Player("Cat");
            Mouse = new Player("Mouse");
            CurrentState = GameState.Start;
        }

        public void Run()
        {
            using (var reader = new StreamReader(InputFile))
            using (writer = new StreamWriter(OutFile))
            {
                WriteHeader();
                reader.ReadLine(); // Пропускаем первую строку с размером поля

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
            char command = parts[0][0];

            if (command == 'P')
            {
                PrintState();
                return;
            }

            int steps = int.Parse(parts[1]);
            if (command == 'M') Mouse.Move(steps, Board);
            else if (command == 'C') Cat.Move(steps, Board);

            CheckCatch();
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

        private int GetDistance() => Math.Abs(Cat.Location - Mouse.Location);

        private void WriteHeader()
        {
            writer!.WriteLine("Cat and Mouse");
            writer.WriteLine();
            writer.WriteLine("Cat Mouse  Distance");
            writer.WriteLine("-------------------");
        }

        private void PrintState()
        {
            string catStr = Cat.State == State.NotInGame ? "??" : Cat.Location.ToString();
            string mouseStr = Mouse.State == State.NotInGame ? "??" : Mouse.Location.ToString();

            if (Cat.State != State.NotInGame && Mouse.State != State.NotInGame)
                writer!.WriteLine($"{catStr,3} {mouseStr,5} {GetDistance(),9}");
            else
                writer!.WriteLine($"{catStr,3} {mouseStr,5}");
        }

        private void WriteFooter()
        {
            writer!.WriteLine("-------------------");
            writer.WriteLine();
            writer.WriteLine();

            writer.WriteLine("Distance traveled:   Mouse    Cat");
            writer.WriteLine($"                       {Mouse.DistanceTraveled,3}     {Cat.DistanceTraveled,3}");
            writer.WriteLine();

            if (Mouse.State == State.Loser)
                writer.WriteLine($"Mouse caught at: {Mouse.Location,2}");
            else
                writer.WriteLine("Mouse evaded Cat");
        }
    }
}