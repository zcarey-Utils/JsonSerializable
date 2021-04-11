using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// The base type of all JsonData that can be written to or read from a JSON file.
	/// </summary>
	public abstract class JsonData : IJsonSerializable {


		/// <exception cref="IOException"></exception>
		abstract internal void Serialize(JsonWriter writer, int depth);

		/// <exception cref="Exception"></exception>
		abstract internal void Parse(JsonReader reader);

		/// <inheritdoc/>
		public JsonData SaveToJson() {
			return this;
		}

		/// <inheritdoc/>
		public abstract void LoadFromJson(JsonData Data);

		/// <exception cref="IOException"></exception>
		/// <exception cref="FormatException"></exception>
		/// <exception cref="Exception"></exception>
		internal static JsonData ParseValue(JsonReader reader) {
			Json.ReadWhitespace(reader);
			int peek = reader.Peek();
			if (peek == -1) throw new IndexOutOfRangeException("End of file was reached before JsonData could be parsed.");
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
				data.Parse(reader);
				return data;
			}
		}

		/// <exception cref="IOException"></exception>
		/// /// <exception cref="FormatException"></exception>
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
				throw new FormatException("Unable to parse number from JSON.");
			}
		}

	}
}
