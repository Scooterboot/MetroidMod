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
	public class VariaSuitV2Helmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit V2 Helmet");
			Tooltip.SetDefault("7% increased ranged damage\n" + 
            "+15 overheat capacity\n" + 
            "Improved night vision");
		}
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 4;
            item.value = 9000;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.07f;
            player.nightVision = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 15;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitHelmet");
            recipe.AddIngredient(null, "KraidTissue", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
