using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles.Hatch
{
	public class BlueHatchOpen : BlueHatch
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
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			TileID.Sets.HousingWalls[Type] = true;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Blue Hatch");
			AddMapEntry(new Color(56, 112, 224), name);
			AdjTiles = new int[] { TileID.OpenDoor };

			otherDoorID = ModContent.TileType<BlueHatch>();
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if (type == Type)
			{
				short doorHeight = 72;
				Tile tile = Main.tile[i, j];
				while (tile.TileFrameY + frameYOffset >= doorHeight)
				{
					frameYOffset -= doorHeight;
				}
				if (tile.TileFrameY < doorHeight * 4)
				{
					if (tile.TileFrameY < doorHeight)
					{
						tile.TileFrameY += doorHeight;
					}
					tile.TileFrameY += doorHeight;
				}
			}
		}
	}
}
