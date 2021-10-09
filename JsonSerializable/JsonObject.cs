using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// Essentially a dictionary with string key that can saved/loaded from a JSON file as a Json Object
	/// </summary>
	public class JsonObject : JsonData {

		/// <summary>
		/// All KeyItemPairs of contained JsonData. 
		/// </summary>
		public IEnumerable<KeyValuePair<string, JsonData>> Items { get => items.AsEnumerable(); }
		private Dictionary<string, JsonData> items;

		/// <summary>
		/// Default constructor as required to load data from JSON
		/// </summary>
		public JsonObject() {
			items = new Dictionary<string, JsonData>();
		}

		/// <summary>
		/// Initializes object with data from a dictionary.
		/// </summary>
		/// <param name="items"></param>
		public JsonObject(Dictionary<string, JsonData> items) {
			this.items = items;
		}

		/// <summary>
		/// Converts from JsonObject to dictionary by returning it's contained dictionary.
		/// </summary>
		/// <param name="data"></param>
		public static implicit operator Dictionary<string, JsonData>(JsonObject data) => data.items;

		/// <summary>
		/// Converts dictionary to JsonObject by creating a new JsonObject and copying all the items from the dictionary.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonObject(Dictionary<string, JsonData> data) => new JsonObject(data);

		///<inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
		public void Add(string key, JsonData value) {
			items[key] = value;
		}

		/// <summary>
		/// Sets or gets the value with the given key. If the key is not present in dictionary, null is returned.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public JsonData this[string key] {
			get {
				if (items.ContainsKey(key)) return items[key];
				else return null;
			}
			set => items[key] = value;
		}

		///<exception cref="IOException"></exception>
		///<exception cref="ArgumentOutOfRangeException"></exception>
		internal override void Serialize(JsonWriter writer, int depth, bool minimal) {
			if (items.Count > 0) {
				writer.Write('{');
				depth++;
				bool isFirst = true;
				foreach (KeyValuePair<string, JsonData> pair in items) {
					if (!isFirst) writer.Write(',');
					else isFirst = false;

					if (!minimal) {
						writer.WriteLine();
						writer.Write('\t', depth);
					}
					JsonString.Serialize(writer, pair.Key);
					writer.Write(':');
					if (!minimal) writer.Write(' ');
					pair.Value.Serialize(writer, depth, minimal);
				}
				depth--;
				if (!minimal) {
					writer.WriteLine();
					writer.Write('\t', depth);
				}
				writer.Write('}');
			} else {
				if (minimal) writer.Write("{}");
				else writer.Write("{ }");
			}
		}

		///<inheritdoc/>
		///<exception cref="IOException"></exception>
		///<exception cref="ArgumentOutOfRangeException"></exception>
		///<exception cref="FormatException"></exception>
		///<exception cref="ArgumentNullException"></exception>
		///<exception cref="ArgumentException"></exception>
		///<exception cref="IndexOutOfRangeException"></exception>
		///<exception cref="OverflowException"></exception>
		internal override void Parse(JsonReader reader) {
			Json.ReadWhitespace(reader);
			if (reader.Read() != '{') throw new FormatException("JsonObject was expecting \'{\'.");
			Json.ReadWhitespace(reader);

			while ((reader.Peek() != '}') && (reader.Peek() != -1)) {
				//Read key
				JsonString parsedKey = new JsonString();
				parsedKey.Parse(reader);
				string key = parsedKey.Value;
				if (key == null) throw new FormatException("JsonObject can\'t have \'null\' as a key.");
				Json.ReadWhitespace(reader);
				if (reader.Read() != ':') throw new FormatException("JsonObject was expecting a \':\'.");

				//Determine value;
				JsonData value = JsonData.ParseValue(reader);
				if (value == null) throw new ArgumentNullException("The parsed JsonData was unexpectedly null.");
				else {
					if (this.items.ContainsKey(key)) throw new FormatException("JsonObject can't have two entries with the same key.");
					this.items.Add(key, value);
				}
				Json.ReadWhitespace(reader);
				if (reader.Peek() == ',') {
					reader.Read(); //We are going to look, we can ignore the comma.
					Json.ReadWhitespace(reader);
					if (reader.Peek() == '}' || reader.Peek() == -1) throw new FormatException("JsonObject was expecting another entry, not \'}\'.");
				}
				Json.ReadWhitespace(reader); //Prepare for the next loop
			}
			Json.ReadWhitespace(reader);
			if (reader.Read() != '}') throw new FormatException("JsonObject was expecting a \'}\'.");
		}

		///<inheritdoc/>
		///<exception cref="ArgumentNullException"></exception>
		///<exception cref="ArgumentException"></exception>
		///<exception cref="InvalidCastException"></exception>
		public override void LoadFromJson(JsonData Data) {
			items.Clear();
			foreach (KeyValuePair<string, JsonData> pair in ((JsonObject)Data).items) {
				items.Add(pair.Key, pair.Value);
			}
		}
	}
}
