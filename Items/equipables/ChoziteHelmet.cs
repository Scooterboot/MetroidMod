﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Head)]
    class ChoziteHelmet : ModItem
    {

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Helmet");
		}

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.value = 3000;
            item.defense = 5;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ChoziteBar", 20);
            recipe.AddIngredient(ItemID.Topaz);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
