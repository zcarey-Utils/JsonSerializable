using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// The interface that is used for any type that should be able to convert to <see cref="JsonData"/> to it can be save/load from a JSON file.
	/// </summary>
	public interface IJsonSerializable {

		/// <summary>
		/// Serialize the data to be saved to file.
		/// This method is expected to throw an exception if the data is unable to be serialized.
		/// </summary>
		/// <returns>The serialized data to be written to file.</returns>
		/// <exception cref="Exception"></exception>
		JsonData SaveToJson();

		/// <summary>
		/// Deserialize the data read from a file.
		/// This method is expected to throw an exception if the data is unable to be deserialized.
		/// </summary>
		/// <param name="Data">The data that needs to be deserialized.</param>
		/// <exception cref="Exception"></exception>
		void LoadFromJson(JsonData Data);

	}

	/// <summary>
	/// A list that implements <see cref="IJsonSerializable"/> to be able to save/load from a JSON.
	/// The element type must be an IJsonSerializable type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SerializableList<T> : List<T>, IJsonSerializable where T : IJsonSerializable, new() {

		/// <summary>
		/// Default empty constructor as required for JSON loading.
		/// </summary>
		public SerializableList() : base() { }

		/// <inheritdoc cref="List{T}(IEnumerable{T})"/>
		public SerializableList(List<T> list) : base(list) { }

		/// <inheritdoc/>
		public SerializableList(IEnumerable<T> collection) : base(collection) { }

		/// <inheritdoc/>
		public void LoadFromJson(JsonData Data) {
			this.Clear();
			foreach (JsonData data in (JsonArray)Data) {
				T obj = new T();
				obj.LoadFromJson(data);
				this.Add(obj);
			}
		}

		/// <inheritdoc/>
		public JsonData SaveToJson() {
			JsonArray array = new JsonArray();

			foreach (T obj in this) {
				array.Add(obj.SaveToJson());
			}

			return array;
		}

	}

	/// <summary>
	/// A dictionary that implements <see cref="IJsonSerializable"/> to be able to save/load from a JSON.
	/// The element type must be an IJsonSerializable type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SerializableDictionary<T> : Dictionary<string, T>, IJsonSerializable where T : IJsonSerializable, new() {

		/// <summary>
		/// Default constructor as required for loading IJsonSerializable.
		/// </summary>
		public SerializableDictionary() : base() { }

		/// <inheritdoc cref="Dictionary{TKey, TValue}(IDictionary{TKey, TValue})"/>
		public SerializableDictionary(Dictionary<string, T> dict) : base(dict) { }

		/// <inheritdoc/>
		public void LoadFromJson(JsonData Data) {
			this.Clear();
			foreach (KeyValuePair<string, JsonData> pair in ((JsonObject)Data).Items) {
				T obj = new T();
				obj.LoadFromJson(pair.Value);
				this.Add(pair.Key, obj);
			}
		}

		/// <inheritdoc/>
		public JsonData SaveToJson() {
			JsonObject array = new JsonObject();

			foreach (KeyValuePair<string, T> pair in this) {
				array.Add(pair.Key, pair.Value.SaveToJson());
			}

			return array;
		}

	}
}
