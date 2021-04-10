using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// Essentially an array that can be saved/loaded to a JSON.
	/// </summary>
	public class JsonArray : JsonData, IEnumerable<JsonData> {

		private List<JsonData> Values;

		/// <summary>
		/// Default constructor as required to load from a JSON.
		/// </summary>
		public JsonArray() {
			Values = new List<JsonData>();
		}

		/// <summary>
		/// Copies values from given list.
		/// </summary>
		/// <param name="values"></param>
		public JsonArray(List<JsonData> values) {
			Values = values;
		}

		/// <summary>
		/// Copies values from given array.
		/// </summary>
		/// <param name="values"></param>
		public JsonArray(JsonData[] values) {
			Values = new List<JsonData>(values);
		}

		/// <summary>
		/// Convert JsonArray to <see cref="List{JsonData}"/> by returning its contained list.
		/// </summary>
		/// <param name="data"></param>
		public static implicit operator List<JsonData>(JsonArray data) => data.Values;

		/// <summary>
		/// Converts a <see cref="List{JsonData}"/> to JsonArray by copying the items.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonArray(List<JsonData> data) => new JsonArray(data);

		///<inheritdoc/>
		///<exception cref="InvalidCastException"></exception>
		public override void LoadFromJson(JsonData Data) {
			Values.Clear();
			foreach (JsonData data in (JsonArray)Data) {
				Values.Add(data);
			}
		}

		///<inheritdoc cref="List{T}.Add(T)"/>
		public void Add(JsonData obj) {
			Values.Add(obj);
		}

		///<inheritdoc cref="List{T}.GetEnumerator"/>
		public IEnumerator<JsonData> GetEnumerator() {
			return Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Values.GetEnumerator();
		}

		///<inheritdoc/>
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

		///<inheritdoc/>
		///<exception cref="FormatException"></exception>
		///<exception cref="IOException"></exception>
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
