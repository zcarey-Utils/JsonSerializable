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
		/// <exception cref="FormatException"></exception>
		private void ParseNull(JsonReader reader) {
			//if(reader.Peek() == 'n' || reader.Peek() == 'N') {
			reader.Read();
			if (reader.Peek() == 'u' || reader.Peek() == 'U') {
				reader.Read();
				if (reader.Peek() == 'l' || reader.Peek() == 'L') {
					reader.Read();
					if (reader.Peek() == 'l' || reader.Peek() == 'L') {
						reader.Read();
						Value = null;
						return;
					}
				}
			}
			//}
			throw new FormatException("JsonString was expecting \'null\'.");
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="FormatException"></exception>
		/// <exception cref="OverflowException"></exception>
		private static void ParseUnicode(JsonReader reader, out char c) {
			c = ' ';
			string str = "";
			for (int i = 0; i < 4; i++) {
				if (reader.Peek() == -1) throw new IndexOutOfRangeException("JsonString was expecting a unicode id, but end of file was reached.");
				else str += (char)reader.Read();
			}
			int uni = Convert.ToInt32(str, 16);
			c = (char)uni;
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="OverflowException"></exception>
		private static void ParseEscape(JsonReader reader, out char c) {
			int peek = reader.Read();

			if (peek == -1) {
				c = ' ';
				throw new IndexOutOfRangeException("Expected JsonString escape character, but end of file was reached.");
			} else if (peek == '\"' || peek == '\\' || peek == '/') c = (char)peek;
			else if (peek == 'b') c = '\b';
			else if (peek == 'f') c = '\f';
			else if (peek == 'n') c = '\n';
			else if (peek == 'r') c = '\r';
			else if (peek == 't') c = '\t';
			else if (peek == 'u') ParseUnicode(reader, out c);
			else {
				c = ' ';
				throw new FormatException("Jsonstring found an unknown escape character \'" + c + "\'");
			}
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <exception cref="FormatException"></exception>
		/// <exception cref="OverflowException"></exception>
		internal override void Parse(JsonReader reader) {
			StringBuilder str = new StringBuilder();
			Json.ReadWhitespace(reader);
			if (reader.Peek() == 'n' || reader.Peek() == 'N') {
				ParseNull(reader);
			} else {
				if (reader.Read() != '\"') throw new FormatException("JsonStrings must start with a \".");
				int peek;
				char c;
				while ((peek = reader.Read()) != '\"') {
					if (peek == -1) throw new IndexOutOfRangeException("The end of file was reached, but the end of the JsonString was never found.");
					c = (char)peek;

					if (c == '\\') {
						ParseEscape(reader, out c);
					}
					str.Append(c);
				}
				Value = str.ToString();
			}
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		internal override void Serialize(JsonWriter writer, int depth, bool minimal) {
			JsonString.Serialize(writer, Value);
		}

		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		internal static void Serialize(JsonWriter writer, string str) {
			if (str == null) writer.Write("null");
			else {
				writer.Write('\"');
				foreach (char c in str) {
					if ((c == '\"') || (c == '\\') || (c == '/')) {
						writer.Write('\\');
						writer.Write(c);
					} else if (c == '\b') {
						writer.Write("\\b");
					} else if(c == '\f') {
						writer.Write("\\f");
					} else if(c == '\n') {
						writer.Write("\\n");
					} else if(c == '\r') {
						writer.Write("\\r");
					} else if(c == '\t') {
						writer.Write("\\t");
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
