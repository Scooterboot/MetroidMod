using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using MetroidModPorted;
using MetroidModPorted.ID;
using static MetroidModPorted.MetroidModPorted;

namespace ExampleMetroidAddonMod.Content.SuitAddons
{
	public class ExampleSpeedUpgrade : ModSuitAddon
	{
		// See ExampleBomb.cs for these three fields.
		public override string ItemTexture => $"{Mod.Name}/Content/SuitAddons/ExampleSpeedUpgradeItem";

		public override string TileTexture => $"{Mod.Name}/Content/SuitAddons/ExampleSpeedUpgradeTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			// See ExampleBomb.cs (or Example Mod) to know what these two lines do.
			DisplayName.SetDefault("Example Speed Upgrade");
			Tooltip.SetDefault("This is an example of a speed upgrade for the power suit.");

			// See ExampleBoot.cs
			AddonSlot = SuitAddonSlotID.Boots_Speed;
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Orange;
		}

		public override void OnUpdateArmorSet(Player player, int stack)
		{
			// Set running speed to rediculously high value.
			player.accRunSpeed = 128f;

			// Speed up quickly.
			player.runAcceleration = 32f;
		}
	}
}
