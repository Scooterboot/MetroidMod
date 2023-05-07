using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class FrozenCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supercooled Plasma Core");
			// Tooltip.SetDefault("'Strange energy core capable of producing supercooled plasma'");

			Item.ResearchUnlockCount = 5;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 18;
			Item.height = 16;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
		}
	}
}
