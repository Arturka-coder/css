using System;

namespace CatAndMouse
{
    public class Player
    {
        public string Name { get; }
        public int Location { get; private set; }
        public int DistanceTraveled { get; private set; }
        public State State { get; private set; }

        public Player(string name)
        {
            Name = name;
            Location = 0;
            DistanceTraveled = 0;
            State = State.NotInGame;
        }

        public void Move(int steps, Board board)
        {
            if (State == State.NotInGame)
            {
                // Спавн: если клетка спавна вне границ 1..Size
                if (!board.IsValidSpawn(steps))
                {
                    throw new ArgumentOutOfRangeException(null, $"{Name} находится за пределами поля");
                }

                Location = steps;
                State = State.Playing;
            }
            else
            {
                // Обычный ход по формуле Wrap
                DistanceTraveled += Math.Abs(steps);
                Location = board.Wrap(Location + steps);
            }
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