using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.ID;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.SuitAddons
{
	public class ReserveTank : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ReserveTank/ReserveTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ReserveTank/ReserveTankTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reserve Tank");
			Tooltip.SetDefault("Stores a heart picked up when at full health\n" +
				"Automatically uses the stored heart to save you from death");
			ItemNameLiteral = true;
			SacrificeTotal = 4;
			AddonSlot = SuitAddonSlotID.Tanks_Reserve;
		}
		public override void SetItemDefaults(Item item)
		{
			item.maxStack = 4;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddIngredient(ItemID.LifeCrystal, 1)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient<Items.Accessories.ReserveTank>(1)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.SuitReserveTanks = stack;
		}
	}
}
