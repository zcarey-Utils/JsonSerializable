using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public abstract class JsonValue<T> : JsonData {
		public T Value { get; set; }

		public JsonValue() {

		}

		public JsonValue(T value) {
			Value = value;
		}

		public static implicit operator T(JsonValue<T> json) {
			return json.Value;
		}
	}
}
