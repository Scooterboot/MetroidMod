
using MetroidMod.Common.Systems;
using MetroidMod.Content.Items.Tiles.Destroyable;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tools
{

	public class ChoziteDualtool : ModItem
	{
		private bool isStarted;
		private bool isRemoving;
		private bool applyRegen = true;

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && Main.mouseLeft)
			{
				(int i, int j) = (Player.tileTargetX, Player.tileTargetY);
				ushort placeType = BreakableTileID.ScrewAttack;

				if (!isStarted)
				{
					isStarted = true;
					isRemoving = FakeBlock.ExistsAt(i, j, placeType) && FakeBlock.Regens(i, j) == applyRegen;
				}

				if(isRemoving)
				{
					ChoziteCutter.RemoveBlockAt(player, i, j);
				}
				else
				{
					if (!FakeBlock.ExistsAt(i, j, placeType))
					{
						if (FakeBlock.ExistsAt(i, j))
						{
							ChoziteCutter.RemoveBlockAt(player, i, j);
						}

						FakeBlock.Place(player, i, j, placeType);
					}


					FakeBlock.SetRegen(i, j, applyRegen);
				}
			}

			return null;
		}

		public override void HoldItem(Player player)
		{
			if (!Main.mouseLeft && player.itemAnimation == 0)
			{
				isStarted = false;
			}

			if(Main.mouseRight && Main.mouseRightRelease)
			{
				applyRegen = !applyRegen;

				int dustAmount = 10;
				Color color = applyRegen ? Color.White : Color.Red;
				CombatText.NewText(player.Hitbox, color, applyRegen ? "Regen" : "No Regen");
			}
		}

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
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.rare = 2;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteWrench>()
				.AddIngredient<ChoziteCutter>()
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
