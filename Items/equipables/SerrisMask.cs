using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Items.equipables
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
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.value = 0;
            item.vanity = true;
        }
	}
}