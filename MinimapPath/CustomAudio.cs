using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MinimapPath {
	internal static class CustomAudio {
		private static bool _playing = false;
		private static object _playingLock = new object();
		private static Timer _playingTimer = null; //To skip garbage collection.?
		private static float _currentVolume = 1f;

		private static Random _random = new Random();

		private static List<WavPlayerStream> _all = new List<WavPlayerStream>() {
			new WavPlayerStream(Properties.Resources.Boosh01),		//0
			new WavPlayerStream(Properties.Resources.Boosh02),
			new WavPlayerStream(Properties.Resources.Boosh03),
			new WavPlayerStream(Properties.Resources.Boosh04),
			new WavPlayerStream(Properties.Resources.Boosh05),
			new WavPlayerStream(Properties.Resources.Boosh06),		//5
			new WavPlayerStream(Properties.Resources.Boosh07),
			new WavPlayerStream(Properties.Resources.Boosh08),
			new WavPlayerStream(Properties.Resources.BooshTShirt),
			new WavPlayerStream(Properties.Resources.ComboBoosh),
			new WavPlayerStream(Properties.Resources.MultiBoosh),	//10
			new WavPlayerStream(Properties.Resources.PowerBoosh),
			new WavPlayerStream(Properties.Resources.shot0),
			new WavPlayerStream(Properties.Resources.shot1),
			new WavPlayerStream(Properties.Resources.shot2),
			new WavPlayerStream(Properties.Resources.shot3),		//15
			new WavPlayerStream(Properties.Resources.shot4),
			new WavPlayerStream(Properties.Resources.shot5),
			new WavPlayerStream(Properties.Resources.Beep),			//18
		};

		public static void PlayBoosh() {
			_PlayFromList(_random.Next(0, 8));
		}

		public static void PlaySpecialBoosh() {
			_PlayFromList(_random.Next(8, 12));
		}

		public static void PlayShot() {
			_PlayFromList(_random.Next(12, 18));
		}

		public static void PlayBeep() {
			_PlayFromList(18);
		}

		private static void _SetPlaying(bool playing, float duration = 0) {
			lock (_playingLock) {
				_playing = playing;
				if (_playing) {
					_playingTimer = new Timer((state) => _SetPlaying(false), null, (uint)(duration * 1000), Timeout.Infinite);
				} else {
					_playingTimer = null;
				}
			}
		}

		private static void _PlayFromList(int index) {
			if (_playing) return;

			var wp = _all[index];

			_SetPlaying(true, wp.Duration);
			wp.Play();
		}

		internal static void ChangeVolume(float v) {
			SetVolume(_currentVolume + v);

			MinimapPath.Notify($"Volume set to {_currentVolume}");
		}

		internal static void SetVolume(float v) {
			float old = _currentVolume;
			_currentVolume = (float)Math.Round(v.Clamped(0, 1), 2);
			if (old == _currentVolume) return;

			foreach (var item in _all) {
				item.SetVolume(_currentVolume);
			}
		}
	}
}
