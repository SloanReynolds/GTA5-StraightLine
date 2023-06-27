using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;

namespace MinimapPath {
	internal struct PosRot {
		private readonly float _x;
		private readonly float _y;
		private readonly float _z;
		private readonly float _heading;

		public Vector3 Vector => new Vector3(_x, _y, _z);
		public float Heading => _heading;

		public PosRot(float x, float y, float z, float heading) {
			this._x = x;
			this._y = y;
			this._z = z;
			this._heading = heading;
		}
	}
}
