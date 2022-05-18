using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.ID;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.SuitAddons
{
	public class EnergyTank : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Tank");
			Tooltip.SetDefault("Grants the user an extra tank of energy.");
			SacrificeTotal = 10;
			AddonSlot = SuitAddonSlotID.Tanks_Energy;
		}
		public override void SetItemDefaults(Item item)
		{
			item.maxStack = 10;
			item.value = Terraria.Item.buyPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(4)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(1)
				.AddRecipeGroup(MetroidModPorted.EvilBarRecipeGroupID, 1)
				.AddIngredient(MetroidModPorted.EvilMaterialRecipeGroupID, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.EnergyTanks = stack;
		}
	}
}
