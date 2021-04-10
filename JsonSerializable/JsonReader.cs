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

		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="IOException"></exception>
		internal JsonReader(Stream stream) {
			reader = new BufferedStream(stream);
			buffer = ReadByte();
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

		/// <exception cref="IOException"></exception>
		private int ReadByte() {
			try {
				return reader.ReadByte();
			}catch(Exception e) {
				throw new IOException("Error occured while reading from the stream.", e);
			}
		}

		public int Peek() {
			return buffer;
		}

		/// <exception cref="IOException"></exception>
		public int Read() {
			temp = buffer;
			buffer = ReadByte();
			return temp;
		}
	}
}
