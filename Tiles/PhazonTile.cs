#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

#endregion

namespace MetroidMod.Tiles
{
	public class PhazonTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<PhazonCore>()] = true;
			
			dustType = 68;
			minPick = 1000;//215;
			soundType = SoundID.Tink;
			drop = ModContent.ItemType<Items.tiles.Phazon>();

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Phazon");
			AddMapEntry(new Color(85, 223, 255), name);
		}

		public override bool CanExplode(int i, int j) => false;
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
		{	
			r = (85f/255f);
			g = (223f/255f);
			b = (255f/255f);
		}
	}
}