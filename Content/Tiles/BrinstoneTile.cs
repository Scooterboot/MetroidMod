﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles
{
	public class BrinstoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			ItemDrop = ModContent.ItemType<Items.Tiles.BrinstoneTile>();

			AddMapEntry(new Color(24, 64, 152));
		}
	}
}
