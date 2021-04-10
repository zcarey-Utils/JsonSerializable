using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// JsonValue that holds native number types such as <see cref="long"/> or <see cref="decimal"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class JsonNumber<T> : JsonValue<T> {

		/// <summary>
		/// Default constructor as required from <see cref="IJsonSerializable"/> for loading data from JSON
		/// </summary>
		public JsonNumber() : base() { }

		/// <summary>
		/// Constructor to pass value to base class.
		/// </summary>
		/// <param name="value"></param>
		public JsonNumber(T value) : base(value) { }
	}
}
