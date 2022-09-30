using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.ID;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.SuitAddons
{
	public class EnergyTank : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Tank");
			Tooltip.SetDefault("Grants the user an extra tank of energy.");
			ItemNameLiteral = true;
			SacrificeTotal = Common.Configs.MServerConfig.Instance.stackEnergyTank;
			AddonSlot = SuitAddonSlotID.Tanks_Energy;
		}
		public override void SetItemDefaults(Item item)
		{
			item.maxStack = Common.Configs.MServerConfig.Instance.stackEnergyTank;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(4)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.EnergyTanks = Math.Min(stack, Common.Configs.MServerConfig.Instance.stackEnergyTank);
		}
	}
}
