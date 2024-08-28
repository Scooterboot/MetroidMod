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
			if (player.whoAmI == Main.myPlayer && MUtils.CanReachWiring(player, Item))
			{
				bool leftClicked = Main.mouseLeft && Main.mouseLeftRelease;
				if (leftClicked && InteractWithHatchLocal())
				{
					return true;
				}

				if(ModContent.GetInstance<ChoziteWrenchAssistSystem>().HitTile(Player.tileTargetX, Player.tileTargetY))
				{
					return true;
				}
			}

			return false;
		}

		public static bool  InteractWithHatchLocal()
		{
			// Presumably tileTargetX and Y are only for the LocalPlayer, so this code should only run for them.
			// This does mean the visual effect won't actually show for others. That is acceptable for now?
			if (!TileUtils.TryGetTileEntityAs(Player.tileTargetX, Player.tileTargetY, out HatchTileEntity tileEntity))
			{
				return false;
			}
			
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


		public override void HoldItem(Player player)
		{
			if (MUtils.CanReachWiring(player, Item))
			{
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = Type;
			}
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
