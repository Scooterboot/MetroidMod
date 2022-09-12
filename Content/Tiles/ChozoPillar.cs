using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles
{
	public class ChozoPillar : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.IsBeam[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			ItemDrop = ModContent.ItemType<Items.Tiles.ChozoPillar>();

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozite Pillar");
			AddMapEntry(new Color(200, 160, 72), name);
		}
	}
}
