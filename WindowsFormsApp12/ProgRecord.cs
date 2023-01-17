using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public class ProgRecord
    {
        public enum KeyTypes
        {
            Up = 0,
            Down
        }

        [JsonPropertyName("t")]
        public long Time { get; set; } = 0;
        [JsonPropertyName("k")]
        public Keys Key { get; set; } = Keys.None;
        public KeyTypes KeyType { get; set; } = KeyTypes.Up;

        public ProgRecord() { }
        public ProgRecord(long time, Keys key, KeyTypes type)
        {
            Time = time;
            Key = key;
            KeyType = type;
        }
    }
}
