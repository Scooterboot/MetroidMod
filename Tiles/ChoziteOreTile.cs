using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Tiles
{
	public class ChoziteOreTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileLighted[Type] = true;
			dustType = 87;
			drop = mod.ItemType("ChoziteOre");
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozite Ore");
			AddMapEntry(new Color(214, 162, 0), name);

			minPick = 55;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}

