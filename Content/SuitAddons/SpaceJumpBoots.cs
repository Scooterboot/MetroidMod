using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.ID;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.SuitAddons
{
	public class SpaceJumpBoots : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJumpBoots/SpaceJumpBootsItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJumpBoots/SpaceJumpBootsTile";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Jump Boots");
			Tooltip.SetDefault("Allows the wearer to double jump\n" +
				"Allows somersaulting\n" +
				"Increases jump height");
			AddonSlot = SuitAddonSlotID.Boots_Jump;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void OnUpdateArmorSet(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			//mp.hiJumpBoost = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			//mp.hiJumpBoost = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				//.AddIngredient(SuitAddonLoader.GetAddon<HiJumpBoots>().Item, 1)
				.AddIngredient(ItemID.CloudinaBottle, 1)
				.AddIngredient<Items.Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				//.AddIngredient(SuitAddonLoader.GetAddon<HiJumpBoots>().Item, 1)
				.AddIngredient(ItemID.BlizzardinaBottle, 1)
				.AddIngredient<Items.Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				//.AddIngredient(SuitAddonLoader.GetAddon<HiJumpBoots>().Item, 1)
				.AddIngredient(ItemID.SandstorminaBottle, 1)
				.AddIngredient<Items.Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				//.AddIngredient(SuitAddonLoader.GetAddon<HiJumpBoots>().Item, 1)
				.AddIngredient(ItemID.TsunamiInABottle, 1)
				.AddIngredient<Items.Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
