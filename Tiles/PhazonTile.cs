using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Tiles
{
	public class PhazonTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			dustType = 68;
			drop = mod.ItemType("Phazon");
			AddMapEntry(new Color(85, 223, 255), "Phazon");
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
		{	
			r = (85f/255f);
			g = (223f/255f);
			b = (255f/255f);
		}
	}
}

