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
			Item.tileBoost = 20;
		}

		public override bool? UseItem(Player player)
		{
			if (Main.mouseLeft)
			{
				ModContent.GetInstance<ChoziteWrenchAssistSystem>().HitTile(Player.tileTargetX, Player.tileTargetY);
			}

			if (player.whoAmI == Main.myPlayer)
			{
				// Presumably tileTargetX and Y are only for the LocalPlayer, so this code should only run for them.
				// This does mean the visual effect won't actually show for others. That is acceptable for now?
				if (TileUtils.TryGetTileEntityAs(Player.tileTargetX, Player.tileTargetY, out HatchTileEntity tileEntity))
				{
					DebugAssist.NewTextMP("Hit with Chozo Wrench");

					tileEntity.State.ToggleBlueConversion();
					tileEntity.SyncState();

					Color color = tileEntity.State.BlueConversion == HatchBlueConversionStatus.Disabled ?
						Color.Red : Color.Cyan;

					int i = tileEntity.Position.X;
					int j = tileEntity.Position.Y;
					Vector2 topLeft = new Point(i, j).ToWorldCoordinates(0, 0);
					Vector2 bottomRight = new Point(i + 4, j + 4).ToWorldCoordinates(0, 0);
					Dust.QuickBox(topLeft, bottomRight, 8, color, null);

					return true;
				}
			}

			return false;
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
