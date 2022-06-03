using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Tiles.ItemTile
{
	public class MissileExpansionTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Missile Expansion");
			AddMapEntry(new Color(132, 4, 20), name);
			ItemDrop = ModContent.ItemType<Items.Tiles.MissileExpansion>();
			Main.tileOreFinderPriority[Type] = 805;
			DustType = 1;
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
