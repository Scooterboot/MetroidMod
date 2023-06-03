using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class EnergyShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Energy Shard");
			// Tooltip.SetDefault("'It could be used to power something if it were whole'");

			Item.ResearchUnlockCount = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 18;
			Item.height = 16;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
		}
	}
}
