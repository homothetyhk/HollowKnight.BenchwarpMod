using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Benchwarp
{
    public enum OneWay
    {
        Bidirectional = 0,
        In = 1,
        Out = 2
    }

    public class DoorTargetConverter : JsonConverter<DoorTarget>
    {
        public static readonly Regex r = new(@"([^\[]*)\[([^\]]*)\]");
        public override DoorTarget ReadJson(JsonReader reader, Type objectType, DoorTarget existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken jt = JToken.Load(reader);
            if (jt.Type == JTokenType.String)
            {
                string s = jt.Value<string>();
                if (s == "None") return DoorTarget.None;
                else
                {
                    Match m = r.Match(jt.Value<string>());
                    return new(m.Groups[1].Value, m.Groups[2].Value);
                }
            }
            else if (jt.Type == JTokenType.Object)
            {
                return new(jt.Value<string>(nameof(DoorTarget.room)), jt.Value<string>(nameof(DoorTarget.door)));
            }
            else throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, DoorTarget value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }

    [JsonConverter(typeof(DoorTargetConverter))]
    public readonly struct DoorTarget
    {
        public readonly string room;
        public readonly string door;

        [JsonConstructor]
        public DoorTarget(string room, string door)
        {
            this.room = room;
            this.door = door;
        }

        public static DoorTarget None = new();

        public override string ToString()
        {
            if (IsInvalid()) return "None";

            return $"{room}[{door}]";
        }

        public bool IsInvalid() => door == null || room == null;
    }

    public record Door(DoorTarget Self, DoorTarget Target, string Area, OneWay OneWay);

    public static class DoorWarp
    {
        public static Door[] Doors;
        public static string[] Areas;
        public static Dictionary<string, string[]> RoomsByArea;
        public static Dictionary<string, string[]> DoorsByRoom;
        public static Dictionary<string, Dictionary<string, Door>> IndexedDoors;

        static DoorWarp()
        {
            Doors = JsonUtil.Deserialize<Door[]>("Benchwarp.Resources.doors.json");
            IndexedDoors = Doors.GroupBy(d => d.Self.room).ToDictionary(g => g.Key, g => g.ToDictionary(d => d.Self.door, d => d));

            // Areas = Doors.Select(d => d.area).Distinct().ToArray();
            // Better to order areas manually
            Areas = new string[]
            {
                "Black Egg Temple",
                "King's Pass",
                "Dirtmouth",
                "Misc",
                "Forgotten Crossroads",
                "Ancestral Mound",
                "Greenpath",
                "Lake of Unn",
                "Stone Sanctuary",
                "Fog Canyon",
                "Overgrown Mound",
                "Teacher's Archives",
                "Queen's Station",
                "Fungal Wastes",
                "Mantis Village",
                "Fungal Core",
                "Deepnest",
                "Distant Village",
                "Beast's Den",
                "Failed Tramway",
                "Weaver's Den",
                "Kingdom's Edge",
                "Cast Off Shell",
                "Hive",
                "Colosseum",
                "Ancient Basin",
                "Palace Grounds",
                "Abyss",
                "Royal Waterways",
                "Isma's Grove",
                "Junk Pit",
                "City of Tears",
                "Soul Sanctum",
                "King's Station",
                "Tower of Love",
                "Pleasure House",
                "Resting Grounds",
                "Blue Lake",
                "Spirits' Glade",
                "Crystal Peak",
                "Hallownest's Crown",
                "Crystallized Mound",
                "Queen's Gardens",
                "Howling Cliffs",
                "Stag Nest",
                "White Palace",
                "Godhome",
                "Stag",
                "Tram"
            };
            RoomsByArea = Doors.GroupBy(d => d.Area).ToDictionary(g => g.Key, g => g.Select(d => d.Self.room).Distinct().ToArray());
            DoorsByRoom = Doors.GroupBy(d => d.Self.room).ToDictionary(g => g.Key, g => g.Select(d => d.Self.door).ToArray());
        }
    }
}
