using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public class JsonArray : JsonData, IEnumerable<JsonData> {

		private List<JsonData> Values;

		public JsonArray() {
			Values = new List<JsonData>();
		}

		public JsonArray(List<JsonData> values) {
			Values = values;
		}

		public JsonArray(JsonData[] values) {
			Values = new List<JsonData>(values);
		}

		public override bool LoadFromJson(JsonData Data) {
			if (Data is JsonArray) {
				Values.Clear();
				foreach (JsonData data in (JsonArray)Data) {
					Values.Add(data);
				}
				return true;
			} else {
				return false;
			}
		}

		public void Add(JsonData obj) {
			Values.Add(obj);
		}

		public IEnumerator<JsonData> GetEnumerator() {
			return Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Values.GetEnumerator();
		}

		internal override void Serialize(JsonWriter writer, int depth) {
			if (Values.Count > 0) {
				writer.Write('[');
				depth++;
				bool isFirst = true;
				foreach (JsonData obj in Values) {
					if (!isFirst) writer.Write(',');
					else isFirst = false;
					writer.WriteLine();
					writer.Write('\t', depth);
					obj.Serialize(writer, depth);
				}
				depth--;
				writer.WriteLine();
				writer.Write('\t', depth);
				writer.Write(']');
			} else {
				writer.Write("[ ]");
			}
		}

		internal override bool Parse(JsonReader reader) {
			Json.ReadWhitespace(reader);
			if (reader.Read() != '[') return false;

			Json.ReadWhitespace(reader);
			while ((reader.Peek() != ']') && (reader.Peek() != -1)) {
				JsonData value = JsonData.ParseValue(reader);
				if (value == null) return false;
				this.Values.Add(value);
				Json.ReadWhitespace(reader);
				if (reader.Peek() == ',') {
					reader.Read(); //Discard separator
					Json.ReadWhitespace(reader);
					if (reader.Peek() == ']' || reader.Peek() == -1) return false;
				}
				Json.ReadWhitespace(reader); //Prepare for next loop
			}
			Json.ReadWhitespace(reader);
			if (reader.Read() != ']') return false;
			else return true;
		}

	}
}
