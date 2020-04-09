using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public interface IJsonSerializable {

		JsonData SaveToJson();
		bool LoadFromJson(JsonData Data);

	}

	public class SerializableList<T> : List<T>, IJsonSerializable where T : IJsonSerializable, new() {

		public SerializableList() : base() { }
		public SerializableList(List<T> list) : base(list) { }
		public SerializableList(IEnumerable<T> collection) : base(collection) { }

		public bool LoadFromJson(JsonData Data) {
			this.Clear();
			if (Data is JsonArray) {
				foreach (JsonData data in (JsonArray)Data) {
					T obj = new T();
					if (obj.LoadFromJson(data)) {
						this.Add(obj);
					} else return false;
				}
				return true;
			} else {
				return false;
			}
		}

		public JsonData SaveToJson() {
			JsonArray array = new JsonArray();

			foreach (T obj in this) {
				array.Add(obj.SaveToJson());
			}

			return array;
		}

	}

	public class SerializableDictionary<T> : Dictionary<string, T>, IJsonSerializable where T : IJsonSerializable, new() {

		public SerializableDictionary() : base() { }
		public SerializableDictionary(Dictionary<string, T> dict) : base(dict) { }

		public bool LoadFromJson(JsonData Data) {
			this.Clear();
			if (Data is JsonObject) {
				foreach (KeyValuePair<string, JsonData> pair in ((JsonObject)Data).Items) {
					T obj = new T();
					if (obj.LoadFromJson(pair.Value)) {
						this.Add(pair.Key, obj);
					} else return false;
				}
				return true;
			} else {
				return false;
			}
		}

		public JsonData SaveToJson() {
			JsonObject array = new JsonObject();

			foreach (KeyValuePair<string, T> pair in this) {
				array.Add(pair.Key, pair.Value.SaveToJson());
			}

			return array;
		}

	}
}
