namespace CatAndMouse
{
    // Игровое поле из N клеток (1..N), ходить можно по кругу
    public class Board
    {
        public int Size { get; }

        public Board(int size)
        {
            Size = size;
        }

        // Пересчёт позиции с закольцовыванием:
        public int Wrap(int position)
        {
            return ((position - 1) % Size + Size) % Size + 1;
        }
    }
}