#region Using directives

using MetroidMod.Common.Systems;
using MetroidMod.Content.Hatches;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

			
			if (TileUtils.TryGetTileEntityAs(Player.tileTargetX, Player.tileTargetY, out HatchTileEntity tileEntity))
			{
				tileEntity.Behavior.HitChozoWrench();

				Color color = tileEntity.Behavior.BlueConversion == HatchBlueConversionStatus.Enabled ?
					Color.Cyan : Color.Red;

				int i = tileEntity.Position.X;
				int j = tileEntity.Position.Y;
				Vector2 topLeft = new Point(i, j).ToWorldCoordinates(0, 0);
				Vector2 bottomRight = new Point(i + 4, j + 4).ToWorldCoordinates(0, 0);
				Dust.QuickBox(topLeft, bottomRight, 8, color, null);

				return true;
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
