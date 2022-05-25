using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Common.GlobalItems
{
	public class MGlobalItem : GlobalItem
	{
		public AddonType AddonType = AddonType.None;
		public int statMissiles = 5;
		public int maxMissiles = 5;

		public int numSeekerTargets = 0;
		public int[] seekerTarget = new int[5];
		public int seekerCharge = 0;
		public static int seekerMaxCharge = 25;

		public Texture2D itemTexture;

		public override bool InstancePerEntity => true;
		protected override bool CloneNewInstances => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			MGlobalItem other = (MGlobalItem)MemberwiseClone();
			other.maxMissiles = maxMissiles;
			other.statMissiles = statMissiles;

			return other;
		}

		public override bool CanUseItem(Item item, Player player)
		{
			return player.GetModPlayer<MPlayer>().VisorInUse != SuitAddonLoader.GetAddon<Content.SuitAddons.ScanVisor>().Type && player.GetModPlayer<MPlayer>().VisorInUse != SuitAddonLoader.GetAddon<Content.SuitAddons.XRayScope>().Type;
		}
	}
	public class Grab : GlobalItem
	{
		public override void GrabRange(Item item, Player player, ref int grabRange)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			grabRange += (int)(mp.statCharge * 1.6f);
		}
	}
	public class ArmorColor : GlobalItem
	{
		public override void DrawArmorColor(EquipType type, int slot, Player P, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			bool pseudoScrew = (mp.statCharge >= MPlayer.maxCharge && mp.somersault);
			if (mp.hyperColors > 0 || mp.speedBoosting || mp.shineActive || (pseudoScrew && mp.psuedoScrewFlash >= 3) || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
			{
				if (mp.hyperColors > 0)
				{
					color = P.GetImmuneAlphaPure(new Color(mp.r, mp.g, mp.b, 255), shadow);
				}
				else if (pseudoScrew && mp.psuedoScrewFlash >= 3)
				{
					color = P.GetImmuneAlphaPure(mp.chargeColor, shadow);
				}
				else if (mp.shineActive || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
				{
					color = P.GetImmuneAlphaPure(new Color(255, 216, 0), shadow);
				}
				else if (mp.speedBoosting)
				{
					color = P.GetImmuneAlphaPure(new Color(0, 200, 255), shadow);
				}
				mp.morphColor = color;

				int dustType = 212;
				if (P.head <= 0 || P.body <= 0 || P.legs <= 0)
				{
					int dust = Dust.NewDust(new Vector2(P.position.X - P.velocity.X, P.position.Y - 2f - P.velocity.Y), P.width, P.height, dustType, -P.velocity.X * 0.25f, -P.velocity.Y * 0.25f, 100, color, 1.0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}
		}
	}
	public class ReservePickup : GlobalItem
	{
		public override bool OnPickup(Item item, Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane)
			{
				if (mp.reserveHearts < mp.reserveTanks && player.statLife >= player.statLifeMax2)
				{
					mp.reserveHearts++;
				}
			}
			return base.OnPickup(item, player);
		}
	}
}
