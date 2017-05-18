using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
    public class GravityGel : ModItem
    {
		public override void SetDefaults()
		{
			item.name = "Gravity Gel";
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.toolTip = "'Totally breaking Newton's laws.'";
			item.value = 10000;
			item.rare = 5;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
    
	}
}