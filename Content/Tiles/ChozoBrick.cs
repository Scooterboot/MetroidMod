using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidModPorted.Content.Tiles
{
	public class ChozoBrick : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;
			ItemDrop = ModContent.ItemType<Items.Tiles.ChozoBrick>();

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozite Brick");
			AddMapEntry(new Color(200, 160, 72), name);
		}
	}
}
