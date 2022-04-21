using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ModLoader;
using Terraria;

namespace MetroidModPorted.Content.Tiles
{
	public class ChoziteOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileLighted[Type] = true;
			DustType = 87;
			ItemDrop = ModContent.ItemType<Items.Tiles.ChoziteOre>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozite Ore");
			AddMapEntry(new Color(214, 162, 0), name);

			MinPick = 50;
			SoundType = 21;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
	}
}

