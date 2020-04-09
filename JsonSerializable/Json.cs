using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	public static class Json {

		public static bool WriteToFile(IJsonSerializable json, string path) {
			if (json == null) return false;
			else return WriteToFile(json.SaveToJson(), path);
		}

		private static bool WriteToFile(JsonData json, string path) {
			if (json == null) {
//				Logger.Log(LogLevel.Warn, "Null JsonObject, ignoring write request.");
				return false;
			}
			try {
				using (JsonWriter writer = new JsonWriter(File.OpenWrite(path))) {
					json.Serialize(writer, 0);
					writer.Flush();
				}
				return true;
			} catch (Exception e) {
//				Logger.Log(LogLevel.Error, "Error writing JSON file.", e);
				return false;
			}
		}

		public static bool ParseFromFile(string path, IJsonSerializable json) {
			return json?.LoadFromJson(ParseFromFile(path)) ?? false;
		}

		private static JsonData ParseFromFile(string path) {
			try {
				using (JsonReader reader = new JsonReader(File.OpenRead(path))) {
					JsonData data = JsonData.ParseValue(reader);
					if (reader.Read() != -1) return null;
					else return data;
				}
			} catch (Exception e) {
//				Logger.Log(LogLevel.Error, "Could not read JSON file.", e);
				return null;
			}
		}

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
