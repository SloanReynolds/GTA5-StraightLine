using System;
using System.Windows.Media.Imaging;
using GTA;
using GTA.Math;

namespace MinimapPath {
	static class Extensions {
		public static string Print(this Vector2 vec) {
			return $"x{vec.X} y{vec.Y} ";
		}
		public static string Print(this Vector3 vec) {
			return $"x{vec.X} y{vec.Y} z{vec.Z} ";
		}

		public static string PrintCode(this Vector3 vec, Ped ped) {
			return $"new PosRot({vec.X}f, {vec.Y}f, {vec.Z-1f}f, {ped.Heading}f)";
		}

		public static string PercentDisplay(this float percent) {
			return string.Format("{0:P2}", percent);
		}
		public static string PercentDisplay(this Vector3 vec) {
			return PlaneStuck.ProjectedPercent(vec).PercentDisplay();
		}
		public static Vector3 RotateAround(this Vector3 vec, Vector3 axis, float angle) {
			Quaternion q = new Quaternion(axis, angle);
			Quaternion qVec = new Quaternion(vec.X, vec.Y, vec.Z, 0f);
			Quaternion result = q * qVec * Quaternion.Conjugate(q);
			return new Vector3(result.X, result.Y, result.Z);
		}
		public static void RotateAround(this Entity ent, Vector3 axis, float angle) {
			angle = (float)(angle * (Math.PI / 180f));
			Quaternion rot = new Quaternion(axis, angle);
			ent.Quaternion = rot * ent.Quaternion;
		}
		public static void Roll(this Entity ent, float angle) {
			ent.RotateAround(ent.ForwardVector, angle);
		}
		public static void Yaw(this Entity ent, float angle) {
			ent.RotateAround(-ent.UpVector, angle);
		}
		public static void Pitch(this Entity ent, float angle) {
			ent.RotateAround(ent.RightVector, angle);
		}
		public static float ToHeading(this Vector3 vec) {
			float theta = (float)Math.Acos(vec.Normalized.X) * 180f / (float)Math.PI;
			if (vec.Y < 0) theta *= -1;
			return theta;
		}
		public static float ToHeading(this Vector3 vec, float additionalAngle) {
			float theta = ToHeading(vec);
			theta += 180f + additionalAngle;
			theta %= 360f;
			theta -= 180f;
			return theta;
		}
		public static float ToPitch(this Vector3 vec) {
			float theta = (float)Math.Acos(vec.Normalized.Z) * 180f / (float)Math.PI;
			if (vec.Y < 0) theta *= -1;
			return theta;
		}
		public static float ToPitch(this Vector3 vec, float additionalAngle) {
			float theta = ToPitch(vec);
			theta += 180f + additionalAngle;
			theta %= 360f;
			theta -= 180f;
			return theta;
		}
		public static Vector3 Grounded(this Vector3 vec, float specifiedHeight = 0) {
			if (specifiedHeight != 0) {
				return new Vector3(vec.X, vec.Y, World.GetGroundHeight(new Vector3(vec.X, vec.Y, specifiedHeight)));
			}
			return new Vector3(vec.X, vec.Y, World.GetGroundHeight(new Vector2(vec.X, vec.Y)));
		}

		public static float Clamped(this float i, float vLow, float vHigh) {
			if (i < vLow) return vLow;
			if (i > vHigh) return vHigh;
			return i;
		}
	}
}
