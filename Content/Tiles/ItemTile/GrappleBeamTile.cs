using System.Collections.Generic;
using MetroidMod.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class GrappleBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Grapple Beam");
			AddMapEntry(new Color(121, 221, 139), name);
			DustType = 1;
		}
		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ModContent.ItemType<GrappleBeam>());
		}
	}
}
