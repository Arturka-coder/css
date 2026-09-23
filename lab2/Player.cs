using System;

namespace CatAndMouse
{
    // Игрок — это и кот, и мышь.
    // Различаются только именем: new Player("Cat") / new Player("Mouse")
    public class Player
    {
        public string Name { get; }
        public int Location { get; private set; }
        public State State { get; private set; }
        public int DistanceTraveled { get; private set; }

        public Player(string name)
        {
            Name = name;
            Location = -1;              // ещё не в игре
            State = State.NotInGame;
            DistanceTraveled = 0;
        }

        // Ход на steps клеток. Если игрок ещё не в игре —
        // это установка стартовой позиции (не считается ходом).
        public void Move(int steps, Board board)
        {
            if (State == State.NotInGame)
            {
                Location = steps;
                State = State.Playing;
                return;
            }

            DistanceTraveled += Math.Abs(steps);
            Location = board.Wrap(Location + steps);
        }

        public void Win()
        {
            State = State.Winner;
        }

        public void Lose()
        {
            State = State.Loser;
        }
    }
}