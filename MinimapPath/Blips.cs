using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace MinimapPath {
	static class Blips {
		private const float BLIP_SPACING = 169f;

		private static Vector3 _prevPos = Vector3.Zero;
		private static float _prevDistance = 0f;
		private static Vector3 _speedPos = Vector3.Zero;

		private static List<Blip> _blips;
		private static bool _isRunning = false;

		private static float _percentProgress = 0f;

		public static bool IsRunning => _isRunning;

		public static void Process(Vector3 pos) {
			_prevDistance = pos.DistanceToSquared(_speedPos);
			_speedPos = pos;
			if (pos.DistanceToSquared(_prevPos) > (BLIP_SPACING + _prevDistance * BLIP_SPACING)) {
				float perc = PlaneStuck.ProjectedPercent(pos);
				if (!PlaneStuck.IsRunning || PlaneStuck.IsComplete || perc > _percentProgress) {
					_AddBlip(pos);
					_percentProgress = perc;
				}
			}
		}

		public static void Start(Vector3 pos) {
			_blips = new List<Blip>();
			_isRunning = true;
			_AddBlip(pos);
		}

		public static void Stop() {
			_isRunning = false;

			_ClearBlips();
		}

		public static void CleanUp() {
			_ClearBlips();
		}

		private static void _ClearBlips() {
			if (_blips == null)
				return;
			foreach (Blip blip in _blips) {
				blip.Delete();
			}
			_blips = new List<Blip>();
		}

		public static void Add(Vector3 pos) {
			_AddBlip(pos);
		}

		private static void _AddBlip(Vector3 pos) {
			if (!_isRunning) return;
			var newBlip = World.CreateBlip(pos);
			newBlip.DisplayType = BlipDisplayType.BothMapNoSelectable;
			newBlip.IsHiddenOnLegend = true;
			newBlip.Sprite = BlipSprite.CriminalWanted;
			newBlip.Color = BlipColor.SimpleBlipDefault;
			newBlip.Scale = 0.2f;
			newBlip.IsShortRange = true;
			_blips.Add(newBlip);

			_prevPos = pos;
		}
	}
}
