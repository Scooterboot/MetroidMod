using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Legs)]
    class ChoziteGreaves : ModItem
    {
    public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Greaves");
		}

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.value = 3000;
            item.defense = 4;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ChoziteBar", 25);
            recipe.AddIngredient(ItemID.Topaz);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
