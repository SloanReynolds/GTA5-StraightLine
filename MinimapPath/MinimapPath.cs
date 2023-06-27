using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace MinimapPath {
	public class MinimapPath : Script {
		private readonly Vector3 _LADDER_SECTION = new Vector3(873f, -930f, 27f);

		private int _frames = 0;
		private Ped _playerCharacter => Game.Player.Character;
		private Vector3 _CurrentPos => GetCurrentView().Position;

		private Props _props = new Props();

		public static MinimapPath I;

		public MinimapPath() {
			I = this;

			Tick += _OnTick;
			KeyDown += _OnKeyDown;
			Aborted += _CleanUp;

			CustomAudio.SetVolume(0.5f);

			Blips.Start(_CurrentPos);

			Props.I.RestrictHatToPlayerHeight();
		}

		private void _CleanUp(object sender, EventArgs e) {
			Blips.CleanUp();
			_CleanUpPlaneStuck();
		}

		private void _CleanUpPlaneStuck() {
			_props.DeleteAll();
			Checkpoints.CleanUp();
			MapGen.CleanUp();
		}

		private void _PlaneStuckStarted() {
			if (!MapGen.IsLadderLoaded) {
				Vector3 oldPos = _playerCharacter.Position;
				//dt = DOWN FUCKING TOWN BABY!!!
				SetPosition(_LADDER_SECTION);
				MapGen.Populate();
				SetPosition(oldPos);
			} else {
				MapGen.Populate();
			}

			Game.Player.IsInvincible = false;
			Function.Call(Hash.REMOVE_WEAPON_FROM_PED, _playerCharacter, WeaponHash.Parachute);
		}

		private void _PlaneStuckEnded() {
			_CleanUpPlaneStuck();
		}

		private Inspections _inspections = new Inspections();
		private bool _nextAssignmentNotification = false;
		private void _OnKeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.F4) {
				GameplayCamera.RelativeHeading = 0f;
				GameplayCamera.RelativePitch = 0f;
				if (Game.IsControlPressed(GTA.Control.Sprint)) {
					if (!PlaneStuck.IsComplete) {
						//return;
					}

					//Completed game!
					_inspections.GoToNext();

					if (!_nextAssignmentNotification) {
						_WriteLine("Press Shift+F4 again to see your next assignment!");
						_nextAssignmentNotification = true;
					}
				}
			} else if (e.KeyCode == Keys.F7 && Game.IsControlPressed(GTA.Control.Sprint)) {
				//Clear ALL old blips.
				Blips.CleanUp();
				//Teleport to 63%
				Vector3 vec = _SetPercentPosition(0.631f);

				//Build new blip path
				BlipFaker faker = new BlipFaker(vec);
				faker.Run();
			} else if (e.KeyCode == Keys.F9) {
				_WriteLine(_CurrentPos.Print());
				_WriteLine($"Progress: {_CurrentPos.PercentDisplay()}");
				//_WriteLine(BaseDirectory);

				//_WriteLine(Props.I.HardHatPos.Print());

				if (File.Exists(BaseDirectory + "/MinimapPath.log")) {
					File.AppendAllLines(BaseDirectory + "/MinimapPath.log", new string[] { "\t\t\t" + _CurrentPos.PrintCode(Game.Player.Character) + "," });
				}
			} else if (e.KeyCode == Keys.F5 && Game.IsControlPressed(GTA.Control.Sprint)) {
				SetPosition(_LADDER_SECTION);
			} else if (e.KeyCode == Keys.F10) {
				if (Game.IsControlPressed(GTA.Control.Sprint)) {
					CustomAudio.PlayBeep();
					return;
				}
				PlaneStuck.ToggleRunning();
				if (PlaneStuck.IsRunning) {
					_PlaneStuckStarted();
					_WriteLine("Punishment Enabled!");
				} else {
					_PlaneStuckEnded();
					_WriteLine("Punishment Disabled!");
				}
			} else if (e.KeyCode == Keys.F11) {
				if (Game.IsControlPressed(GTA.Control.Sprint)) {
					CustomAudio.ChangeVolume(-0.05f);
				} else {
					Checkpoints.LoadCheckpoint(this);
				}
			} else if (e.KeyCode == Keys.F12) {
				if (Game.IsControlPressed(GTA.Control.Sprint)) {
					CustomAudio.ChangeVolume(0.05f);
				} else {
					SetPosition(PlaneStuck.Project(GetCurrentView().Position).Grounded());
				}
			}
		}

		private Vector3 _SetPercentPosition(float percent) {
			var vec = PlaneStuck.PercentToPos(percent).Grounded();
			SetPosition(vec);
			return vec;
		}

		internal static void SetPositionRotation(PosRot posRot, bool fixCamera = false) {
			var actor = Game.Player.Character;
			SetPositionRotation(actor, posRot.Vector, posRot.Heading);
			if (fixCamera) {
				GameplayCamera.RelativeHeading = 0f;
				GameplayCamera.RelativePitch = 0f;
			}
		}

		internal static void SetPositionRotation(ISpatial actor, Vector3 pos, float heading) {
			actor.Position = pos;
			actor.Rotation = new Vector3(0, 0, heading);
		}

		public void SetPosition(Vector3 vec) {
			float heading = PlaneStuck.Plane.ToHeading();
			if (_playerCharacter.IsInVehicle()) {
				_SetPositionWithVehicle(vec, heading);
				return;
			}
			if (_IsFreeCam()) {
				vec.Z += 6f;
			}
			SetPositionRotation(GetCurrentView(), vec, heading);
		}

		private void _SetPositionWithVehicle(Vector3 vec, float heading) {
			Vehicle last = _playerCharacter.CurrentVehicle;
			SetVehiclePosition(last, vec, heading);
			last.Heading = heading;
		}

		public static void SetVehiclePosition(Vehicle veh, Vector3 vec, float heading = 0f) {
			float speed = veh.IsAircraft ? veh.Velocity.Length() : 0f;
			if (veh.IsAircraft) {
				if (veh.Position.Z > vec.Z) {
					vec.Z = veh.Position.Z;
				}
				veh.Throttle = 1f;
				veh.ThrottlePower = 1f;
			}
			veh.Position = vec;
			veh.Rotation = new Vector3(0, 0, heading);
			veh.Velocity = PlaneStuck.Plane.Normalized * speed;
		}

		private bool _isOnBike = false;
		private Model? _bikeModel = null;
		private Vehicle _bikeVehicle = null;



		private void _OnTick(object sender, EventArgs e) {
			_OnTick_BikeRiding();

			//if (_playerCharacter.IsDead && PlaneStuck.IsRunning) {
			if (_playerCharacter.IsDead) {
				_OnTick_Death();
				return;
			}

			_OnTick_BikeRidingPart2();

			Vector3 pos = _CurrentPos;
			_frames++;

			//Add Blips!
			_OnTick_Blips(pos);

			//PlaneStuck the player
			_OnTick_PlaneStuck();

			Checkpoints.OnTick();
		}

		private void _OnTick_PlaneStuck() {
			//PlaneStuck the player
			ISpatial curView = GetCurrentView();

			Vector3 oldPos = curView.Position;
			Vector3 playerProjection = PlaneStuck.Project(oldPos);

			if (PlaneStuck.IsRunning) {
				if (PlaneStuck.NextRun <= Game.GameTime) {
					PlaneStuck.Process(oldPos, playerProjection, _playerCharacter, _IsFreeCam());
				}
			}

			//PlaneStuck the HardHat
			_props.SetHardHatPos(_playerCharacter, playerProjection, _IsFreeCam());
		}

		private void _OnTick_Blips(Vector3 pos) {
			if (Blips.IsRunning) {
				if (_frames % 10 == 0) {
					Blips.Process(pos);
				}
			}
		}

		private void _OnTick_BikeRidingPart2() {
			if (_isOnBike && !_playerCharacter.IsOnBike) {
				_isOnBike = false;
			}
		}

		private void _OnTick_Death() {
			Vector3 safeCheckpoint = Checkpoints.GetRespawnPos();
			FastRespawn.RespawnCharacter(_playerCharacter, safeCheckpoint, _isOnBike ? _bikeModel : null);
			SetPosition(safeCheckpoint);
			_bikeVehicle?.Delete();
			//Alive and set now
			Checkpoints.TriggerRespawn();
		}

		private void _OnTick_BikeRiding() {
			if (!_isOnBike && _playerCharacter.IsOnBike) {
				_isOnBike = true;
				_bikeVehicle = _playerCharacter.CurrentVehicle;
				_bikeModel = _bikeVehicle.Model;
			}
		}

		private bool _IsFreeCam() {
			//return MapEditor.MapEditor.IsInFreecam;
			return false;
		}

		public ISpatial GetCurrentView() {
			return _IsFreeCam() ? (ISpatial)World.RenderingCamera : _playerCharacter;
		}

		private void _WriteLine(string msg) {
			Notify(msg);
		}

		public static void Notify(string msg) {
			Notification.Show(msg);
			//SHVDN.Log.Message(SHVDN.Log.Level.Warning, msg);
		}
	}
}
