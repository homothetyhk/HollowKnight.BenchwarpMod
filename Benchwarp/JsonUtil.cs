using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Benchwarp
{
    public static class JsonUtil
    {
        public static readonly JsonSerializer _js;

        public static T Deserialize<T>(string embeddedResourcePath)
        {
            using (StreamReader sr = new StreamReader(typeof(JsonUtil).Assembly.GetManifestResourceStream(embeddedResourcePath)))
            using (var jtr = new JsonTextReader(sr))
            {
                return _js.Deserialize<T>(jtr);
            }
        }

        public static T DeserializeString<T>(string json)
        {
            using (StringReader sr = new StringReader(json))
            using (var jtr = new JsonTextReader(sr))
            {
                return _js.Deserialize<T>(jtr);
            }
        }

        public static void Serialize(object o, string fileName)
        {
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(typeof(JsonUtil).Assembly.Location), fileName), Serialize(o));
        }

        public static string Serialize(object o)
        {
            using (StringWriter sw = new StringWriter())
            {
                _js.Serialize(sw, o);
                sw.Flush();
                return sw.ToString();
            }
        }

        private class Vector2Converter : JsonConverter<Vector2>
        {
            public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0;
                float y = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = (string)reader.Value;
                        switch (propertyName)
                        {
                            case "x":
                                x = (float)reader.ReadAsDouble();
                                break;
                            case "y":
                                y = (float)reader.ReadAsDouble();
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject) break;
                }

                return new Vector2(x, y);
            }

            public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WriteEndObject();
            }
        }

        private class Vector3Converter : JsonConverter<Vector3>
        {
            public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0;
                float y = 0;
                float z = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = (string)reader.Value;
                        switch (propertyName)
                        {
                            case "x":
                                x = (float)reader.ReadAsDouble();
                                break;
                            case "y":
                                y = (float)reader.ReadAsDouble();
                                break;
                            case "z":
                                z = (float)reader.ReadAsDouble();
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject) break;
                }

                return new Vector3(x, y, z);
            }

            public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WritePropertyName("z");
                writer.WriteValue(value.z);
                writer.WriteEndObject();
            }
        }

        static JsonUtil()
        {
            _js = new JsonSerializer
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
            };

            _js.Converters.Add(new StringEnumConverter());
            _js.Converters.Add(new Vector2Converter());
            _js.Converters.Add(new Vector3Converter());
        }
    }
}
