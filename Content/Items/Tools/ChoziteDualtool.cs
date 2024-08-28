
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
		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && Main.mouseLeft)
			{
				(int i, int j) = (Player.tileTargetX, Player.tileTargetY);
				ushort placeType = BreakableTileID.ScrewAttack;

				if(ChoziteDualtoolSettings.IsPlacing)
				{
					bool neededTypeIsThere = FakeBlock.ExistsAt(i, j, placeType);

					Tile tile = Main.tile[i, j];
					bool onSolidTile = tile.HasTile && Main.tileSolid[tile.TileType];
					bool solidCondition = ChoziteDualtoolSettings.AllowPlaceOnEmpty || onSolidTile;
					
					if (!neededTypeIsThere && ChoziteDualtoolSettings.AllowPlaceNew && solidCondition)
					{
						ChoziteCutter.RemoveBlockAt(player, i, j);
						FakeBlock.Place(player, i, j, placeType);
						neededTypeIsThere = true;
					}

					if (neededTypeIsThere)
					{
						FakeBlock.SetRegen(i, j, ChoziteDualtoolSettings.ApplyRegen);
					}
				}
				else
				{
					ChoziteCutter.RemoveBlockAt(player, i, j);
				}
			}

			return null;
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
