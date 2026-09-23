using System;

namespace CatAndMouse
{
    public class Player
    {
        public string Name { get; }
        public int Location { get; private set; }
        public State State { get; private set; }
        public int DistanceTraveled { get; private set; }

        public Player(string name)
        {
            Name = name;
            Location = -1;              // По умолчанию не в игре
            State = State.NotInGame;
            DistanceTraveled = 0;
        }

        // Ход игрока. Первый ход задаёт стартовую позицию и не увеличивает пройденное расстояние.
        public void Move(int steps, Board board)
        {
            if (State == State.NotInGame)
            {
                Location = board.Wrap(steps);
                State = State.Playing;
                return;
            }

            DistanceTraveled += Math.Abs(steps);
            Location = board.Wrap(Location + steps);
        }

        public void Win() => State = State.Winner;
        public void Lose() => State = State.Loser;
    }
}