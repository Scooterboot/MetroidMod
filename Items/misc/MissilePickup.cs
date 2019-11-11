using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
    public class MissilePickup : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile");
			ItemID.Sets.ItemNoGravity[item.type] = true;
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 2));
		}
		public override void SetDefaults()
		{
			item.maxStack = 255;
			item.width = 30;
			item.height = 30;
			item.value = 100;
			item.rare = 1;
		}
		public override bool OnPickup(Player player)
		{
			for(int i = 0; i < player.inventory.Length; i++)
			{
				if(player.inventory[i].type == mod.ItemType("MissileLauncher"))
				{
					MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>();
					mi.statMissiles += item.stack;
				}
			}
			Main.PlaySound(7,(int)player.position.X,(int)player.position.Y,1);
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.White, item.stack, false, false);
			return false;
		}
	}
}
