using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using MetroidMod;
using MetroidMod.ID;
using static MetroidMod.MetroidMod;

namespace ExampleMetroidAddonMod.Content.SuitAddons
{
	public class ExampleBoot : ModSuitAddon
	{
		// See ExampleBomb.cs for these three fields.
		public override string ItemTexture => $"{Mod.Name}/Content/SuitAddons/ExampleBootItem";

		public override string TileTexture => $"{Mod.Name}/Content/SuitAddons/ExampleBootTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			// See ExampleBomb.cs (or Example Mod) to know what these two lines do.
			DisplayName.SetDefault("Example Boot");
			Tooltip.SetDefault("This is an example suit");

			// This declares what suit addon slot the addon will be equippable in.
			// There are, as of writing, 14 slots.
			// In general, if you're setting it to any 'SuitAddonSlotID.Suit_X'
			// slot, you should have set ArmorTextureHead and other relatedly-
			// named values to a texture.
			AddonSlot = SuitAddonSlotID.Boots_Jump;
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Blue;
		}

		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.hasJumpOption_Sandstorm = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(SuitAddonLoader.GetAddon("MetroidMod/HiJumpBoots").ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
	}
}
