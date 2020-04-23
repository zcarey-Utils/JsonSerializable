using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public class JsonBool : JsonValue<bool> {
		public JsonBool() : base() { }
		public JsonBool(bool value) : base(value) { }

		public static implicit operator bool(JsonBool data) => data.Value;
		public static explicit operator JsonBool(bool data) => new JsonBool(data);

		public override bool LoadFromJson(JsonData Data) {
			if (Data is JsonBool) {
				this.Value = ((JsonBool)Data).Value;
				return true;
			} else {
				return false;
			}
		}

		internal override bool Parse(JsonReader reader) {
			string str = "";
			for (int i = 0; i < "true".Length; i++) str += (char)reader.Read();
			if (bool.TryParse(str, out bool b)) {
				Value = b;
				return true;
			} else {
				str += (char)reader.Read();
				if (bool.TryParse(str, out b)) {
					Value = b;
					return true;
				} else {
					return false;
				}
			}
		}

		internal override void Serialize(JsonWriter writer, int depth) {
			writer.Write(Value);
		}
	}
}
