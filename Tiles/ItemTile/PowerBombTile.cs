using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class PowerBombTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Power Bombs");
			AddMapEntry(new Color(160, 0, 176), name);
			drop = mod.ItemType("PowerBombAddon");
			dustType = 1;
			animationFrameHeight = 18;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (frameCounter++ > 3)
			{
				if (frame++ > 2)
					frame = 0;
				frameCounter = 0;
			}
		}
	}
}