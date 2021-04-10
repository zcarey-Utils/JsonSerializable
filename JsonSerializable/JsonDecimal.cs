using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// JsonValue for the native type <see cref="double"/>
	/// </summary>
	public class JsonDecimal : JsonNumber<double> {

		/// <inheritdoc/>
		public JsonDecimal() : base() { }

		/// <inheritdoc/>
		public JsonDecimal(double value) : base(value) { }

		/// <summary>
		/// Converts the JsonValue to a double by returning the contained value.
		/// </summary>
		/// <param name="data"></param>
		public static implicit operator double(JsonDecimal data) => data.Value;

		/// <summary>
		/// Converts the double to a JsonValue by creating a new JsonValue and setting its value to the double.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonDecimal(double data) => new JsonDecimal(data);

		/// <summary>
		/// Converts the float to a JsonValue by creating a new JsonValue and setting its value to the float.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonDecimal(float data) => new JsonDecimal(data);

		/// <inheritdoc/>
		/// <exception cref="InvalidCastException"></exception>
		public override void LoadFromJson(JsonData Data) {
			this.Value = ((JsonDecimal)Data).Value;
		}

		/// <exception cref="InvalidOperationException"></exception>
		internal override bool Parse(JsonReader reader) {
			//Parsing should be done in a parent class.
			throw new InvalidOperationException();
		}

		/// <exception cref="IOException"></exception>
		internal override void Serialize(JsonWriter writer, int depth) {
			writer.Write(Value);
		}
	}
}
