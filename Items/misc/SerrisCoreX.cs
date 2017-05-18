using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
    public class SerrisCoreX : ModItem
    {
		public override void SetDefaults()
		{
			item.name = "Serris Core-X";
			item.maxStack = 99;
			item.toolTip = "Soft and squishy\n" + 
			"Seems to react to kinetic energy, and amplifies it";
			item.width = 64;
			item.height = 64;
			item.value = 10000;
			item.rare = 5;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
        public override DrawAnimation GetAnimation()
		{
			return new DrawAnimationVertical(5, 8);
		}
	}
}