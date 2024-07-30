using DOSBOX.Suggestions.fusion.jsondata;
using DOSBOX.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Warp
    {
        [JsonPropertyName("room")]
        public int DestinationRoom { get; set; }
        [JsonPropertyName("tiles")]
        public _FromTo Tiles { get; set; }
        public Warp()
        {
        }
        public Warp(RoomData_warps w)
        {
            DestinationRoom = w.room;
            Tiles = new _FromTo(w.tiles);
        }
        public Warp(int destinationRoom, RoomData_fromto[] raw)
        {
            DestinationRoom = destinationRoom;
            Tiles = new _FromTo(raw);
        }
        public Warp(int destinationRoom, _FromTo tiles)
        {
            DestinationRoom = destinationRoom;
            Tiles = tiles;
        }
    }

    public class _FromTo
    {
        [JsonPropertyName("tiles.from")]
        public List<vec> From;
        [JsonPropertyName("tiles.to")]
        public List<vec> To;
        public _FromTo()
        {
        }
        public _FromTo(RoomData_fromto[] raw)
        {
            From = raw.ToList().Select(ft => ft.from.AsVec()).ToList();
            To = raw.ToList().Select(ft => ft.to.AsVec()).ToList();
        }
        public _FromTo(List<vec> from, List<vec> to)
        {
            From = from;
            To = to;
        }
        public _FromTo(vec[] from, vec[] to)
        {
            From = from.ToList();
            To = to.ToList();
        }
    }
}
