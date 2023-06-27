using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Runtime.InteropServices.ComTypes;

namespace MinimapPath {
	[DesignerCategory("")] //This stops VS from trying to open it in design view
	internal class WavPlayerStream : SoundPlayer {
		private float _currentVolume = 1f;
		private int _byteRate = 0;
		private int _size;
		private Stream _originalStream;

		private List<SubChunkHeader> _subChunks = new List<SubChunkHeader>();

		public int ByteRate => _byteRate;
		public int Size => _size;
		public float Duration => (float)_size / (float)_byteRate;

		public WavPlayerStream(UnmanagedMemoryStream wavStream) : base(wavStream) {
			_originalStream = new MemoryStream(new byte[wavStream.Length], true);
			Stream = new MemoryStream(new byte[wavStream.Length], true);

			_Copy(wavStream, _originalStream);
			_Copy(wavStream, Stream);

			{ // Make subchucks
				long offset = wavStream.Position;

				//Get byteRate
				wavStream.Seek(28, SeekOrigin.Begin);
				_byteRate = _Read4Bytes(wavStream);

				//Get SubchunkHeaders
				wavStream.Position = 36;
				SubChunkHeader latest;
				do {
					latest = new SubChunkHeader(wavStream);
					_subChunks.Add(latest);
					wavStream.Position += latest.Size;
					_size += latest.Size;
				} while (wavStream.Position < wavStream.Length);

				wavStream.Position = offset;
			}
		}

		private void _Copy(Stream s1, Stream s2) {
			long offset = s1.Position;
			long offset2 = s2.Position;

			s1.CopyTo(s2);

			s1.Position = offset;
			s2.Position = offset2;
		}

		public void SetVolume(float volume) {
			var newStream = new MemoryStream(new byte[_originalStream.Length], true);

			//Sync-O-Tron Max
			_originalStream.Seek(0, SeekOrigin.Begin);
			newStream.Seek(0, SeekOrigin.Begin);

			foreach (var sub in _subChunks) {
				long stop = sub.DataOffset + sub.Size;

				//Write headers
				while (newStream.Position < sub.DataOffset) {
					newStream.WriteByte((byte)_originalStream.ReadByte());
				}

				//Shiggity shiggity shwaa data
				while (newStream.Position < stop) {
					short newVal = (short)(_Read2Bytes(_originalStream) * volume);
					_Write2Bytes(newStream, newVal);
				}
			}

			_originalStream.Seek(0, SeekOrigin.Begin);
			newStream.Seek(0, SeekOrigin.Begin);

			Stream = newStream;

			_currentVolume = volume;
		}

		public new void Play() {
			base.Play();
		}

		private static short _Read2Bytes(Stream stream) {
			return (short)((stream.ReadByte() << (8 * 0)) + (stream.ReadByte() << (8 * 1)));
		}

		private static void _Write2Bytes(Stream stream, short num) {
			stream.WriteByte((byte)num);
			stream.WriteByte((byte)(num >> 8));
		}

		private static int _Read4Bytes(Stream stream) {
			return (stream.ReadByte() << (8 * 0)) + (stream.ReadByte() << (8 * 1)) + (stream.ReadByte() << (8 * 2)) + (stream.ReadByte() << (8 * 3));
		}

		private struct SubChunkHeader {
			public int SubChunkID;
			public long DataOffset;
			public int Size;

			public SubChunkHeader(Stream stream) {
				SubChunkID = _Read4Bytes(stream);
				Size = _Read4Bytes(stream);
				DataOffset = stream.Position;
			}
		}
	}
}
