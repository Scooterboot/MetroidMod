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
[AutoloadEquip(EquipType.Legs)]
	public class VariaSuitV2Greaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit V2 Greaves");
			Tooltip.SetDefault("6% increased ranged damage\n" +
             "10% increased movement speed\n" +
             "+15 overheat capacity\n" +
             "Allows you to slide down walls");
		}
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.value = 6000;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.06f;
            player.moveSpeed += 0.10f;
            player.spikedBoots += 1;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 15;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitGreaves");
            recipe.AddIngredient(null, "KraidTissue", 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
