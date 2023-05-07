using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using MetroidMod.Common.Players;
using MetroidMod.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class TerraGravitySuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/TerraGravitySuit/TerraGravitySuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/TerraGravitySuit/TerraGravitySuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/TerraGravitySuit/TerraGravitySuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/TerraGravitySuit/TerraGravitySuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/TerraGravitySuit/TerraGravitySuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/TerraGravitySuit/TerraGravitySuitGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terra Gravity Suit");
			/* Tooltip.SetDefault("+10 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"5% increased hunter critical strike chance\n" +
				"10% increased movement speed\n" +
				"15% increased energy barrier efficiency\n" + // Provisional name
				"10% increased energy barrier resilience\n" + // Provisional name
				"Grants 7 seconds of lava immunity\n" +
				"Default gravity in space\n" +
				"Immune to Distorted and Amplified Gravity effects"); */
			AddonSlot = SuitAddonSlotID.Suit_Augment;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 11, 70, 0);
			item.rare = ItemRarityID.Lime;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += 10;
			player.moveSpeed += 0.10f;
			player.lavaMax += 420;
			player.gravity = Player.defaultGravity;
			player.buffImmune[BuffID.VortexDebuff] = true;
			player.buffImmune[Terraria.ModLoader.ModContent.BuffType<Buffs.GravityDebuff>()] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 5;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.05f;
			mp.missileCost -= 0.05f;
			mp.EnergyDefenseEfficiency += 0.15f;
			mp.EnergyExpenseEfficiency += 0.10f;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(138, 255, 252);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.ChlorophyteBar, 60)
				.AddIngredient<Items.Miscellaneous.NightmareCoreXFragment>(45)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
