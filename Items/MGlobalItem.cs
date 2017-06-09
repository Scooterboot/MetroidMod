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
  /*  public class armortrail : GlobalItem
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
            if(mp.hyperColors > 0 || mp.speedBoosting || mp.shineDirection != 0)
			{
				if(mp.hyperColors > 0)
				{
					color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
				else if(mp.shineDirection != 0)
				{
					color = new Color(255, 216, 0);
				}
				else if(mp.speedBoosting)
				{
					color = Color.DeepSkyBlue;
				}

				int dustType = 212;
				/*if(P.head > 0)
				{
					edi.dyeHead = 31;
				}
				else
				{*/
					int dust = Dust.NewDust(new Vector2(P.position.X - P.velocity.X, P.position.Y - 2f - P.velocity.Y), P.width, P.height, dustType, -P.velocity.X * 0.25f, -P.velocity.Y * 0.25f, 100, color, 1.0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
			/*	}
				if(P.body > 0)
				{
					edi.dyeBody = 31;
				}
				else
				{
					int dust = Dust.NewDust(new Vector2(P.position.X - P.velocity.X, P.position.Y - 2f - P.velocity.Y), P.width, P.height, dustType, -P.velocity.X * 0.25f, -P.velocity.Y * 0.25f, 100, color, 1.0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
				if(P.legs > 0)
				{
					edi.dyeLegs = 31;
				}
				else
				{
					int dust = Dust.NewDust(new Vector2(P.position.X - P.velocity.X, P.position.Y - 2f - P.velocity.Y), P.width, P.height, dustType, -P.velocity.X * 0.25f, -P.velocity.Y * 0.25f, 100, color, 1.0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}*/
				if(P.shadow == 0f && mp.hyperColors > 0)
				{
					mp.hyperColors--;
				}
			}
        }
    }

   
}
