using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimapPath {
	internal class Inspections {
		private List<PosRot> _assignments = new List<PosRot>() {
			new PosRot(2875.925f, 5026.389f, 30.76876f, 119.7328f),
			new PosRot(635.0869f, -1808.711f, 9.693849f, 338.0852f),
			new PosRot(-634.4974f, 590.0632f, 130.6572f, 85.03202f),
			new PosRot(1098.563f, -207.4314f, 54.94822f, 143.2525f),
			new PosRot(-74.61349f, 621.0673f, 204.6417f, 272.8257f),
			new PosRot(-408.4603f, 4376.717f, 52.41923f, 63.63473f),
			new PosRot(587.6799f, -744.7433f, 10.39138f, 179.613f),
			new PosRot(-210.6359f, -2561.22f, 44.89586f, 38.98191f),
			new PosRot(964.4084f, -2552.385f, 29.04682f, 109.0406f),
			new PosRot(-216.5217f, 4258.861f, 30.30041f, 258.26f),
			new PosRot(1246.846f, -1122.512f, 49.75781f, 343.159f),
			new PosRot(629.8868f, -1505.046f, 8.72192f, 348.5876f),
			new PosRot(111.1996f, 3332.764f, 32.06013f, 336.042f),
			new PosRot(-1073.856f, -644.9175f, 18.16555f, 99.019f),
			new PosRot(667.3715f, -2083.67f, 7.406595f, 29.62212f),
			new PosRot(74.22257f, -619.4459f, 30.52384f, 305.7401f),
			new PosRot(598.4619f, -2115.771f, 4.752252f, 323.9219f),
			new PosRot(-84.22175f, -817.8181f, 325.084f, 74.69757f),
		};

		private int _currentPos = 0;
		private bool _completed = false;

		internal void GoToNext() {
			if (_assignments.Count == 0)
				return;

			if (_currentPos >= _assignments.Count) {
				_currentPos = 0;
				_completed = true;
				MinimapPath.Notify("All inspections complete. Great work! (Keep going if you want to start inspections over)");
			}

			MinimapPath.SetPositionRotation(_assignments[_currentPos], true);

			_currentPos++;
		}
	}
}
