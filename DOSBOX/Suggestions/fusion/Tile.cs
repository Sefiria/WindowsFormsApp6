namespace DOSBOX.Suggestions.fusion
{
    public class Tile
    {
        public static int TSZ => 8;
        public enum TYPE
        {
            EMPTY = 0,
            SOLID,
            FRONT
        }

        public byte[,] Pixels;
        public TYPE Type;

        public byte this[int x, int y] => x < 0 || y < 0 || x > 7 || y > 7 ? (byte)0 : Pixels[x, y];
    }
}
