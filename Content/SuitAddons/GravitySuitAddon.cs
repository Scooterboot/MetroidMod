using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class GravitySuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => ((WorldGen.drunkWorldGen || WorldGen.everythingWorldGen) && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues) || MSystem.bossesDown.HasFlag(MetroidBossDown.downedPhantoon);

		public override double GenerationChance(int x, int y) => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gravity Suit");
			/* Tooltip.SetDefault("+9 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"3% increased hunter critical strike chance\n" +
				"15% increased energy barrier efficiency\n" + // Provisional name
				"7.5% increased energy barrier resilience\n" + // Provisional name
				"Infinite breath underwater\n" +
				"Immune to knockback\n" +
				"Free movement in liquid\n" +
				"Grants 7 seconds of lava immunity"); */
			AddonSlot = SuitAddonSlotID.Suit_Primary;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 7, 80, 0);
			item.rare = ItemRarityID.Pink;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += 9;
			player.noKnockback = true;
			player.ignoreWater = true;
			player.lavaMax += 420; // blaze it
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 3;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.05f;
			mp.missileCost -= 0.05f;
			mp.breathMult = 2;
			mp.EnergyDefenseEfficiency += 0.15f;
			mp.EnergyExpenseEfficiency += 0.075f;
			if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
			{
				player.gills = true;
			}
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(0, 248, 112);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HallowedBar, 54)
				.AddIngredient<Items.Miscellaneous.GravityFlare>(54)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
