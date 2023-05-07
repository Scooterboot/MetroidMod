using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;

using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.Tiles
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
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Chozite Ore");
			AddMapEntry(new Color(214, 162, 0), name);

			MinPick = 50;
			HitSound = SoundID.Tink;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
	}
}

