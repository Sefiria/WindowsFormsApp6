using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DOSBOX.Suggestions.fusion.jsondata
{
    public class RoomData_objects
    {
        [JsonPropertyName("type")]
        public Enumerations.PhysicalObjectType type { get; set; }
        [JsonPropertyName("side")]
        public Enumerations.PhysicalObjectSide side { get; set; }
        public RoomData_coord vec { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}
