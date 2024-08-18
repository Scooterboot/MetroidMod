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
			if (Main.mouseLeft)
			{
				ModContent.GetInstance<ChoziteWrenchAssistSystem>().HitTile(Player.tileTargetX, Player.tileTargetY);
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
		}
	}
}
