using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// JsonValue for the native type <see cref="long"/>.
	/// </summary>
	public class JsonInteger : JsonNumber<long> {

		/// <inheritdoc/>
		public JsonInteger() : base() { }

		/// <inheritdoc/>
		public JsonInteger(long value) : base(value) { }

		/// <summary>
		/// Converts the JsonValue to a long by returning the contained value.
		/// </summary>
		/// <param name="data"></param>
		public static implicit operator long(JsonInteger data) => data.Value;

		/// <summary>
		/// Converts a long to a JsonValue by creating a new JsonValue and setting its value to the long.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonInteger(long data) => new JsonInteger(data);

		/// <summary>
		/// Converts a ulong to a JsonValue by creating a new JsonValue and setting its value to the ulong.
		/// </summary>
		/// <param name="data"></param>
		public static explicit operator JsonInteger(ulong data) => new JsonInteger((long)data);

		/// <inheritdoc/>
		/// <exception cref="InvalidCastException"></exception>
		public override void LoadFromJson(JsonData Data) {
			this.Value = ((JsonInteger)Data).Value;
		}

		/// <exception cref="InvalidOperationException"></exception>
		internal override void Parse(JsonReader reader) {
			//Should be getting parsed in a parent class.
			throw new InvalidOperationException();
		}

		/// <exception cref="IOException"></exception>
		internal override void Serialize(JsonWriter writer, int depth, bool minimal) {
			writer.Write(Value);
		}
	}
}
