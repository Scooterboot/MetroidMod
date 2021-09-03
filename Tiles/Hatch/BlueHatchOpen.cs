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
	public class BlueHatchOpen : BlueHatch
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.DrawsWalls[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			TileID.Sets.HousingWalls[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blue Hatch");
			AddMapEntry(new Color(56, 112, 224), name);
			adjTiles = new int[]{ TileID.OpenDoor };
			
			otherDoorID = mod.TileType("BlueHatch");
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
	}
}