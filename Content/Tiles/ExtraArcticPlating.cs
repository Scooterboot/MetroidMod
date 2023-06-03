using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles
{
	public class ExtraArcticPlating : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(132, 231, 214));
		}
	}
}
