using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace MetroidMod.Content.Tiles.Hatch
{
	public class YellowHatchOpenVertical : BlueHatch
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.DrawsWalls[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			TileID.Sets.HousingWalls[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Yellow Hatch");
			AddMapEntry(new Color(248, 232, 56), name);
			AdjTiles = new int[]{ TileID.OpenDoor };
			MinPick = 210;
			
			otherDoorID = ModContent.TileType<YellowHatchVertical>();
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Tiles.YellowHatch>();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Tiles.YellowHatch>());
		}

		public override bool RightClick(int i, int j)
		{
			HitWire(i, j);
			if (Main.netMode == NetmodeID.MultiplayerClient) { SendRightClick(i, j); }
			return true;
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if(type == Type)
			{
				short doorHeight = 72;
				Tile tile = Main.tile[i, j];
				while(tile.TileFrameY+frameYOffset >= doorHeight)
				{
					frameYOffset -= doorHeight;
				}
				if(tile.TileFrameY < doorHeight*4)
				{
					if(tile.TileFrameY < doorHeight)
					{
						tile.TileFrameY += doorHeight;
					}
					tile.TileFrameY += doorHeight;
				}
			}
		}
		
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawDoor(i,j,spriteBatch,ModContent.Request<Texture2D>($"{Mod.Name}/Content/Tiles/Hatch/YellowHatchVerticalDoor").Value);
			return true;
		}
	}
}
