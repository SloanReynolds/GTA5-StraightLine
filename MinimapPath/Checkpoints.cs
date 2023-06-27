using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
//using MapEditor;
//using static MapEditor.ObjectDatabase;

namespace MinimapPath {
	public enum PickupHash {
		Pistol = -105925489,
		CombatPistol = -1989692173,
		APPIstol = 996550793,
		MicroSMG = 496339155,
		SMG = 978070226,
		AssaultRifle = -214137936,
		CarbineRifle = -546236071,
		AdvancedRifle = -1296747938,
		SawnOffShotgun = -1766583645,
		PumpShotgun = -1456120371,
		AssaultShotgun = -1835415205,
		SniperRifle = -30788308,
		HeavySniper = 1765114797,
		MachineGun = -2050315855,
		CombatMachineGun = -1298986476,
		GrenadeLauncher = 779501861,
		RPG = 1295434569,
		Minigun = 792114228,
		Grenade = 1577485217,
		StickyBomb = 2081529176,
		Molotov = 768803961,
		PetrolCan = -962731009,
		SmokeGrenade = 483787975,
		Knife = 663586612,
		Bat = -2115084258,
		Hammer = 693539241,
		Crowbar = -2027042680,
		GolfClub = -1997886297,
		Nightstick = 1587637620,
		Parachute = 1735599485,
		Armour = 1274757841,
		Health = -1888453608,
		VehiclePistol = -1521817673,
		VehicleCombatPistol = -794112265,
		VehicleAPPistol = -863291131,
		VehicleMicroSMG = -1200951717,
		VehicleSMG = -864236261,
		VehicleSawnOffShotgun = 772217690,
		VehicleGrenade = -1491601256,
		VehicleMolotov = -2066319660,
		VehicleSmokeGrenade = 1705498857,
		VehicleStickyGrenade = 746606563,
		VehicleHealth = 160266735,
		VehicleArmour = 1125567497,
		MoneyCase = -831529621,
		MoneyBag = 545862290,
		MoneyMediumBag = 341217064,
		MoneyPaperBag = 1897726628,
		CrateUnfixed = 1852930709,
		Package = -2136239332,
		BulletAmmo = 1426343849,
		MissleAmmo = -107080240,
		Camera = -482507216,
		Snack = 483577702,
		Purse = 513448440,
		SecurityCase = -562499202,
		Money = -31919185,
		Wallet = 1575005502,
	}

	internal static class Checkpoints {
		private const float CHECKPOINT_SENSITIVITY = 0.001f;

		private static readonly List<CheckpointEvent> _checkpointEvents = new List<CheckpointEvent>() {
			new CheckpointEvent(0f, _SetCheckpoint),
			new CheckpointEvent(0.083f, _SetCheckpoint),
			new CheckpointEvent(0.1f, _SetCheckpoint),
			new CheckpointEvent(0.123f, _SetCheckpoint),
			new CheckpointEvent(0.15f, _SetCheckpoint),
			new CheckpointEvent(0.178f, _SetCheckpoint),
			new CheckpointEvent(0.195f, (chk) => {
				_SetCheckpoint(chk);
				_SpawnBridgeParachute();
			}, (chk) => {
				_GiveWeapon(WeaponHash.Parachute);
			}),
			new CheckpointEvent(0.202f, (chk) => { _SetCheckpoint(chk); _RemoveParachute(); }),
			new CheckpointEvent(0.207f, _SetCheckpoint),
			new CheckpointEvent(0.211f, (chk) => { _SetCheckpoint(chk); //Props.I.RestrictHatToPlayerHeight();
			}),
			new CheckpointEvent(0.248f, (chk) => { _SetCheckpoint(chk); //Props.I.UnrestrictHatToPlayerHeight(); 
			}),
			new CheckpointEvent(0.275f, _SetCheckpoint),
			new CheckpointEvent(0.29f, _SetCheckpoint),
			new CheckpointEvent(0.30f, _SetCheckpoint),
			new CheckpointEvent(0.317f, (chk) => { _SpawnKamikaze(); }),
			new CheckpointEvent(0.33f, _SetCheckpoint),
			new CheckpointEvent(0.365f, _SetCheckpoint),
			new CheckpointEvent(0.39f, _SetCheckpoint),
			new CheckpointEvent(0.42f, _SetCheckpoint),
			new CheckpointEvent(0.45f, _SetCheckpoint),
			new CheckpointEvent(0.48f, _SetCheckpoint),
			new CheckpointEvent(0.51f, _SetCheckpoint),
			new CheckpointEvent(0.54f, _SetCheckpoint),
			new CheckpointEvent(0.57f, _SetCheckpoint),
			new CheckpointEvent(0.60f, _SetCheckpoint),
			new CheckpointEvent(0.63f, _SetCheckpoint),
			new CheckpointEvent(0.66f, _SetCheckpoint),
			new CheckpointEvent(0.675f, _SetCheckpoint),
			new CheckpointEvent(0.679f, (chk) => { _SetCheckpoint(chk); _SpawnPickup(PickupHash.Parachute, new Vector3(590.5837f, 146.5652f, 157f)); }, (chk) => { _GiveWeapon(WeaponHash.Parachute); }),
			new CheckpointEvent(0.713f, (chk) => { _SetCheckpoint(chk); _RemoveParachute(); }),
			//new CheckpointEvent(0.72f, _SetCheckpoint),
			new CheckpointEvent(0.728f, _SetCheckpoint, (chk) => { _EnsureVehicleNear(VehicleHash.Akuma, new Vector3(703.2481f, -295.6843f, 59.17931f), -164.8244f, 1f); }),
			//new CheckpointEvent(0.745f, (chk) => { //Props.I.RestrictHatToPlayerHeight(); 
			//}),
			//new CheckpointEvent(0.75f, (chk) => { _SetCheckpoint(chk, 25f); }),
			new CheckpointEvent(0.755f, _SetCheckpoint),
			new CheckpointEvent(0.78f, _SetCheckpoint),
			new CheckpointEvent(0.804f, _SetCheckpoint),
			new CheckpointEvent(0.84f, _SetCheckpoint),
			new CheckpointEvent(0.8551f, _SetCheckpoint),
			new CheckpointEvent(0.87f, _SetCheckpoint),
			new CheckpointEvent(0.90f, _SetCheckpoint),
			new CheckpointEvent(0.93f, _SetCheckpoint),
			new CheckpointEvent(0.96f, _SetCheckpoint),
			new CheckpointEvent(0.984f, _SetCheckpoint),
			new CheckpointEvent(0.9925f, _SetCheckpoint),
			new CheckpointEvent(1f, (chk) => { _SetCheckpoint(chk); _StartFireworks(); _BeginHardHatDonning(); }),
		};

		private static void _BeginHardHatDonning() {
			Props.I.StartDonningHardHat();
		}

		private const string FIREWORK1 = "scr_indep_firework_starburst";
		private const string FIREWORK2 = "scr_indep_firework_fountain";
		private const string FIREWORK3 = "scr_indep_firework_shotburst";
		private const string FIREWORK4 = "scr_indep_firework_trailburst";
		private const int FIREWORK_DELAY = 90;
		private const int FIREWORK_VAR = 20;
		private static readonly string[] _fireworksNames = new string[] { FIREWORK1, FIREWORK2, FIREWORK3, FIREWORK4 };
		private static Random _random = new Random();
		private static bool _isFireworks = false;
		private static int _fireworksNext = 0;
		private static int _fireworksEnd = 0;
		private static ParticleEffectAsset _fireworksPFXA = new ParticleEffectAsset("scr_indep_fireworks");

		internal static void OnTick() {
			if (_isFireworks && _fireworksNext < Game.GameTime) {
				//See if we should end
				if (_fireworksEnd < Game.GameTime) {
					_isFireworks = false;
				}

				//Fire one off
				Vector3 pos = Vector3.RandomXYZ() * 20f + Game.Player.Character.Position;
				_SpawnFireworkAt(pos);

				//Set up next firework
				_fireworksNext = Game.GameTime + FIREWORK_DELAY + _random.Next(FIREWORK_VAR + 1);
			}
		}

		private static void _StartFireworks() {
			if (!_fireworksPFXA.IsLoaded) {
				_fireworksPFXA.Request();
			}

			_isFireworks = true;
			_fireworksNext = Game.GameTime + FIREWORK_DELAY + _random.Next(FIREWORK_VAR + 1);
			_fireworksEnd = Game.GameTime + 10000;
		}

		private static void _SpawnFireworkAt(Vector3 pos) {
			Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_COLOUR, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 1f), Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 1f), Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 1f));
			World.CreateParticleEffectNonLooped(_fireworksPFXA, _fireworksNames[_random.Next(_fireworksNames.Length)], pos, Vector3.RandomXY());
		}

		private static void _EnsureVehicleNear(VehicleHash model, Vector3 vector3, float heading, float radius) {
			if (Game.Player.Character.IsInVehicle() && Game.Player.Character.CurrentVehicle.Model.Hash == (int)VehicleHash.CarbonRS) {
				//Do nothing
				return;
			}

			var vehicles = World.GetNearbyVehicles(vector3, 1f, new Model[] { model });
			if (vehicles.Length == 0) {
				MapGen.AddVehicle(World.CreateVehicle(model, vector3, heading));
			}
		}

		internal static Vector3 GetRespawnPos() {
			return _currentCheckpointEvent.Position.Grounded(_currentCheckpointEvent.SpecifiedHeight);
		}

		internal static void LoadCheckpoint(MinimapPath script) {
			script.SetPosition(GetRespawnPos());
		}

		public static void SpawnKamikaze() {
			_SpawnKamikaze();
		}

		private static Vehicle _kami = null;

		private static void _SpawnKamikaze() {
			Vector3 pos = Game.Player.Character.Position;
			Model model = new Model(VehicleHash.Luxor);
			var current = World.CreateVehicle(model, new Vector3(pos.X, pos.Y, pos.Z + 120f), PlaneStuck.Plane.ToHeading());
			current.IsEngineRunning = true;
			if (current.IsAircraft) {
				current.Throttle = 1f;
				current.ThrottlePower = 1f;
				current.Yaw(-30f);
				current.Pitch(15f);
				current.Roll(-15f);
				current.Velocity = current.ForwardVector.Normalized * 20f;
			}
			_kami = current;
		}

		private static void _RemoveParachute() {
			Function.Call(Hash.REMOVE_WEAPON_FROM_PED, Game.Player.Character, WeaponHash.Parachute);
		}

		private static void _SpawnBridgeParachute() {
			_SpawnPickup(PickupHash.Parachute, 0.197f);
		}

		private static void _SpawnPickup(PickupHash pickupHash, float percent) {
			Vector3 pos = PlaneStuck.PercentToPos(percent).Grounded();
			pos.Z += 1f;
			_SpawnPickup(pickupHash, pos);
		}

		private static void _SpawnPickup(PickupHash pickupHash, Vector3 pos) {
			Model model = new Model((int)pickupHash);
			_ = Function.Call<int>(Hash.CREATE_PICKUP_ROTATE, model.Hash, pos.X, pos.Y,
					pos.Z, 0, 0, 0, 0b1000001000, 100, 0, false, 0);
		}

		private static float _currentMaxPercentsitivity = 0f;
		private static CheckpointEvent _currentCheckpointEvent = _checkpointEvents[0];

		internal static void Reset() {
			Game.Player.Character.Style[PedPropType.Hats].SetVariation(0);
			_currentCheckpointEvent = _checkpointEvents[0];
			_currentMaxPercentsitivity = 0f;
			foreach (var chkp in _checkpointEvents) {
				chkp.Reset();
			}
			Props.I.Reset();
		}

		internal static void TriggerRespawn() => _currentCheckpointEvent.TriggerRespawn();
		internal static void UpdateCheckpoint(float percent) {
			//Sensitivity check, so we don't trigger a Linq query for every little jiggle.
			percent = (float)Math.Floor(percent / CHECKPOINT_SENSITIVITY) * CHECKPOINT_SENSITIVITY;
			if (percent <= _currentMaxPercentsitivity) {
				return;
			}
			_currentMaxPercentsitivity = percent;
			CheckpointEvent max = _checkpointEvents.Where(e => e.Distance <= percent).OrderByDescending(e => e.Distance).First();
			if (max != _currentCheckpointEvent) {
				_currentCheckpointEvent = max;
				_currentCheckpointEvent.TriggerAttained();
			}
		}

		private static void _SetCheckpoint(CheckpointEvent checkpointEvent) {
			_SetCheckpoint(checkpointEvent, 0f);
		}

		private static void _SetCheckpoint(CheckpointEvent checkpointEvent, float specifiedHeight) {
			_currentCheckpointEvent = checkpointEvent;
			if (specifiedHeight != 0)
				checkpointEvent.SpecifiedHeight = specifiedHeight;
			MinimapPath.Notify($"Checkpoint! {_currentCheckpointEvent.Distance.PercentDisplay()}");
		}

		private static void _GiveWeapon(WeaponHash hash) => Game.Player.Character.Weapons.Give(hash, 1, false, false);

		internal static void CleanUp() {
			_kami?.Delete();
		}
	}
}
