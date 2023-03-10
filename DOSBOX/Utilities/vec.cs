using Newtonsoft.Json;

namespace DOSBOX.Utilities
{
    public class vec
    {
        public int x { get; set; }
        public int y { get; set; }
        public vec() { x = y = 0; }
        public vec(vec v) { x = v.x; y = v.y; }
        public vec(int x, int y) { this.x = x; this.y = y; }
        [JsonIgnore]
        public vecf f => new vecf(x, y);

        public static bool operator ==(vec a, vec b) => a?.x == b?.x && a?.y == b?.y;
        public static bool operator !=(vec a, vec b) => a?.x != b?.x || a?.y != b?.y;
        public static vec operator +(vec a, vec b) => new vec(a.x + b.x, a.y + b.y);
        public static vec operator -(vec a, vec b) => new vec(a.x - b.x, a.y - b.y);
        public static vec operator *(vec a, float b) => new vec((int)(a.x * b), (int)(a.y * b));
    }
}
