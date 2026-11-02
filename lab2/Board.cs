namespace CatAndMouse
{
    public class Board
    {
        public int Size { get; }

        public Board(int size)
        {
            Size = size;
        }

        // Твоя точная формула циклического сдвига
        public int Wrap(int position)
        {
            return ((position - 1) % Size + Size) % Size + 1;
        }

        // Проверка правильности начальной клетки (1 <= pos <= Size)
        public bool IsValidSpawn(int position)
        {
            return position >= 1 && position <= Size;
        }
    }
}