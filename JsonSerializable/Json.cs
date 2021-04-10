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
		public static void Write(IJsonSerializable json, string filePath) {
			if (json == null) throw new ArgumentNullException("json", "Unable to serialize null.");
			JsonData data = null;
			try {
				data = json.SaveToJson();
			}catch(Exception e) {
				throw new System.Runtime.Serialization.SerializationException("An error occured when trying to serialize the data.", e);
			}
			Write(json.SaveToJson(), filePath);
		}

		/// <summary>
		/// Serializes and writes JSON data to a stream.
		/// </summary>
		/// <param name="json">The JSON data to serialize and write to a file.</param>
		/// <param name="stream">The file path to write to.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="json"/> is null.</exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">Thrown when serializing the data into JsonData fails.</exception>
		/// <exception cref="IOException"></exception>
		public static void Write(IJsonSerializable json, Stream stream) {
			if (json == null) throw new ArgumentNullException("json", "Unable to serialize null.");
			JsonData data = null;
			try {
				data = json.SaveToJson();
			} catch (Exception e) {
				throw new System.Runtime.Serialization.SerializationException("An error occured when trying to serialize the data.", e);
			}
			Write(data, stream);
		}
		
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="IOException">Any error occurs while trying to write the file to the disk.</exception>
		private static void Write(JsonData json, string path) {
			if (json == null) throw new ArgumentNullException("json");
			Stream fileStream = null;
			try {
				fileStream = File.OpenWrite(path);
			}catch(Exception e) {
				throw new IOException("Unable to open file.", e);
			}
			Write(json, fileStream);
		}

		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="IOException"></exception>
		private static void Write(JsonData json, Stream stream) {
			if (json == null) throw new ArgumentNullException("json");
			if (stream == null) throw new ArgumentNullException("stream");
			try {
				using (JsonWriter writer = new JsonWriter(stream)) {
					json.Serialize(writer, 0);
					writer.Flush();
				}
			} catch (Exception e) {
				throw new IOException("An error occured while writing the JSON file.", e);
			}
		}
		
		/// <summary>
		/// Reads JSON from a file and deserializes the data.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="json"></param>
		/// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public static void Read(string filePath, IJsonSerializable json) {
			JsonData data = Read(filePath);
			try {
				json.LoadFromJson(data);
			}catch(Exception e) {
				throw new System.Runtime.Serialization.SerializationException("An error occured when trying to deserialize the data.", e);
			}
		}

		/// <summary>
		/// Reads JSON from a stream and deserializes the data.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="json"></param>
		/// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public static void Read(Stream stream, IJsonSerializable json) {
			JsonData data = Read(stream);
			try {
				json.LoadFromJson(data);
			} catch (Exception e) {
				throw new System.Runtime.Serialization.SerializationException("An error occured when trying to deserialize the data.", e);
			}
		}

		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="IOException"></exception>
		private static JsonData Read(string filePath) {
			Stream fileStream = null;
			try {
				fileStream = File.OpenRead(filePath);
			}catch(Exception e) {
				throw new IOException("Unable to open file.", e);
			}
			return Read(fileStream);
		}

		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="IOException"></exception>
		private static JsonData Read(Stream stream) {
			if (stream == null) throw new ArgumentNullException("stream", "Stream can't be null.");
			try {
				using (JsonReader reader = new JsonReader(stream)) {
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
