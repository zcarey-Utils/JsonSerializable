using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public class JsonString : JsonValue<string> {
		public JsonString() : base() { }
		public JsonString(string value) : base(value) { }

		public override bool LoadFromJson(JsonData Data) {
			if (Data is JsonString) {
				this.Value = ((JsonString)Data).Value;
				return true;
			} else {
				return false;
			}
		}

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

		internal override void Serialize(JsonWriter writer, int depth) {
			JsonString.Serialize(writer, Value);
		}

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
