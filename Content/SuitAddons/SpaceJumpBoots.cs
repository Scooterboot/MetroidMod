using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.ID;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.SuitAddons
{
	public class SpaceJumpBoots : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJumpBoots/SpaceJumpBootsItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJumpBoots/SpaceJumpBootsTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Jump Boots");
			Tooltip.SetDefault("Allows the wearer to double jump\n" +
				"Allows somersaulting");
			AddonSlot = SuitAddonSlotID.Boots_Jump;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
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
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				//.AddIngredient(SuitAddonLoader.GetAddon<HiJumpBoots>().Item, 1)
				.AddIngredient(ItemID.BlizzardinaBottle, 1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				//.AddIngredient(SuitAddonLoader.GetAddon<HiJumpBoots>().Item, 1)
				.AddIngredient(ItemID.SandstorminaBottle, 1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				//.AddIngredient(SuitAddonLoader.GetAddon<HiJumpBoots>().Item, 1)
				.AddIngredient(ItemID.TsunamiInABottle, 1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
