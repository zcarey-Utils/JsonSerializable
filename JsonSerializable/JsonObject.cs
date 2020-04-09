using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public class JsonObject : JsonData {

		private Dictionary<string, JsonData> items;
		public IEnumerable<KeyValuePair<string, JsonData>> Items { get => items.AsEnumerable(); }


		public JsonObject() {
			items = new Dictionary<string, JsonData>();
		}

		public JsonObject(Dictionary<string, JsonData> items) {
			this.items = items;
		}

		public void Add(string key, JsonData value) {
			items[key] = value;
		}

		public JsonData this[string key] {
			get {
				if (items.ContainsKey(key)) return items[key];
				else return null;
			}
			set => items[key] = value;
		}

		internal override void Serialize(JsonWriter writer, int depth) {
			if (items.Count > 0) {
				writer.Write('{');
				depth++;
				bool isFirst = true;
				foreach (KeyValuePair<string, JsonData> pair in items) {
					if (!isFirst) writer.Write(',');
					else isFirst = false;
					writer.WriteLine();
					writer.Write('\t', depth);
					JsonString.Serialize(writer, pair.Key);
					writer.Write(": ");
					pair.Value.Serialize(writer, depth);
				}
				depth--;
				writer.WriteLine();
				writer.Write('\t', depth);
				writer.Write('}');
			} else {
				writer.Write("{ }");
			}
		}

		internal override bool Parse(JsonReader reader) {
			Json.ReadWhitespace(reader);
			if (reader.Read() != '{') return false;
			Json.ReadWhitespace(reader);

			while ((reader.Peek() != '}') && (reader.Peek() != -1)) {
				//Read key
				JsonString parsedKey = new JsonString();
				if (!parsedKey.Parse(reader)) return false;
				string key = parsedKey.Value;
				if (key == null) return false;
				Json.ReadWhitespace(reader);
				if (reader.Read() != ':') return false;

				//Determine value;
				JsonData value = JsonData.ParseValue(reader);
				if (value == null) return false;
				else {
					if (this.items.ContainsKey(key)) return false;
					this.items.Add(key, value);
				}
				Json.ReadWhitespace(reader);
				if (reader.Peek() == ',') {
					reader.Read(); //We are going to look, we can ignore the comma.
					Json.ReadWhitespace(reader);
					if (reader.Peek() == '}' || reader.Peek() == -1) return false;
				}
				Json.ReadWhitespace(reader); //Prepare for the next loop
			}
			Json.ReadWhitespace(reader);
			if (reader.Read() != '}') return false;
			return true;
		}

		public override bool LoadFromJson(JsonData Data) {
			if (Data is JsonObject) {
				items.Clear();
				foreach (KeyValuePair<string, JsonData> pair in ((JsonObject)Data).items) {
					items.Add(pair.Key, pair.Value);
				}
				return true;
			} else {
				return false;
			}
		}
	}
}
