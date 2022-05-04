using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.ID;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.SuitAddons
{
	public class SpaceJump : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJump/SpaceJumpItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJump/SpaceJumpTile";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Jump");
			Tooltip.SetDefault("'Somersault continuously in the air!'\n" +
				"Allows somersaulting\n" +
				"Allows the user to jump up to 10 times in a row\n" +
				"Jumps recharge mid-air\n" +
				"Increases jump height and prevents fall damage");
			AddonSlot = SuitAddonSlotID.Boots_Jump;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(SuitAddonLoader.GetAddon<SpaceJumpBoots>().Item, 1)
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.SoulofFlight, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJump = true;
			//mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
		public override void OnUpdateArmorSet(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJump = true;
			//mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
	}
}
