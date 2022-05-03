using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.ID;

namespace MetroidModPorted.Content.SuitAddons
{
	public class PowerGrip : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PowerGrip/PowerGripItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PowerGrip/PowerGripTile";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Grip");
			Tooltip.SetDefault("Allows the user to grab onto ledges\n" +
				"Does not need to be equipped; works while in inventory");
			AddonSlot = SuitAddonSlotID.Misc_Grip;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(4)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player)
		{
			player.GetModPlayer<Common.Players.MPlayer>().powerGrip = true;
		}
		public override void UpdateInventory(Player player)
		{
			player.GetModPlayer<Common.Players.MPlayer>().powerGrip = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<Common.Players.MPlayer>().powerGrip = true;
		}
	}
}
