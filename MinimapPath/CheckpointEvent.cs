using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace MinimapPath {
	internal class CheckpointEvent {
		public float Distance { get; }

		private bool _attainedTriggered;
		private float _specifiedHeight = 0f;
		private Action<CheckpointEvent> attainedAction { get; }
		private Action<CheckpointEvent> _respawnAction { get; }


		public Vector3 Position {
			get {
				Vector3 vec = PlaneStuck.PercentToPos(Distance);
				if (SpecifiedHeight != 0f)
					vec.Z = SpecifiedHeight;
				return vec;
			}
		}
		public float SpecifiedHeight { get => _specifiedHeight; set => _specifiedHeight = value; }
		public CheckpointEvent(float distance) : this(distance, null, null) { }

		public CheckpointEvent(float distance, Action<CheckpointEvent> attainAction) : this(distance, attainAction, null) { }

		public CheckpointEvent(float distance, Action<CheckpointEvent> attainAction, Action<CheckpointEvent> respawnAction) {
			this.Distance = distance;
			this.attainedAction = attainAction;
			this._respawnAction = respawnAction;
			_attainedTriggered = false;
		}

		public void TriggerAttained() {
			if (!_attainedTriggered)
				attainedAction.Invoke(this);
			_attainedTriggered = true;
		}

		public void TriggerRespawn() {
			_respawnAction?.Invoke(this);
		}

		internal void Reset() {
			_attainedTriggered = false;
		}
	}
}
