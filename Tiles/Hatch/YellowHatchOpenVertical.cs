using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace MetroidMod.Tiles.Hatch
{
	public class YellowHatchOpenVertical : BlueHatch
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
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
			adjTiles = new int[]{ TileID.OpenDoor };
		}

		public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("YellowHatch");
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("YellowHatch"));
        }

        public override bool NewRightClick(int i, int j)
		{
			HitWire(i, j);
			return true;
		}
		public override void HitWire(int i, int j)
		{
			ToggleHatch(i,j,(ushort)mod.TileType("YellowHatchVertical"),true);
		}
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if(type == Type)
			{
				short doorHeight = 72;
				Tile tile = Main.tile[i, j];
				while(tile.frameY+frameYOffset >= doorHeight)
				{
					frameYOffset -= doorHeight;
				}
				if(tile.frameY < doorHeight*4)
				{
					if(tile.frameY < doorHeight)
					{
						tile.frameY += doorHeight;
					}
					tile.frameY += doorHeight;
				}
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            DrawDoor(i,j,spriteBatch,mod.GetTexture("Tiles/Hatch/YellowHatchVerticalDoor"));
            return true;
        }
	}
}
