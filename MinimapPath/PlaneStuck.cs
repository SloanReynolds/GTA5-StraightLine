using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace MinimapPath {
	static class PlaneStuck {
		private const float DAMAGE_START_DISTANCE = 9f;
		private const int PROCESS_INTERVAL_MS = 650;

		public static readonly Vector3 Start = new Vector3(-1015.4f, 6259.6f, 0.1f);
		public static readonly Vector3 End = new Vector3(1346.6f, -2726.1f, 2.2f);

		private static int _nextRun = 0;
		public static int NextRun {
			get => _nextRun;
		}

		private static Vector3 _planeLine;
		public static Vector3 Plane => _planeLine;

		private static bool _isRunning = false;
		public static bool IsRunning => _isRunning;

		private static bool _isComplete = false;
		public static bool IsComplete => _isComplete;

		static PlaneStuck() {
			_planeLine = End - Start;
			_planeLine.Z = 0;
		}

		internal static void ToggleRunning() {
			_isRunning = !_isRunning;

			Checkpoints.Reset();
		}

		internal static void Process(Vector3 oldPos, Vector3 projectedPos, Ped player, bool isFreeCam = false) {
			_nextRun = Game.GameTime + PROCESS_INTERVAL_MS;

			if (_isComplete) {
				return;
			}

			//Damage Player Check
			float planeDist = projectedPos.DistanceToSquared(oldPos);
			if (planeDist > DAMAGE_START_DISTANCE) {
				float damage = planeDist / DAMAGE_START_DISTANCE / 2;
				damage = (float)Math.Pow(damage, 3);
				if (!isFreeCam) {
					Props.I.FireAtPlayer(player, damage);
				}
			}

			if (!isFreeCam) Checkpoints.UpdateCheckpoint(ProjectedPercent(oldPos));
		}

		public static Vector3 PercentToPos(float percent) {
			Vector3 newDisplacement = percent * Plane.Length() * Plane.Normalized;
			return Start + newDisplacement;
		}

		public static float ProjectedPercent(Vector3 oldPos) {
			Vector3 currDiff = oldPos - Start;
			float dot = Vector2.Dot(Plane, currDiff);
			//MinimapPath.Notify(dot + " " + Plane.LengthSquared() + " " + dot / Plane.LengthSquared());
			return dot / Plane.LengthSquared();
		}

		public static Vector3 ProjectedPos(Vector3 oldPos) {
			return PercentToPos(ProjectedPercent(oldPos));
		}

		public static Vector3 Project(Vector3 oldPos) {
			Vector3 newVec = ProjectedPos(oldPos);

			newVec.Z = oldPos.Z;
			return newVec;
		}

		internal static void ReportComplete() {
			_isComplete = true;

			//True Engineer!
			MinimapPath.Notify("Congratulations! You've earned the full-fledged title of 'Engineer!' Here's your hardhat and your gun to prove it.\nNow get out there and inspect some things!\n\nPress Shift+F4 to get started.");
		}
	}
}
