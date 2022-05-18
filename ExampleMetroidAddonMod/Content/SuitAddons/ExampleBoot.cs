using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using MetroidModPorted;
using MetroidModPorted.ID;
using static MetroidModPorted.MetroidModPorted;

namespace ExampleMetroidAddonMod.Content.SuitAddons
{
	public class ExampleBoot : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Content/SuitAddons/ExampleBootItem";

		public override string TileTexture => $"{Mod.Name}/Content/SuitAddons/ExampleBootTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Example Boot");
			Tooltip.SetDefault("This is an example suit");

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
				.AddIngredient(SuitAddonLoader.GetAddon("MetroidModPorted/HiJumpBoots").ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
	}
}
