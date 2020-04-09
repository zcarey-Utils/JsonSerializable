using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public class JsonInteger : JsonNumber<long> {
		public JsonInteger() : base() { }
		public JsonInteger(long value) : base(value) { }

		public override bool LoadFromJson(JsonData Data) {
			if (Data is JsonInteger) {
				this.Value = ((JsonInteger)Data).Value;
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
