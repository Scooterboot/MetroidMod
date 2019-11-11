using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class GripLedge : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grip Ledge");
            Tooltip.SetDefault("Cannot be stood on. \nCan be gripped to using Power Grip \nCan be toggle on or off with wire");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("GripLedge");
			item.rare = 2;
		}
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe();
            recipe.AddIngredient(mod, "ChoziteBar", 4);
            recipe.AddIngredient(ItemID.Emerald, 3);
            recipe.AddIngredient(ItemID.Topaz, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 99);
            recipe.AddRecipe();
        }
    }
}