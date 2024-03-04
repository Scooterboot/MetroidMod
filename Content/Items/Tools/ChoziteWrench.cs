#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Systems;
using MetroidMod.ID;

#endregion

namespace MetroidMod.Content.Items.Tools
{
	public class ChoziteWrench : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozite Wrench");
			// Tooltip.SetDefault("Toggles regeneration of weapon-destructable blocks. \nBlocks with disabled regeneration will have a red tint.");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
		}

		public override bool? UseItem(Player player)
		{
			//Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
			//Vector2 pos = new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16);
			if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] != BreakableTileID.None)
			{
				MSystem.dontRegen[Player.tileTargetX, Player.tileTargetY] = !MSystem.dontRegen[Player.tileTargetX, Player.tileTargetY];
				Wiring.ReActive(Player.tileTargetX, Player.tileTargetY);
			}
			return base.UseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Wrench)
				.AddIngredient<Miscellaneous.ChoziteBar>(5)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wrench);
			recipe.AddIngredient(null, "ChoziteBar", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
