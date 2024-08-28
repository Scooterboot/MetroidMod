﻿
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
		private int currentPlaceType = -1;

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && Main.mouseLeft && MUtils.CanReachWiring(player, Item))
			{
				(int i, int j) = (Player.tileTargetX, Player.tileTargetY);

				if(ChoziteDualtoolSettings.IsPlacing)
				{
					Item ammoItem = player.ChooseAmmo(Item);
					bool hasAmmo = ammoItem != null;

					if (hasAmmo)
					{
						ushort placeType = (ammoItem.ModItem as FakeBlock).PlaceType;
						bool neededTypeIsThere = FakeBlock.ExistsAt(i, j, placeType);

						Tile tile = Main.tile[i, j];
						bool onSolidTile = tile.HasTile && Main.tileSolid[tile.TileType];
						bool solidCondition = ChoziteDualtoolSettings.AllowPlaceOnEmpty || onSolidTile;

						if (!neededTypeIsThere && ChoziteDualtoolSettings.AllowPlaceNew && solidCondition)
						{
							ammoItem.stack -= 1;
							ChoziteCutter.RemoveBlockAt(player, i, j);
							FakeBlock.Place(player, i, j, placeType);
							currentPlaceType = placeType;
						}
					}

					FakeBlock.SetRegen(i, j, ChoziteDualtoolSettings.ApplyRegen);
				}
				else
				{
					ChoziteCutter.RemoveBlockAt(player, i, j);
				}
			}

			return false;
		}

		public override void HoldItem(Player player)
		{
			if (MUtils.CanReachWiring(player, Item))
			{
				Item ammoItem = player.ChooseAmmo(Item);
				if (ammoItem != null)
				{
					player.cursorItemIconEnabled = true;
					player.cursorItemIconID = ammoItem.type;
				}
			}

			bool usingItem = player.ItemAnimationActive || Main.mouseLeft;
			if(!usingItem)
			{
				currentPlaceType = -1;
			}
		}

		public override bool? CanChooseAmmo(Item ammo, Player player)
		{
			if(currentPlaceType != -1)
			{
				if(ammo.ModItem is FakeBlock fakeBlock)
				{
					if(fakeBlock.PlaceType != currentPlaceType)
					{
						return false;
					}
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
			Item.rare = 1;
			Item.tileBoost = 20;
			Item.useAmmo = ModContent.ItemType<FakeBlock>();
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
