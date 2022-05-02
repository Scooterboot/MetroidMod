using Terraria;
using Terraria.ID;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Suits
{
	public class VariaSuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate_Body";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitGreaves_Legs";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit");
			Tooltip.SetDefault("Increased resistance to cold temperatures.");
			AddonSlot = SuitAddonSlotID.Suit_Varia;
		}
		public override void SetItemDefaults()
		{
			Item.Item.value = Terraria.Item.buyPrice(0, 2, 10, 0);
			Item.Item.rare = ItemRarityID.Orange;
		}
		public override void OnUpdateArmorSet(Player player)
		{
			player.statDefense += 6;
			player.nightVision = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 5;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.10f;
			mp.missileCost -= 0.05f;
			mp.visorGlow = true;
			mp.breathMult = 1.55f;
		}
		public override void OnUpdateVanitySet(Player player)
		{
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HellstoneBar, 45)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
