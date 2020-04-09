using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public abstract class JsonNumber<T> : JsonValue<T> {
		public JsonNumber() : base() { }
		public JsonNumber(T value) : base(value) { }
	}
}
