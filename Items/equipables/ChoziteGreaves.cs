using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.equipables
{
    class ChoziteGreaves : ModItem
    {
        public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
        {
            equips.Add(EquipType.Legs);
            return true;
        }

        public override void SetDefaults()
        {
            item.name = "Chozite Greaves";
            item.width = 18;
            item.height = 18;
            item.rare = 1;
            item.value = 3000;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
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
