using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Benchwarp
{
    public enum OneWay
    {
        Bidirectional = 0,
        In = 1,
        Out = 2
    }

    public class Door
    {
        public readonly string room;
        public readonly string door;
        public readonly string area;
        public readonly OneWay oneWay;

        public Door mapsTo
        {
            get; private set;
        }

        public Door(string room, string door, string area, OneWay oneWay)
        {
            this.room = room;
            this.door = door;
            this.area = area;
            this.oneWay = oneWay;
        }

        public void SetMapsTo(Door mapsTo)
        {
            this.mapsTo = mapsTo;
        }

        public static (Door door, string toRoom, string toDoor) FromXml(XmlNode roomNode, XmlNode doorNode)
        {
            Door door = new Door(
                roomNode.Attributes["name"].InnerText, 
                doorNode.Attributes["name"].InnerText, 
                roomNode.Attributes["area"].InnerText,
                (OneWay)Enum.Parse(typeof(OneWay), doorNode?["oneWay"]?.InnerText ?? OneWay.Bidirectional.ToString())
                );

            string toRoom = doorNode?["mapsTo"]?["room"]?.InnerText;
            string toDoor = doorNode?["mapsTo"]?["door"]?.InnerText;

            return (door, toRoom, toDoor);
        }

        public override string ToString()
        {
            return $"{room}[{door}]";
        }

    }


    public static class DoorWarp
    {
        public static Door[] Doors;
        public static List<string> Areas;
        public static Dictionary<string, string[]> RoomsByArea;
        public static Dictionary<string, string[]> DoorsByRoom;

        public static void Load()
        {
            XmlDocument doc = new XmlDocument();
            Stream stream = typeof(Benchwarp).Assembly.GetManifestResourceStream("Benchwarp.Data.doors.xml");
            doc.Load(stream);
            stream.Dispose();

            List<XmlNode> nodes = doc.SelectNodes("doors/room").Cast<XmlNode>().ToList();

            List<(Door door, string toRoom, string toDoor)> preDoors = new List<(Door door, string toRoom, string toDoor)>();

            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode child in node.SelectNodes("door"))
                {
                    preDoors.Add(Door.FromXml(node, child));
                }
            }

            foreach (var t in preDoors)
            {
                if (t.door.oneWay != OneWay.Out)
                {
                    t.door.SetMapsTo(preDoors.FirstOrDefault(u => u.door.room == t.toRoom && u.door.door == t.toDoor).door);
                }
            }

            Doors = preDoors.Select(t => t.door).ToArray();
            // Areas = Doors.Select(d => d.area).Distinct().ToArray();
            Areas = new List<string> {
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
            RoomsByArea = Doors.GroupBy(d => d.area).ToDictionary(g => g.Key, g => g.Select(d => d.room).ToArray());
            DoorsByRoom = Doors.GroupBy(d => d.room).ToDictionary(g => g.Key, g => g.Select(d => d.door).ToArray());
        }




    }
}
