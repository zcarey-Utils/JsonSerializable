using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	internal class JsonReader : IDisposable {

		private BufferedStream reader;
		private int buffer;
		private int temp;

		internal JsonReader(Stream stream) {
			reader = new BufferedStream(stream);
			buffer = reader.ReadByte();
		}

		~JsonReader() {
			Dispose();
		}

		public void Dispose() {
			reader.Dispose();
		}

		public void Close() {
			reader.Close();
		}

		public int Peek() {
			return buffer;
		}

		public int Read() {
			temp = buffer;
			buffer = reader.ReadByte();
			return temp;
		}
	}
}
