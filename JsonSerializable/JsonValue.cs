using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// Used as a parent class for basic JSON types such as bool, number, and string.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class JsonValue<T> : JsonData {

		/// <summary>
		/// The value that is being held.
		/// </summary>
		public T Value { get; set; }

		/// <summary>
		/// Default constructor as required by <see cref="IJsonSerializable"/> for loading from JSON.
		/// </summary>
		public JsonValue() {
		}

		/// <summary>
		/// Creates a new JsonValue and initialized the value to the given value.
		/// </summary>
		/// <param name="value"></param>
		public JsonValue(T value) {
			Value = value;
		}

		/// <summary>
		/// Implicitly converts the JsonValue to it's native type by returning the contained value.
		/// </summary>
		/// <param name="json"></param>
		public static implicit operator T(JsonValue<T> json) {
			return json.Value;
		}
	}
}
