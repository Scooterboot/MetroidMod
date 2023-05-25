using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class SerrisCoreX : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Serris Core-X");
			/* Tooltip.SetDefault("Soft and squishy\n" + 
			"Seems to react to kinetic energy, and amplifies it"); */
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 8));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 64;
			Item.height = 64;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
		}
	}
}
