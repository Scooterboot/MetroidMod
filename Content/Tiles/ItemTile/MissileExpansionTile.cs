using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class MissileExpansionTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			//base.SetStaticDefaults();
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Missile Expansion");
			AddMapEntry(new Color(132, 4, 20), name);
			ItemDrop = ModContent.ItemType<Items.Tiles.MissileExpansion>();
			Main.tileOreFinderPriority[Type] = 805;
			DustType = 1;
			AnimationFrameHeight = 18;
			Main.tileLavaDeath[Type] = false;
			Main.tileObsidianKill[Type] = false;
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
