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

		public override void ItemSetStaticDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetStaticDefaults(generatedModItem);
			generatedModItem.Item.ResearchUnlockCount = 14;
			ItemID.Sets.ShimmerTransformToItem[ItemType] = ModContent.ItemType<MissileExpansion>();
		}
		public override void TileSetStaticDefaults(GeneratedModTile generatedModTile)
		{
			base.TileSetStaticDefaults(generatedModTile);
			TileID.Sets.FriendlyFairyCanLureTo[TileType] = true;
		}

		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.Item.DefaultToPlaceableTile(TileType);

			generatedModItem.Item.width = 16;
			generatedModItem.Item.height = 11;
			generatedModItem.Item.maxStack = 14;
			generatedModItem.Item.value = Item.buyPrice(0, 10, 0, 0);
			generatedModItem.Item.rare = ItemRarityID.Green;
		}
		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
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
