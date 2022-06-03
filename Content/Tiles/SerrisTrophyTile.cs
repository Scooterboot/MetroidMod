using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidModPorted.Content.Tiles
{
	public class SerrisTrophyTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			DustType = 7;
			Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;//disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Serris Trophy");
			AddMapEntry(new Color(137, 98, 69), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Tiles.SerrisTrophy>());
		}
	}
}
