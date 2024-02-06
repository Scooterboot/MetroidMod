#region Using directives

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Content.Tiles
{
	public class PhazonTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<PhazonCore>()] = true;

			DustType = 68;
			MinPick = 1000;//215;
			HitSound = SoundID.Tink;

			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Phazon");
			AddMapEntry(new Color(85, 223, 255), name);
		}

		public override bool CanExplode(int i, int j) => false;
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
		{
			r = (85f / 255f);
			g = (223f / 255f);
			b = (255f / 255f);
		}
	}
}
