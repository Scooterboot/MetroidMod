#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Worlds;

#endregion

namespace MetroidMod.Items.tiles
{
	public class CrumbleBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble Block (Instant)");
            Tooltip.SetDefault("Deactivates a tile when a player stands on it \nUse Chozite Cutters to break.");
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
			item.rare = 1;
		}

        // Netsyncing ?
        public override bool UseItem(Player player)
        {
            if (player.itemTime == 0 && player.itemAnimation > 0 && player.controlUseItem)
            {
				Vector2 pos = new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16);
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 0)
                {
                    MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] = 1;
                    player.ConsumeItem(item.type);
                    Main.PlaySound(SoundID.Dig, pos);
                }
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Actuator);
            recipe.AddIngredient(ItemID.SandBlock);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
