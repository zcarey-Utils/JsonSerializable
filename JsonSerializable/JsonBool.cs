using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// JsonValue for the native type <see cref="bool"/>.
	/// </summary>
	public class JsonBool : JsonValue<bool> {

		/// <inheritdoc/>
		public JsonBool() : base() { }

		/// <inheritdoc/>
		public JsonBool(bool value) : base(value) { }

		/// <summary>
		/// Converts the JsonValue to a bool by returning the contained value.
		/// </summary>
		/// <param name="data"></param>
		public static implicit operator bool(JsonBool data) => data.Value;

		/// <summary>
		/// Converts a bool to a JsonValue by creating a new JsonValue and setting it's value to the bool.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonBool(bool data) => new JsonBool(data);

		/// <inheritdoc/>
		/// <exception cref="InvalidCastException"></exception>
		public override void LoadFromJson(JsonData Data) {
			this.Value = ((JsonBool)Data).Value;
		}

		/// <exception cref="IOException"></exception>
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

		/// <exception cref="IOException"></exception>
		internal override void Serialize(JsonWriter writer, int depth) {
			writer.Write(Value);
		}
	}
}
