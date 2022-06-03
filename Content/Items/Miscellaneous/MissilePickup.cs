using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.Miscellaneous
{
    public class MissilePickup : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile");
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 2));
		}
		public override void SetDefaults()
		{
			Item.maxStack = 255;
			Item.width = 30;
			Item.height = 30;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
		}
		public override bool ItemSpace(Player player) => true;
		public override bool OnPickup(Player player)
		{
			for(int i = 0; i < player.inventory.Length; i++)
			{
				if(player.inventory[i].type == ModContent.ItemType<Weapons.MissileLauncher>())
				{
					MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>();
					mi.statMissiles += Item.stack;
				}
			}
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab,player.position);
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.White, Item.stack, false, false);
			return false;
		}
	}
}
