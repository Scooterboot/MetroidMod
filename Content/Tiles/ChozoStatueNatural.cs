using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles
{
	public class ChozoStatueNatural : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileObjectData.newTile.LavaDeath = false;
			Main.tileObsidianKill[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileSpelunker[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX); 
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 18 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2; 
			TileObjectData.newTile.StyleMultiplier = 2; 
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; 
			TileObjectData.addAlternate(1); 
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			Main.tileOreFinderPriority[Type] = 806;
			name.SetDefault("Chozo Statue");
			AddMapEntry(new Color(90, 90, 90), name);
			DustType = 1;
			TileID.Sets.DisableSmartCursor[Type] = true;//disableSmartCursor = true;
			MineResist = 4f;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Tiles.ChoziteOre>(), Main.rand.Next(10, 25));
		}
	}
}
