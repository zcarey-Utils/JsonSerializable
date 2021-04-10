using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {

	/// <summary>
	/// Handles the reading and writing of JSON files.
	/// </summary>
	public static class Json {

		/// <summary>
		/// Serializes and writes JSON data to a file.
		/// </summary>
		/// <param name="json">The JSON data to serialize and write to a file.</param>
		/// <param name="path">The file path to write to.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">Thrown when serializing the data into JsonData fails.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="json"/> is null.</exception>
		/// <exception cref="IOException"></exception>
		public static void WriteToFile(IJsonSerializable json, string path) {
			if (json == null) throw new ArgumentNullException("json", "Unable to serialize null.");
			JsonData data = null;
			try {
				data = json.SaveToJson();
			}catch(Exception e) {
				throw new System.Runtime.Serialization.SerializationException("An error occured when trying to serialize the data.", e);
			}
			WriteToFile(json.SaveToJson(), path);
		}

		/// <summary>
		/// Writes the JSON data to the file system.
		/// </summary>
		/// <param name="json">The data to write.</param>
		/// <param name="path">The file path to write to.</param>
		/// <exception cref="IOException">Any error occurs while trying to write the file to the disk.</exception>
		private static void WriteToFile(JsonData json, string path) {
			if (json == null) throw new ArgumentNullException("json");
			Stream fileStream = null;
			try {
				fileStream = File.OpenWrite(path);
			}catch(Exception e) {
				throw new IOException("Unable to open file.", e);
			}
			try {
				using (JsonWriter writer = new JsonWriter(fileStream)) {
					json.Serialize(writer, 0);
					writer.Flush();
				}
			} catch (Exception e) {
				throw new IOException("An error occured while writing the JSON file.", e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="json"></param>
		/// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
		/// <exception cref="IOException"></exception>
		public static void ParseFromFile(string path, IJsonSerializable json) {
			JsonData data = ParseFromFile(path);
			try {
				json.LoadFromJson(data);
			}catch(Exception e) {
				throw new System.Runtime.Serialization.SerializationException("An error occured when trying to deserialize the data.", e);
			}
		}

		/// <exception cref="IOException"></exception>
		private static JsonData ParseFromFile(string path) {
			Stream fileStream = null;
			try {
				fileStream = File.OpenRead(path);
			}catch(Exception e) {
				throw new IOException("Unable to open file.", e);
			}
			try {
				using (JsonReader reader = new JsonReader(fileStream)) {
					JsonData data = JsonData.ParseValue(reader);
					if (reader.Read() != -1) return null;
					else return data;
				}
			} catch (Exception e) {
				throw new IOException("An error occured while reading the JSON file.", e);
			}
		}

		//Util function used to read through whitespace in the file.
		/// <exception cref="IOException"></exception>
		internal static void ReadWhitespace(JsonReader reader) {
			int raw;
			char c;
			while (true) {
				raw = reader.Peek();
				if (raw == -1) return;
				c = (char)raw;
				if (char.IsWhiteSpace(c)) reader.Read();
				else break;
			}
		}
	}
}
