using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchwarp
{
    public enum OneWay
    {
        Bidirectional = 0,
        In = 1,
        Out = 2
    }

    public readonly struct DoorTarget
    {
        public readonly string door;
        public readonly string room;

        [Newtonsoft.Json.JsonConstructor]
        public DoorTarget(string door, string room)
        {
            this.door = door;
            this.room = room;
        }

        public static DoorTarget None = new DoorTarget();

        public override string ToString()
        {
            if (IsInvalid()) return "None";

            return $"{room}[{door}]";
        }

        public bool IsInvalid() => door == null || room == null;
    }

    public class Door
    {
        public readonly DoorTarget self;
        public readonly DoorTarget target;
        public readonly string area;
        public readonly OneWay oneWay;

        [Newtonsoft.Json.JsonConstructor]
        public Door(DoorTarget self, string area, OneWay oneWay, DoorTarget target)
        {
            this.self = self;
            this.area = area;
            this.oneWay = oneWay;
            this.target = target;
        }

        public override string ToString()
        {
            return self.ToString();
        }
    }


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
            IndexedDoors = Doors.GroupBy(d => d.self.room).ToDictionary(g => g.Key, g => g.ToDictionary(d => d.self.door, d => d));

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
                "White Palace"
            };
            RoomsByArea = Doors.GroupBy(d => d.area).ToDictionary(g => g.Key, g => g.Select(d => d.self.room).Distinct().ToArray());
            DoorsByRoom = Doors.GroupBy(d => d.self.room).ToDictionary(g => g.Key, g => g.Select(d => d.self.door).ToArray());
        }
    }
}
