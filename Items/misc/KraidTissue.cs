using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
	public class KraidTissue : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heat-Resistant Tissue");
			Tooltip.SetDefault("'Tissue from Kraid, it could be used to upgrade your suit...'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 34;
			item.height = 32;
			item.value = 100;
			item.rare = 4;
		}

	}
}