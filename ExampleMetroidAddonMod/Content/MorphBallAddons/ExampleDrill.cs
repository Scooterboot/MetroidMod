using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using MetroidModPorted;
using static MetroidModPorted.MetroidModPorted;

namespace ExampleMetroidAddonMod.Content.MorphBallAddons
{
	internal class ExampleDrill : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Content/MorphBallAddons/ExampleDrillItem";

		public override string TileTexture => $"{Mod.Name}/Content/MorphBallAddons/ExampleDrillItem";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Example Morph Ball Drill");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~90% pickaxe power\n" +
			"This is an example morph ball drill.");

			// This sets the strength of the drill, being about the same as
			// your typical Item.pick stat.
			DrillPower = 90;
			ItemNameLiteral = true;
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Blue;
		}
	}
}
