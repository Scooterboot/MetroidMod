using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class GravityGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Gel");
			Tooltip.SetDefault("'Totally breaking Newton's laws.'");
			ItemID.Sets.ItemNoGravity[Type] = true;

			SacrificeTotal = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			
		}
	
	}
}
