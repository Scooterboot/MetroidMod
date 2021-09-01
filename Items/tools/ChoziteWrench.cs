#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Worlds;

#endregion

namespace MetroidMod.Items.tools
{
	public class ChoziteWrench : ModItem
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Wrench");
            Tooltip.SetDefault("Toggles regeneration of weapon-destructable blocks. \nBlocks with disabled regeneration will have a red tint.");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 1;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.rare = 1;
		}
        
        public override bool UseItem(Player player)
        {
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            Vector2 pos = new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16);
            if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] != 0)
            {
                MWorld.dontRegen[Player.tileTargetX, Player.tileTargetY] = !MWorld.dontRegen[Player.tileTargetX, Player.tileTargetY];
                Wiring.ReActive(Player.tileTargetX, Player.tileTargetY);
                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wrench);
            recipe.AddIngredient(null, "ChoziteBar", 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}