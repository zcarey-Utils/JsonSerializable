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

		public void Close() {
			writer.Flush();
			writer.Close();
		}

		public void Flush() {
			writer.Flush();
		}

		private void Append() {
			byte[] bytes = Encoding.ASCII.GetBytes(str.ToString());
			writer.Write(bytes, 0, bytes.Length);
			str.Clear();
		}

		private void AppendLine() {
			str.AppendLine();
			Append();
		}

		public void WriteLine() {
			str.AppendLine();
			Append();
		}

		public void Write(byte value) {
			str.Append(value);
			Append();
		}

		public void Write(sbyte value) {
			str.Append(value);
			Append();
		}

		public void Write(ushort value) {
			str.Append(value);
			Append();
		}

		public void Write(short value) {
			str.Append(value);
			Append();
		}

		public void Write(uint value) {
			str.Append(value);
			Append();
		}

		public void Write(int value) {
			str.Append(value);
			Append();
		}

		public void Write(ulong value) {
			str.Append(value);
			Append();
		}

		public void Write(long value) {
			str.Append(value);
			Append();
		}

		public void Write(float value) {
			str.Append(value);
			Append();
		}

		public void Write(double value) {
			str.Append(value);
			Append();
		}

		public void Write(decimal value) {
			str.Append(value);
			Append();
		}

		public void Write(char value) {
			str.Append(value);
			Append();
		}

		public void Write(bool value) {
			str.Append(value);
			Append();
		}

		public void Write(object value) {
			str.Append(value);
			Append();
		}

		public void Write(string value) {
			str.Append(value);
			Append();
		}

		public void Write(char[] value) {
			str.Append(value);
			Append();
		}

		public void Write(char value, int repeat) {
			str.Append(value, repeat);
			Append();
		}

		public void Write(char[] value, int start, int count) {
			str.Append(value, start, count);
			Append();
		}

		public void Write(string value, int start, int count) {
			str.Append(value, start, count);
			Append();
		}

		public void WriteLine(byte value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(sbyte value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(ushort value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(short value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(uint value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(int value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(ulong value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(long value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(float value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(double value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(decimal value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(char value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(bool value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(object value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(string value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(char[] value) {
			str.Append(value);
			AppendLine();
		}

		public void WriteLine(char value, int repeat) {
			str.Append(value, repeat);
			AppendLine();
		}

		public void WriteLine(char[] value, int start, int count) {
			str.Append(value, start, count);
			AppendLine();
		}

		public void WriteLine(string value, int start, int count) {
			str.Append(value, start, count);
			AppendLine();
		}

	}
}
