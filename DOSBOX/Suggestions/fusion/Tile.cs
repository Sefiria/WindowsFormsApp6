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

        public Tile() { }
        public Tile(TYPE type, string str_pixels)
        {
            Type = type;

            var values = str_pixels.Split(',');
            int w = int.Parse(values[0]);
            int h = int.Parse(values[1]);
            Pixels = new byte[w, h];
            for (int i = 0, p = 2; i < h; i++)
            {
                for (int j = 0; j < w; j++, p++)
                {
                    Pixels[j, i] = byte.Parse(values[p]);
                }
            }

        }
    }
}
