using System;
using GTA;
using GTA.Math;

namespace MinimapPath {
	class Props {
		private static Props _instance;
		public static Props I => _instance;

		private Prop _hardHat = null;
		private Prop _rifle = null;
		private float _freqX = 1f;
		private float _freqZ = 2f / 3f;
		private float _freqSpeed = 1f / 6f;

		private int _startDonning = 0;
		private bool _isDonning = false;
		private int _startedDonning = 0;

		private Random _random = new Random();

		public Vector3 HardHatPos => _hardHat.Position;

		public Props() {
			_instance = this;
		}

		internal void SetHardHatPos(Ped playerCharacter, Vector3 pos, bool isFreeCam) {
			if (!PlaneStuck.IsRunning)
				return;

			if (_isDonning && _startDonning < Game.GameTime) {
				_SetHardHadPos_Donning(playerCharacter);
				return;
			}

			pos += PlaneStuck.Plane.Normalized * (isFreeCam ? 10f : playerCharacter.IsInVehicle() ? 6f : 2f);

			float circleX = (float)Math.Sin((_freqX * _freqSpeed * Game.GameTime + 90f) * Math.PI / 180) * (isFreeCam ? 0.5f : 0.18f);
			float circleY = (float)Math.Cos((_freqX * _freqSpeed * Game.GameTime + 90f) * Math.PI / 180) * (isFreeCam ? 0.5f : 0.18f);
			float pulseHeight = (float)Math.Sin((_freqZ * _freqSpeed * Game.GameTime) * Math.PI / 180) * (isFreeCam ? 1f : 0.3f) + 1.4f;

			float groundHeight;
			if (_restrictedToPlayerHeight) {
				float playerGround = MinimapPath.I.GetCurrentView().Position.Z;
				groundHeight = playerGround;
				int count = 0;
				while (World.GetGroundHeight(new Vector3(pos.X, pos.Y, groundHeight)) == 0) {
					groundHeight += 0.005f;
					count++;
					if (count == 1000) {
						break;
					}
				}
			} else {
				groundHeight = World.GetGroundHeight(new Vector2(pos.X, pos.Y)) + 0.6f;
			}

			pos.X += circleX;
			pos.Y += circleY;
			pos.Z = groundHeight + pulseHeight;

			_HardHat(pos);
			_Rifle(pos, playerCharacter);
		}

		private const float DON_SPEED_MIN = 1f / 145f;
		private const float DON_SPEED_FAC = 1f / 100f;
		private bool _isDonned = false;
		private void _SetHardHadPos_Donning(Ped playerCharacter) {
			if (_isDonned) {
				_HardHat(playerCharacter.Position - new Vector3(0, 0, 10f));
				_Rifle(playerCharacter.Position - new Vector3(0, 0, 10f), playerCharacter);
				return;
			}

			Vector3 headPos = playerCharacter.Position;
			headPos.Z = headPos.Z + 0.7f;
			Vector3 travel = headPos - _hardHat.Position;
			float speed = travel.Length() * DON_SPEED_FAC;
			float minSpeed = DON_SPEED_MIN * (1 + (Game.GameTime - _startedDonning) / 1000f);
			if (speed < minSpeed)
				speed = minSpeed;

			Vector3 newPos = _hardHat.Position + travel.Normalized * speed;

			_HardHat(newPos);
			_Rifle(newPos, playerCharacter);

			if ((newPos - headPos).Length() < minSpeed * 3) {
				_isDonned = true;
				_DonHardHat();
				Game.Player.Character.Weapons.Give(WeaponHash.AssaultRifle, 10000, true, true);

				PlaneStuck.ReportComplete();
			}
		}

		private static void _DonHardHat() {
			Game.Player.Character.Style[PedPropType.Hats].SetVariation(8);
		}

		private void _HardHat(Vector3 pos) {
			if (_hardHat == null) {
				_hardHat = _CreateProp(-537490919, pos);
				_hardHat.IsCollisionEnabled = false;
			} else {
				_hardHat.Position = pos;
			}
		}

		private void _Rifle(Vector3 pos, Ped playerCharacter) {
			Vector3 playerDiff = playerCharacter.Position - pos;
			pos = new Vector3(pos.X, pos.Y, pos.Z - 0.15f);
			if (_rifle == null) {
				_rifle = _CreateProp(273925117, pos);
				_rifle.IsCollisionEnabled = false;
			} else {
				_rifle.Position = pos;
				_rifle.Rotation = new Vector3(_rifle.Rotation.X, playerDiff.ToPitch(-100f), playerDiff.ToHeading(0f));
			}
		}

		internal void DeleteAll() {
			_hardHat.Delete();
			_rifle.Delete();

			_hardHat = null;
			_rifle = null;
		}

		private Prop _CreateProp(int hash, Vector3 pos, bool dynamic = false) {
			Model model = new Model(hash);
			model.Request(10000);
			Prop prop = World.CreateProp(model, pos, dynamic, true);
			prop.Position = pos;
			prop.LodDistance = 3000;
			if (!dynamic) {
				prop.IsPositionFrozen = true;
				prop.IsCollisionEnabled = false;
			}
			return prop;
		}

		private int _shotCount = 0;
		private int _booshCount = 0;
		ParticleEffectAsset _coreAsset = new ParticleEffectAsset("core");

		internal void FireAtPlayer(Ped player, float damage) {
			if (_isDonning || _isDonned || _rifle == null || _hardHat == null) {
				return;
			}
			Vector3 rifleTip = _rifle.Position + (player.Position - _rifle.Position).Normalized * 0.59f + Vector3.WorldUp * 0.122f;
			Vector3 chestPos = (player.AbovePosition - player.Position) / 4 + player.Position;
			Vector3 rifleDiff = rifleTip - chestPos;
			int healthBefore = player.Health;
			World.ShootBullet(chestPos + rifleDiff.Normalized * 0.1f, chestPos, null, WeaponHash.Revolver, (int)damage);
			if (healthBefore == player.Health) { //If the gun misses...
				player.ApplyDamage((int)damage);
			}
			if (healthBefore == player.Health) { //If the apply damage doesn't work?
				player.Health = player.Health - (int)damage;
				player.IsInvincible = false; //????
			}

			double rndNum = _random.NextDouble();
			if (_shotCount > 3 && rndNum <= 0.1f) {
				if (_booshCount == 10) {
					CustomAudio.PlaySpecialBoosh();
					_booshCount = 0;
				} else {
					CustomAudio.PlayBoosh();
					_booshCount++;
				}
				_shotCount = 0;
			} else {
				CustomAudio.PlayShot();
				_shotCount++;
			}
			//player.ApplyForce(rifleDiff.Normalized * 100f);
			if (!_coreAsset.IsLoaded) {
				_coreAsset.Request();
			}

			World.CreateParticleEffectNonLooped(_coreAsset, "muz_assault_rifle", rifleTip);
		}

		internal void StartDonningHardHat() {
			_isDonning = true;
			_startDonning = Game.GameTime + 2500;
			_startedDonning = Game.GameTime;
		}

		private bool _restrictedToPlayerHeight = false;
		internal void RestrictHatToPlayerHeight() {
			_restrictedToPlayerHeight = true;
		}
		internal void UnrestrictHatToPlayerHeight() {
			_restrictedToPlayerHeight = false;
		}

		internal void Reset() {
			_isDonning = false;
			_isDonned = false;
		}
	}
}
