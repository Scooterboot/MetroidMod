using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Content.Tiles;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class EnergyTank : ModSuitAddon, IBreastplateAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankTile";


		//public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedBoss2;

		public override double GenerationChance() => 4;

        public virtual BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Energy;

		public override void ItemSetStaticDefaults()
		{
			GeneratedModItem.Item.ResearchUnlockCount = 14;
			ItemID.Sets.ShimmerTransformToItem[ItemType] = ModContent.ItemType<MissileExpansion>();
		}
		public override void TileSetStaticDefaults()
		{
			base.TileSetStaticDefaults();

			TileID.Sets.FriendlyFairyCanLureTo[TileType] = true;
		}

		public override void ItemSetDefaults()
		{
			GeneratedModItem.Item.DefaultToPlaceableTile(TileType);

			GeneratedModItem.Item.width = 16;
			GeneratedModItem.Item.height = 11;
			GeneratedModItem.Item.maxStack = 14;
			GeneratedModItem.Item.value = Item.buyPrice(0, 10, 0, 0);
			GeneratedModItem.Item.rare = ItemRarityID.Green;
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(4)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 5)
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
