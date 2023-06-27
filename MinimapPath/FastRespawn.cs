using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace MinimapPath {
	public static class FastRespawn {
		public static void RespawnCharacter(Ped player, Vector3 safeCheckpoint, Model? _bikeModel) {
			//Delete and respawn one?
			Vehicle current = null;
			Model? model;
			float speed = 0;
			if (_bikeModel.HasValue) {
				model = _bikeModel.Value;
			} else {
				current = player.CurrentVehicle;
				model = current?.Model;
				speed = current == null ? 0f : current.Velocity.Length();
			}
			//Something about respawning seems to delete aircraft, and also blows it up after a 2nd quick death..?
			Go();

			current?.Delete();
			current = null;

			if (current == null && model.HasValue) {
				current = World.CreateVehicle(model.Value, new Vector3(safeCheckpoint.X, safeCheckpoint.Y, safeCheckpoint.Z + 20f), 0);

				current.IsEngineRunning = true;
				if (current.IsAircraft) {
					current.Throttle = 1f;
					current.ThrottlePower = 1f;
					if (current.IsHelicopter) {
						current.HeliBladesSpeed = 1f;
					}
					if (current.IsPlane) {
						current.Velocity = PlaneStuck.Plane.Normalized * speed;
					}
				}

				player.SetIntoVehicle(current, VehicleSeat.Driver);
				MinimapPath.SetVehiclePosition(current, safeCheckpoint);
			}
		}

		public static void Go() {
			Screen.FadeOut(300);
			Script.Wait(750);

			Ped ped = Function.Call<Ped>((Hash)0xEF29A16337FACADB, (InputArgument[])(object)new InputArgument[4] {
				Game.Player.Character,
				false,
				false,
				false
			});

			ped.Heading = Game.Player.Character.Heading;
			ped.IsPersistent = true;
			ped.Kill();

			//TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME
			Function.Call((Hash)0x9DC711BC69C548DF, "respawn_controller");
			//IGNORE_NEXT_RESTART
			Function.Call((Hash)0x21FFB63D8C615361, true);
			//PAUSE_DEATH_ARREST_RESTART
			Function.Call((Hash)0x2C2B3493FBF51C71, true);
			//TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME
			Function.Call((Hash)0x9DC711BC69C548DF, "respawn_controller");
			Game.TimeScale = 1f;
			//ANIMPOSTFX_STOP_ALL
			Function.Call((Hash)0xB4EDDC19532BFB85);
			//NETWORK_REQUEST_CONTROL_OF_ENTITY
			Function.Call((Hash)0xB69317BF5E782347, Game.Player.Character);
			//NETWORK_RESURRECT_LOCAL_PLAYER
			Function.Call((Hash)0xEA23C49EAA83ACFB, 
				Game.Player.Character.Position.X,
				Game.Player.Character.Position.Y,
				Game.Player.Character.Position.Z,
				Game.Player.Character.Heading,
				false,
				false
			);
			//FORCE_GAME_STATE_PLAYING
			Function.Call((Hash)0xC0AA53F866B3134D, Game.Player);
			//DISPLAY_HUD
			Function.Call((Hash)(0xA6294919E56FF02A), true);
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.Character.IsCollisionEnabled = true;

			Script.Wait(750);
			Screen.FadeIn(300);
		}
	}
}
