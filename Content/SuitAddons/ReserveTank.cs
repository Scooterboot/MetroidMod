using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.ID;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.SuitAddons
{
	public class ReserveTank : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ReserveTank/ReserveTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ReserveTank/ReserveTankTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => true;

		public override double GenerationChance(int x, int y) => WorldGen.drunkWorldGen && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues ? 20 : 15;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Reserve Tank");
			// TODO: Write a better tooltip. I'm tired. - DarkSamus49
			// Tooltip.SetDefault("Can store a spare tank of energy.");
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Items.Accessories.ReserveTank>();
			ItemNameLiteral = true;
			SacrificeTotal = 4;
			AddonSlot = SuitAddonSlotID.Tanks_Reserve;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 11;
			item.maxStack = 4;
			item.value = Item.buyPrice(0, 5, 0, 0);
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
