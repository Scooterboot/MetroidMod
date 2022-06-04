using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidModPorted.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class SerrisMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris Mask");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 0;
			Item.vanity = true;
		}
	}
}
