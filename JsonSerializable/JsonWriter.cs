using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializable {
	internal class JsonWriter : IDisposable {

		private BufferedStream writer;
		private StringBuilder str = new StringBuilder();

		/// <exception cref="ArgumentNullException"></exception>
		internal JsonWriter(Stream stream) {
			writer = new BufferedStream(stream);
		}

		~JsonWriter() {
			Dispose();
		}

		public void Dispose() {
			writer.Dispose();
			str.Clear();
		}

		/// <exception cref="IOException"></exception>
		public void Close() {
			Flush();
			writer.Close();
		}

		/// <exception cref="IOException"></exception>
		public void Flush() {
			try {
				writer.Flush();
			}catch(Exception e) {
				throw new IOException("Unable to flush contents.", e);
			}
		}

		private void throwIOError(Exception e) {
			throw new IOException("There was an error writing to the file.", e);
		}

		/// <exception cref="Exception"></exception>
		private void Append() {
			byte[] bytes = Encoding.ASCII.GetBytes(str.ToString());
			writer.Write(bytes, 0, bytes.Length);
			str.Clear();
		}

		/// <exception cref="IOException"></exception>
		private void AppendLine() {
			try {
				str.AppendLine();
				Append();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine() {
			try {
				str.AppendLine();
				Append();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(byte value) {
			try {
				str.Append(value);
				Append();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(sbyte value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(ushort value) {try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(short value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			} 
		}

		/// <exception cref="IOException"></exception>
		public void Write(uint value) {
			try {
				str.Append(value);
				Append();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(int value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(ulong value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(long value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(float value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(double value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(decimal value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(char value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(bool value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(object value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(string value) {
			try {
				str.Append(value);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(char[] value) {
			try {
				str.Append(value);
				Append();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(char value, int repeat) {
			try {
				str.Append(value, repeat);
				Append();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(char[] value, int start, int count) {
			try {
				str.Append(value, start, count);
				Append();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void Write(string value, int start, int count) {
			try {
				str.Append(value, start, count);
				Append();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(byte value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(sbyte value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(ushort value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(IOException e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(short value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(uint value) {
			try {
				str.Append(value);
				AppendLine();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(int value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(ulong value) {
			try {
				str.Append(value);
				AppendLine();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(long value) {
			try {
				str.Append(value);
				AppendLine();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(float value) {
			try {
				str.Append(value);
				AppendLine();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(double value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(decimal value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(char value) {
			try {
				str.Append(value);
				AppendLine();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(bool value) {
			try {
				str.Append(value);
				AppendLine();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(object value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(string value) {
			try {
				str.Append(value);
				AppendLine();
			} catch (Exception e) { 
				throwIOError(e); 
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(char[] value) {
			try {
				str.Append(value);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(char value, int repeat) {
			try {
				str.Append(value, repeat);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(char[] value, int start, int count) {
			try {
				str.Append(value, start, count);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

		/// <exception cref="IOException"></exception>
		public void WriteLine(string value, int start, int count) {
			try {
				str.Append(value, start, count);
				AppendLine();
			}catch(Exception e) {
				throwIOError(e);
			}
		}

	}
}
