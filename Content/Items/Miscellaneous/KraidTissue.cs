using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class KraidTissue : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Reptile Tissue");
			Tooltip.SetDefault("Tough tissue that can be used to upgrade the Varia Suit");

			SacrificeTotal = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 34;
			Item.height = 32;
			Item.value = 100;
			Item.rare = ItemRarityID.LightRed;
		}

	}
}
