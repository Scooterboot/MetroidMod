using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Tiles
{
	public class GripLedgeOff : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = false;
			Main.tileSolidTop[Type] = false;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = false;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.Platforms[Type] = false;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(new Color(39, 76, 39));
			drop = mod.ItemType("GripLedge");
			disableSmartCursor = true;
			adjTiles = new int[]{ TileID.Platforms };
		}

		public override void PostSetDefaults()
		{
			Main.tileNoSunLight[Type] = false;
        }
        public override void HitWire(int i, int j)
        {
            Main.tile[i, j].type = (ushort)mod.TileType("GripLedge");
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}