using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
	public class UnknownPlasmaBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unknown Item");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Primary B\n" +
				string.Format("[c/BE7878:Error: Addon is damaged and requires repairing before it can be installed.]"));
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 10;
			item.height = 14;
			item.value = 100;
			item.rare = 4;
		}
	}
}