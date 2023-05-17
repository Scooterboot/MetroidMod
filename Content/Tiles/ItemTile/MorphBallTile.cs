using System.Collections;
using System.Collections.Generic;
using MetroidMod.Content.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class MorphBallTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Morph Ball");
			AddMapEntry(new Color(250, 85, 34), name);
			DustType = 1;
		}
		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ModContent.ItemType<MorphBall>());
		}
	}
}
