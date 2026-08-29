using MetroidMod.Common.Players;
using MetroidMod.Content.Items;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class ReserveTank : ModSuitAddon, IBreastplateAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ReserveTank/ReserveTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ReserveTank/ReserveTankTile";

		public override bool CanGenerateOnChozoStatue() => true;

		public override double GenerationChance() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues ? 20 : 15;

		public BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Reserve;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Reserve Tank");
			// TODO: Write a better tooltip. I'm tired. - DarkSamus49
			// Tooltip.SetDefault("Can store a spare tank of energy.");
			//ItemID.Sets.ShimmerTransformToItem[ItemType] = ModContent.ItemType<Items.Accessories.ReserveTank>();
		}
		public override void ItemSetStaticDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetStaticDefaults(generatedModItem);
			generatedModItem.Item.ResearchUnlockCount = Common.Configs.MConfigItems.Instance.stackReserveTank;
		}

		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetDefaults(generatedModItem);
			generatedModItem.Item.width = 16;
			generatedModItem.Item.height = 11;
			generatedModItem.Item.maxStack = Common.Configs.MConfigItems.Instance.stackReserveTank;
			generatedModItem.Item.value = Item.buyPrice(0, 5, 0, 0);
			generatedModItem.Item.rare = ItemRarityID.Green;
		}
		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddIngredient(ItemID.LifeCrystal, 1)
				.AddTile(TileID.Anvils)
				.Register();
			generatedModItem.CreateRecipe(1)
				.AddIngredient<Items.Accessories.ReserveTank>(1)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.SuitReserveTanks = stack <= Common.Configs.MConfigItems.Instance.stackReserveTank ? stack : Common.Configs.MConfigItems.Instance.stackReserveTank;
		}
	}
}
