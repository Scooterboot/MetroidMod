using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items
{
	public class MGlobalItem : GlobalItem
	{
		public int addonSlotType = -1;

		public int missileSlotType = -1;
		public int statMissiles = 5;
		public int maxMissiles = 5;
		
		public int numSeekerTargets = 0;
		public int[] seekerTarget = new int[5];
		public int seekerCharge = 0;
		public static int seekerMaxCharge = 25;

		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}
		public override bool CloneNewInstances
		{
			get
			{
				return true;
			}
		}
	}
	public class grab : GlobalItem
	{
		public override void GrabRange(Terraria.Item item, Player player, ref int grabRange)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			grabRange += (int)(mp.statCharge * 1.6f);
		}
	}
	/*public class armortrail : GlobalItem
	{
		public override void ArmorSetShadows(Player player, string set)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			if (mp.tweak > 4)
			{
				longTrail = true;
			}
		}
	}*/
	public class armorcolor : GlobalItem
	{
		public override void DrawArmorColor(EquipType type, int slot, Player P, float shadow, ref Color color,ref int glowMask, ref Color glowMaskColor)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			bool pseudoScrew = (mp.statCharge >= MPlayer.maxCharge && mp.somersault && mp.SMoveEffect <= 0);
			if(mp.hyperColors > 0 || mp.speedBoosting || mp.shineActive || (pseudoScrew && mp.psuedoScrewFlash >= 3) || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
			{
				if(mp.hyperColors > 0)
				{
					color = new Color(mp.r, mp.g, mp.b, 255);
				}
				else if(pseudoScrew && mp.psuedoScrewFlash >= 3)
				{
					color = mp.chargeColor;
				}
				else if(mp.shineActive || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
				{
					color = new Color(255, 216, 0);
				}
				else if(mp.speedBoosting)
				{
					color = new Color(0, 170, 255);
				}
				mp.morphColor = color;

				int dustType = 212;
				if(P.head <= 0 || P.body <= 0 || P.legs <= 0)
				{
					int dust = Dust.NewDust(new Vector2(P.position.X - P.velocity.X, P.position.Y - 2f - P.velocity.Y), P.width, P.height, dustType, -P.velocity.X * 0.25f, -P.velocity.Y * 0.25f, 100, color, 1.0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}
		}
	}
}
