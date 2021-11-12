using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class MissileExpansionTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Missile Expansion");
			AddMapEntry(new Color(132, 4, 20), name);
			drop = mod.ItemType("MissileExpansion");
			Main.tileValue[Type] = 805;
			dustType = 1;
			animationFrameHeight = 18;
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