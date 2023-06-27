using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;

namespace MinimapPath {
	internal class BlipFaker {
		private const int NUMBER = 350;
		private const float JITTER_WEAKNESS = 1.1f;
		private const float JITTER_STRONG = 6f;
		private const int JITTER_STRONG_CHANCE = 35;

		private const float HARMONIC1 = 2.3f;
		private const float HARMONIC1_WEAKNESS = 5.76f;
		private const double HARMONIC1_PHASE_START = Math.PI / 4f;

		private const float HARMONIC2 = 10.744f;
		private const float HARMONIC2_WEAKNESS = 10.96f;
		private const double HARMONIC2_PHASE_START = 0;

		private Vector3 _endPoint;
		private Random _rnd = new Random();

		public BlipFaker(Vector3 endPoint) {
			this._endPoint = endPoint;
		}

		public void Run() {
			var startPoint = PlaneStuck.Start;
			foreach (Vector3 next in _GetNextPos(startPoint)) {
				Blips.Add(next);
			}
		}

		private IEnumerable<Vector3> _GetNextPos(Vector3 startPoint) {
			Vector3 linearPart = (_endPoint - startPoint) / NUMBER;
			Vector3 current = startPoint;
			
			for (int i = 0; i < NUMBER; i++) {
				double iRad = i * 2 * Math.PI / NUMBER;
				double harm1 = Math.Sin((iRad + HARMONIC1_PHASE_START) * HARMONIC1) / HARMONIC1_WEAKNESS;

				double harm2 = Math.Sin((iRad + HARMONIC2_PHASE_START) * HARMONIC2) / HARMONIC2_WEAKNESS;

				var jitter = (Vector3.RandomXY() / JITTER_WEAKNESS);
				if (_rnd.Next(0, JITTER_STRONG_CHANCE) == 0) {
					jitter *= JITTER_STRONG;
				}

				current += linearPart * (float)(1 + harm1 + harm2);

				if (current.DistanceTo(_endPoint) <= 15f) {
					break;
				}

				yield return current + jitter;
			}
		}
	}
}
