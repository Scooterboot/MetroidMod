using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Items.Addons.Hunters;
using MetroidMod.ID;

namespace MetroidMod.Content.Items.Addons
{
	public class PhazonBeamAddon : ModItem, IBeamAddon
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon Beam");
			/* Tooltip.SetDefault("Power Beam Addon\n" +
			"Slot Type: Charge\n" +
			"Decreases base damage from 14 to 6, and base overheat use from 4 to 1\n" +
			"Dramatically increases firerate\n" +
			"Affected by addons regardless of version\n" + 
			"Disables freeze and other debuff effects\n" +
			"Can only be used while wearing the Phazon Suit\n" +
			"'It's made of pure Phazon energy!'"); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<HyperBeamAddon>();
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 100000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.PhazonBeamTile>();
   			this.beamID = BeamID.Beam.Phazon;
      			this.slotType = BeamID.SlotType.Charge;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.SDMG, 1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.FragmentSolar, 10)
				.AddIngredient(ItemID.FragmentNebula, 10)
				.AddIngredient(ItemID.FragmentVortex, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarFlareBook, 1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.FragmentSolar, 10)
				.AddIngredient(ItemID.FragmentNebula, 10)
				.AddIngredient(ItemID.FragmentVortex, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.StarWrath, 1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.FragmentSolar, 10)
				.AddIngredient(ItemID.FragmentNebula, 10)
				.AddIngredient(ItemID.FragmentVortex, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
