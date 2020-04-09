using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public abstract class JsonData : IJsonSerializable {

		abstract internal void Serialize(JsonWriter writer, int depth);
		abstract internal bool Parse(JsonReader reader);

		public JsonData SaveToJson() {
			return this;
		}

		public abstract bool LoadFromJson(JsonData Data);

		internal static JsonData ParseValue(JsonReader reader) {
			Json.ReadWhitespace(reader);
			int peek = reader.Peek();
			if (peek == -1) return null;
			else {
				JsonData data;
				char c = (char)peek;
				if (c == '{') data = new JsonObject();
				else if (c == '[') data = new JsonArray();
				else if (c == '\"' || c == 'n' || c == 'N') data = new JsonString();
				else if (c == 't' || c == 'T' || c == 'f' || c == 'F') data = new JsonBool();
				else {
					return ParseNumber(reader);
				}
				return data.Parse(reader) ? data : null;
			}
		}

		private static JsonData ParseNumber(JsonReader reader) {
			int peek;
			char c;
			string number = "";
			while ((peek = reader.Peek()) != -1) {
				c = (char)peek;
				if (c == '-' || c == '.' || c == 'e' || c == 'E' || char.IsDigit(c)) {
					number += c;
					reader.Read(); //Pop the character from the stream
				} else {
					break; //Reach the end of the number.
				}
			}

			if (long.TryParse(number, out long lg)) {
				return new JsonInteger(lg);
			} else if (double.TryParse(number, out double d)) {
				return new JsonDecimal(d);
			} else {
				return null;
			}
		}

	}
}
