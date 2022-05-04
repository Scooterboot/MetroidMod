using Terraria;
using Terraria.ID;
using MetroidModPorted.Common.Players;
using MetroidModPorted.ID;

namespace MetroidModPorted.Content.SuitAddons
{
	public class GravitySuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitGreaves_Legs";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit");
			Tooltip.SetDefault("+9 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"3% increased hunter critical strike chance\n" +
				"Infinite breath underwater\n" +
				"Immune to knockback\n" +
				"Free movement in liquid\n" +
				"Grants 7 seconds of lava immunity");
			AddonSlot = SuitAddonSlotID.Suit_Utility;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 7, 80, 0);
			item.rare = ItemRarityID.Pink;
		}
		public override void OnUpdateArmorSet(Player player)
		{
			player.statDefense += 9;
			player.noKnockback = true;
			player.ignoreWater = true;
			player.gills = true;
			player.lavaMax += 420; // blaze it
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 3;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.05f;
			mp.missileCost -= 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HallowedBar, 54)
				.AddIngredient<Items.Miscellaneous.GravityGel>(54)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
