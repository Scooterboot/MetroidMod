using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class HiJumpBoots : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/HiJumpBoots/HiJumpBootsItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/HiJumpBoots/HiJumpBootsTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => true;

		public override double GenerationChance(int x, int y) => 35;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hi-Jump Boots");
			Tooltip.SetDefault("Increases jump height\n" +
			"Stacks with other jump height accessories");
			AddonSlot = SuitAddonSlotID.Boots_JumpHeight;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(10)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(3)
				.AddIngredient(ItemID.Topaz, 1)
				.AddIngredient(ItemID.Emerald, 1)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient<Items.Accessories.HiJumpBoots>(1)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.GetModPlayer<Common.Players.MPlayer>().hiJumpBoost = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<Common.Players.MPlayer>().hiJumpBoost = true;
		}
	}
}
