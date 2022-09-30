using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public abstract class MissileAbst : ItemTile
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/MissileAddons/{Name}/{Name}Tile";

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AnimationFrameHeight = 18;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 3)
			{
				frame++;
				if (frame > 2)
					frame = 0;
				frameCounter = 0;
			}
		}
	}
}
