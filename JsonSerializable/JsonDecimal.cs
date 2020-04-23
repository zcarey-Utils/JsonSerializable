using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public class JsonDecimal : JsonNumber<double> {
		public JsonDecimal() : base() { }
		public JsonDecimal(double value) : base(value) { }

		public static implicit operator double(JsonDecimal data) => data.Value;
		public static explicit operator JsonDecimal(double data) => new JsonDecimal(data);
		public static explicit operator JsonDecimal(float data) => new JsonDecimal(data);

		public override bool LoadFromJson(JsonData Data) {
			if (Data is JsonDecimal) {
				this.Value = ((JsonDecimal)Data).Value;
				return true;
			} else {
				return false;
			}
		}

		internal override bool Parse(JsonReader reader) {
			throw new InvalidOperationException();
		}

		internal override void Serialize(JsonWriter writer, int depth) {
			writer.Write(Value);
		}
	}
}
