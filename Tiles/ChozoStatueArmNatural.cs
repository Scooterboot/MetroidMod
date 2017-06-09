using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Tiles
{
	public class ChozoStatueArmNatural : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileNoAttach[Type] = false;
			Main.tileTable[Type] = true;
			//Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 18 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2; 
			TileObjectData.newTile.StyleMultiplier = 2; 
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; 
			TileObjectData.addAlternate(1); 
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozo Statue");
			AddMapEntry(new Color(90, 90, 90), name);
			dustType = 1;
			disableSmartCursor = true;
			mineResist = 4f;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 32, mod.ItemType("ChoziteOre"), Main.rand.Next(5, 15));
		}
	}
}