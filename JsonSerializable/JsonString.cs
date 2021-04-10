using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// JsonValue for the native type <see cref="string"/>.
	/// </summary>
	public class JsonString : JsonValue<string> {

		/// <inheritdoc/>
		public JsonString() : base() { }

		/// <inheritdoc/>
		public JsonString(string value) : base(value) { }

		/// <summary>
		/// Converts the JsonValue to a string by returning the contained value.
		/// </summary>
		/// <param name="data"></param>
		public static implicit operator string(JsonString data) => data.Value;

		/// <summary>
		/// Converts the string to a JsonValue by creating a new JsonValue and setting its value to the string.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonString(string data) => new JsonString(data);

		/// <inheritdoc/>
		/// <exception cref="InvalidCastException"></exception>
		public override void LoadFromJson(JsonData Data) {
			this.Value = ((JsonString)Data).Value;
		}

		/// <exception cref="IOException"></exception>
		private bool ParseNull(JsonReader reader) {
			//if(reader.Peek() == 'n' || reader.Peek() == 'N') {
			reader.Read();
			if (reader.Peek() == 'u' || reader.Peek() == 'U') {
				reader.Read();
				if (reader.Peek() == 'l' || reader.Peek() == 'L') {
					reader.Read();
					if (reader.Peek() == 'l' || reader.Peek() == 'L') {
						reader.Read();
						Value = null;
						return true;
					}
				}
			}
			//}
			return false;
		}

		/// <exception cref="IOException"></exception>
		private static bool ParseUnicode(JsonReader reader, out char c) {
			c = ' ';
			string str = "";
			for (int i = 0; i < 4; i++) {
				if (reader.Peek() == -1) return false;
				else str += (char)reader.Read();
			}
			int uni = -1;
			try {
				uni = Convert.ToInt32(str, 16);
				c = (char)uni;
				return true;
			} catch (Exception) {
				return false;
			}
		}

		/// <exception cref="IOException"></exception>
		private static bool ParseEscape(JsonReader reader, out char c) {
			int peek = reader.Read();

			if (peek == -1) {
				c = ' ';
				return false;
			} else if (peek == '\"' || peek == '\\' || peek == '/') c = (char)peek;
			else if (peek == 'b') c = '\b';
			else if (peek == 'f') c = '\f';
			else if (peek == 'n') c = '\n';
			else if (peek == 'r') c = '\r';
			else if (peek == 't') c = '\t';
			else if (peek == 'u') return ParseUnicode(reader, out c);
			else {
				c = ' ';
				return false;
			}

			return true;
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		internal override bool Parse(JsonReader reader) {
			StringBuilder str = new StringBuilder();
			Json.ReadWhitespace(reader);
			if (reader.Peek() == 'n' || reader.Peek() == 'N') return ParseNull(reader);
			if (reader.Read() != '\"') return false;
			int peek;
			char c;
			while ((peek = reader.Read()) != '\"') {
				if (peek == -1) return false;
				c = (char)peek;

				if (c == '\\') {
					if (!ParseEscape(reader, out c)) return false;
				}
				str.Append(c);
			}
			Value = str.ToString();
			return true;
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		internal override void Serialize(JsonWriter writer, int depth) {
			JsonString.Serialize(writer, Value);
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		internal static void Serialize(JsonWriter writer, string str) {
			if (str == null) writer.Write("null");
			else {
				writer.Write('\"');
				foreach (char c in str) {
					if ((c == '\"') || (c == '\\') || (c == '/')
						|| (c == '\b') || (c == '\f') || (c == '\n')
						|| (c == '\t')) {
						writer.Write('\\');
						writer.Write(c);
					} else if (c > byte.MaxValue) {
						if (c <= 0xFFFF) {
							writer.Write('\\');
							writer.Write('u');
							writer.Write(((int)c).ToString().PadLeft(4, '0'));
						}
					} else {
						writer.Write(c);
					}
				}
				writer.Write('\"');
			}
		}
	}
}
