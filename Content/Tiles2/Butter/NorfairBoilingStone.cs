using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class NorfairBoilingStone : ModTile
	{
		public override string Texture => "MetroidMod/Content/Tiles2/Butter/Tile/NorfairBoilingStone";

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			DustType = DustID.Torch;
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(144, 24, 24));
			AnimationFrameHeight = 90;


		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 18)
			{
				frame++;
				if (frame > 6)
					frame = 0;
				frameCounter = 0;
			}
		}
	}
}
