using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Content.Tiles.ItemTile;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class EnergyTank : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankTile";

		public override bool AddOnlyAddonItem => false;

		//public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedBoss2;

		public override double GenerationChance() => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Energy Tank");
			// Tooltip.SetDefault("Grants the user an extra tank of energy.");
			ItemNameLiteral = true;
			SacrificeTotal = 14;
			//ItemID.Sets.ShimmerTransformToItem[ItemType] = ModContent.ItemType<UAExpansion>();
			AddonSlot = SuitAddonSlotID.Tanks_Energy;
			TileID.Sets.FriendlyFairyCanLureTo[TileType] = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 11;
			item.maxStack = 14;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)											//The idea is that the recipe should be something you CAN make in lategame,
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(4)   //and that its ingredients are all renewable, sustainable resources,
				.AddIngredient<Items.Miscellaneous.PowerCore>(1)    //to a point where you could see the chozo having used the recipe themselves,
				.AddIngredient(ItemID.LifeFruit, 20)                //but actually making it in bulk would be incredibly tedious and annoying
				.AddIngredient<Items.Miscellaneous.EnergyShard>(15) //especially for a single person
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.EnergyTanks = stack;
		}
	}
}
