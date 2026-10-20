using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Weapons.ArmCannon
{
	public class ArmCannon : ModItem, IArmCannon
	{
		private Item[] beamAddons;
		public Item[] BeamAddons
		{
			get {
				if (beamAddons == null)
				{
					beamAddons = new Item[6];
					for (int i = 0; i < beamAddons.Length; ++i)
					{
						beamAddons[i] = new Item();
						beamAddons[i].TurnToAir();
					}
				}

				return beamAddons;
			}
			set { beamAddons = value; }
		}

		private Item[] chargeHotswap;
		public Item[] ChargeBeamHotswap
		{
			get {
				if (chargeHotswap == null)
				{
					chargeHotswap = new Item[MetroidMod.beamChangeSlotAmount];
					for (int i = 0; i < chargeHotswap.Length; ++i)
					{
						chargeHotswap[i] = new Item();
						chargeHotswap[i].TurnToAir();
					}
				}
				return chargeHotswap;
			}
			set { chargeHotswap = value; }
		}

		private Item[] missileAddons;
		public Item[] MissileAddons
		{
			get {
				if (missileAddons == null)
				{
					missileAddons = new Item[3];
					for (int i = 0; i < missileAddons.Length; ++i)
					{
						missileAddons[i] = new Item();
						missileAddons[i].TurnToAir();
					}
				}
				return missileAddons;
			}
			set { missileAddons = value; }
		}

		private Item[] missileChargeHotswap;
		public Item[] MissileChargeHotswap
		{
			get {
				if (missileChargeHotswap == null)
				{
					missileChargeHotswap = new Item[MetroidMod.beamChangeSlotAmount];
					for (int i = 0; i < missileChargeHotswap.Length; ++i)
					{
						missileChargeHotswap[i] = new Item();
						missileChargeHotswap[i].TurnToAir();
					}
				}
				return missileChargeHotswap;
			}
			set { missileChargeHotswap = value; }
		}

		/// <summary>
		/// Arm Cannon Mode. False for Beams, True for Missiles.
		/// </summary>
		public bool Mode;

		public IArmCannon.ArmCannonUIConfig UIConfiguration => new() { };


	}
}
