namespace CatAndMouse
{
    public class Board
    {
        public int Size { get; }

        public Board(int size)
        {
            Size = size;
        }
        public int Wrap(int position)
        {
            return ((position - 1) % Size + Size) % Size + 1;
        }
    }
}