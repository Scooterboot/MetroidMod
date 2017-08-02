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
	public class GravitySuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit Greaves");
			Tooltip.SetDefault("5% increased ranged damage\n" + 
            "20% increased movement speed\n" + 
            "+20 overheat capacity\n" + 
            "Allows you to slide down walls");
		}
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 4;
            item.value = 24000;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.moveSpeed += 0.20f;
            player.spikedBoots += 1;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 20;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitBreastplate");
            recipe.AddIngredient(null, "GravityGel", 17);
            recipe.AddIngredient(ItemID.Wire, 8);
            recipe.AddIngredient(ItemID.CursedFlame, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitBreastplate");
            recipe.AddIngredient(null, "GravityGel", 17);
            recipe.AddIngredient(ItemID.Wire, 8);
            recipe.AddIngredient(ItemID.Ichor, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
