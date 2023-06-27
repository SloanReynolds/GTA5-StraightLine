using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace MinimapPath {
	public class MapGen : Script {
		internal static List<Prop> Props = new List<Prop>();
		internal static List<Vehicle> Vehicles = new List<Vehicle>();

		private const int LOD_DISTANCE = 3000;
		private static bool _initialized = false;

		public static bool IsLadderLoaded { get; private set; } = false;

		internal static void Populate() {
			if (!_initialized) {
				//Script.Yield();
				Function.Call(Hash.REQUEST_ANIM_DICT, "dt1_15");
				Function.Call(Hash.REQUEST_ANIM_SET, "dt1_15");
				Function.Call(Hash.REQUEST_CLIP_SET, "dt1_15");

				/* PROPS */
				_CreateProps1();
				_CreateProps2();
				_CreateProps3();
				_CreateProps4();
				_CreateProps5();
				_CreateProps6();

				/* VEHICLES */
				Vehicles.Add(World.CreateVehicle(new Model(1131912276), new Vector3(-740.5942f, 5225.297f, 98.66917f), 116.9015f));
				Vehicles.Add(World.CreateVehicle(new Model(55628203), new Vector3(705.6704f, -295.4153f, 58.66553f), -164.5054f));
				Vehicles.Add(World.CreateVehicle(new Model(VehicleHash.Akuma), new Vector3(703.5488f, -295.9598f, 58.69348f), -164.8244f));

				/* PEDS */


				/* PICKUPS */

				_initialized = true;
			}
		}

		private static void _CreateProps6() {
			Props.Add(_CreateProp(1329706303, new Vector3(990.1339f, -1366.564f, 22.11f), new Vector3(7.257359f, -54.81586f, -32.54821f), false));
			Props.Add(_CreateProp(1434516869, new Vector3(1001.507f, -1419.704f, 28.91699f), new Vector3(8.590446f, 1.609834f, 89.50897f), false));
			Props.Add(_CreateProp(-1366478936, new Vector3(999.545f, -1420.073f, 29.63619f), new Vector3(4.774381f, -0.0002605047f, -87.8371f), false));
			Props.Add(_CreateProp(1120812170, new Vector3(1003.747f, -1420.025f, 29.33318f), new Vector3(25.274f, 6.462345f, 89.08973f), false));
			Props.Add(_CreateProp(-1101201844, new Vector3(1124.73f, -1876.91f, 35.85f), new Vector3(0f, 0f, -157.2449f), false));
			Props.Add(_CreateProp(1977786498, new Vector3(1346.678f, -2726.677f, 1.283121f), new Vector3(0f, 0f, 15.00001f), false));
			Props.Add(_CreateProp(-415369673, new Vector3(1346.666f, -2726.533f, 1.309603f), new Vector3(7.388947E-08f, 7.044289E-08f, -163.9991f), false));
		}

		private static void _CreateProps5() {
			Props.Add(_CreateProp(-1366478936, new Vector3(1012.597f, -1454.455f, 35.47291f), new Vector3(1.71786f, 19.66313f, -92.20406f), false));
			Props.Add(_CreateProp(-1681802986, new Vector3(1012.41f, -1455.74f, 38.89f), new Vector3(-5.97114E-13f, -5.008956E-06f, -89.99999f), false));
			Props.Add(_CreateProp(-523732689, new Vector3(945.4359f, -1198.335f, 25.05789f), new Vector3(0f, 0f, 2f), false));
			Props.Add(_CreateProp(1903780998, new Vector3(953.76f, -1234.7f, 28.75f), new Vector3(2.035555E-13f, -5.008956E-06f, -55f), false));
			Props.Add(_CreateProp(1903780998, new Vector3(953.76f, -1234.7f, 33.75f), new Vector3(2.035555E-13f, -5.008956E-06f, -55f), false));
			Props.Add(_CreateProp(1903780998, new Vector3(953.76f, -1234.7f, 38.75f), new Vector3(2.035555E-13f, -5.008956E-06f, -55f), false));
		}

		private static void _TryCreateLadder() {
			try {
				Props.Add(_CreateProp(-1101201844, new Vector3(880.1099f, -939.0206f, 32.25787f), new Vector3(0f, 0f, 0f), false));

				IsLadderLoaded = true;
			} catch { 
				//Do nothing lol
			}
		}

		private static void _CreateProps4() {
			Props.Add(_CreateProp(656278478, new Vector3(582.3714f, 159.4532f, 154.4733f), new Vector3(-40.99901f, 1.963235f, -3.79702f), false));
			Props.Add(_CreateProp(-524606703, new Vector3(583.537f, 179.2202f, 138.4572f), new Vector3(-84.98721f, 0.001447936f, -8.910907f), false));
			Props.Add(_CreateProp(1366334172, new Vector3(690.7871f, -236.9976f, 52.17543f), new Vector3(-20.74859f, -18.97857f, -20.65378f), false));
			Props.Add(_CreateProp(-2041628332, new Vector3(691.2216f, -235.7524f, 51.05119f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(1471437843, new Vector3(691.6654f, -234.938f, 51.27115f), new Vector3(-4.283011f, -23.62068f, 11.73048f), false));
			Props.Add(_CreateProp(-2041628332, new Vector3(689.9026f, -238.4856f, 52.57304f), new Vector3(-17.03214f, -179.0008f, -32.7084f), false));
			Props.Add(_CreateProp(-1666677539, new Vector3(730.2859f, -386.3332f, 41.80009f), new Vector3(2.667477f, -0.488658f, -74.74021f), false));
			Props.Add(_CreateProp(929870599, new Vector3(794.7355f, -614.1493f, 31.63266f), new Vector3(-35.64454f, -14.37224f, 14.82456f), false));

			//LADDER SHENANIGANS!
			_TryCreateLadder();


			Props.Add(_CreateProp(1903780998, new Vector3(882.2908f, -951.17f, 41.8f), new Vector3(0f, 0f, 0f), false));
		}

		private static void _CreateProps3() {
			Props.Add(_CreateProp(-882459311, new Vector3(584.3796f, 194.4109f, 133.1323f), new Vector3(2.035555E-13f, -5.008956E-06f, 17.5f), false));
			Props.Add(_CreateProp(656278478, new Vector3(582.4581f, 200.5115f, 145.13f), new Vector3(1.00179E-05f, 5.008956E-06f, 17.5f), false));
			Props.Add(_CreateProp(656278478, new Vector3(574.6472f, 225.3118f, 145.13f), new Vector3(1.001791E-05f, 5.008956E-06f, 17.5f), false));
			Props.Add(_CreateProp(-643802684, new Vector3(563.52f, 260.58f, 133.15f), new Vector3(3.053332E-13f, -5.008956E-06f, -162.5f), false));
			Props.Add(_CreateProp(-643802684, new Vector3(560.23f, 271.03f, 121.14f), new Vector3(-1.017777E-13f, 5.008957E-06f, -162.5f), false));
			Props.Add(_CreateProp(656278478, new Vector3(560.24f, 271.02f, 121.14f), new Vector3(-1.017777E-13f, 5.008956E-06f, 17.5f), false));
			Props.Add(_CreateProp(-2025598867, new Vector3(549.73f, 304.4f, 115.26f), new Vector3(-1.017777E-13f, 5.008957E-06f, -162.5f), false));
			//Props.Add(createProp(1531047580, new Vector3(561.9702f, 265.6421f, 129.1361f), new Vector3(51.49492f, -38.94016f, 69.70879f), false));
			Props.Add(_CreateProp(100630055, new Vector3(563.5657f, 260.3863f, 134.8768f), new Vector3(90f, 17.5f, 0f), false));
			Props.Add(_CreateProp(-890658845, new Vector3(550.2527f, 302.871429f, 118.812981f), new Vector3(42.49871f, 5.334402f, 12.43706f), false));
		}

		private static void _CreateProps2() {
			//Props.Add(createProp(-259572998, new Vector3(565.488f, 161.4115f, 188.4941f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(700605237, new Vector3(-528.751f, 4428.845f, 28.84951f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(-297314236, new Vector3(-533.7551f, 4414.294f, 29.40107f), new Vector3(4.679575f, 39.69666f, -135.9217f), false));
			Props.Add(_CreateProp(1943138900, new Vector3(-533.7174f, 4409.091f, 30.21427f), new Vector3(-11.7021f, -10.51312f, -54.71703f), false));
			Props.Add(_CreateProp(1891144592, new Vector3(-517.5532f, 4377.485f, 59.40654f), new Vector3(-22.59246f, -62.7655f, -47.21919f), false));
			Props.Add(_CreateProp(-136564233, new Vector3(-516.5166f, 4376.651f, 60.6651f), new Vector3(56.68222f, -7.552982f, -35.45392f), false));
			Props.Add(_CreateProp(-890658845, new Vector3(-516.7963f, 4375.796f, 62.61176f), new Vector3(13.11714f, -18.54223f, -154.1527f), false));
		}

		private static void _CreateProps1() {
			Props.Add(_CreateProp(1993764676, new Vector3(-815.6598f, 5486.459f, 25.14642f), new Vector3(6.897256f, -61.09258f, -18.52911f), false));
			Props.Add(_CreateProp(-1692750285, new Vector3(-787.9356f, 5394.26f, 34.53205f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(-1395207765, new Vector3(-786.6092f, 5393.831f, 35.49192f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(-593160806, new Vector3(-783.9549f, 5377.61f, 38.60539f), new Vector3(4.67443f, 56.69639f, 90.5407f), false));
			Props.Add(_CreateProp(-1321831897, new Vector3(-584.8761f, 4624.725f, 149.2256f), new Vector3(0f, 0f, 37.2421f), false));
			Props.Add(_CreateProp(-1321831897, new Vector3(-581.7717f, 4607.117f, 133.6075f), new Vector3(-5.403408f, 172.3618f, -6.51535f), false));
			Props.Add(_CreateProp(160789653, new Vector3(-527.1695f, 4428.162f, 28.96663f), new Vector3(11.41294f, -132.0605f, 154.2678f), false));
			Props.Add(_CreateProp(-593160806, new Vector3(-531.5619f, 4420.677f, 30.31002f), new Vector3(46.62344f, 169.0397f, -108.9865f), false));
			Props.Add(_CreateProp(700605237, new Vector3(-530.0791f, 4423.573f, 29.33241f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(1065258673, new Vector3(-528.6346f, 4422.52f, 29.66764f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(773471646, new Vector3(-529.3013f, 4423.811f, 27.72148f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(773471646, new Vector3(-534.2947f, 4417.584f, 28.41531f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(69661806, new Vector3(-528.8597f, 4395.037f, 31.88087f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(390860802, new Vector3(-524.3647f, 4399.276f, 30.24688f), new Vector3(-1.387382E-06f, 27.05626f, -3.183071f), false));
			Props.Add(_CreateProp(773471646, new Vector3(-526.1513f, 4388.151f, 43.04073f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(-1215378248, new Vector3(-523.8635f, 4386.328f, 44.72789f), new Vector3(9.230833f, -0.3224833f, 0.05173087f), false));
			Props.Add(_CreateProp(2042668880, new Vector3(-521.346f, 4384.252f, 45.85239f), new Vector3(0f, 0f, 0f), false));
			Props.Add(_CreateProp(-1053433850, new Vector3(-520.3372f, 4382.188f, 49.51622f), new Vector3(2.086181f, -179.0817f, 170.2793f), false));
			Props.Add(_CreateProp(-1321831897, new Vector3(-517.8837f, 4379.343f, 50.74379f), new Vector3(-3.599503f, 171.4289f, -6.688209f), false));
			Props.Add(_CreateProp(-1580339749, new Vector3(-521.6583f, 4382.547f, 53.23592f), new Vector3(17.35186f, 6.716324f, 59.13979f), false));
			Props.Add(_CreateProp(-1580339749, new Vector3(-520.2507f, 4380.345f, 56.58185f), new Vector3(-15.00986f, -5.386103f, 45.60232f), false));
		}

		internal static void AddVehicle(Vehicle vehicle) {
			Vehicles.Add(vehicle);
		}

		internal static void CleanUp() {
			foreach (var prop in Props) {
				prop?.Delete();
			}

			foreach (var vehicle in Vehicles) {
				vehicle?.Delete();
			}

			Props = new List<Prop>();
			Vehicles = new List<Vehicle>();

			_initialized = false;
		}

		private static Prop _CreateProp(int hash, Vector3 pos, Vector3 rot, bool dynamic) {
			Model model = new Model(hash);
			model.Request(10000);
			Prop prop = World.CreateProp(model, pos, rot, dynamic, false);
			prop.Position = pos;
			prop.LodDistance = LOD_DISTANCE;
			if (!dynamic)
				prop.IsPositionFrozen = true;
			return prop;
		}
	}
}