using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

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

		public override string ArmorTextureShouldersGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/TerraGravitySuit/TerraGravitySuitBreastplate_Shoulders_Glow";
		
		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || MSystem.bossesDown.HasFlag(MetroidBossDown.downedNightmare);

		public override double GenerationChance() => 4;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 19; //Added suit defense
		public static int energyCap = 4; //Added E-tank capacity
		public static float energyEff = 30f; //%Increased energy damage absorption
		public static float energyRes = 17.5f; //%Increased energy DR
		public static int overheatCap = 30; //Added maximum overheat
		public static float overheatCost = 10f; //%Decreased overheat cost
		public static float comboCost = 10f; //%Decreased Charge Combo cost
		public static float huntDamage = 10f; //%Increased hunter damage
		public static int huntCrit = 8; //Increased hunter crit
		public static float speedUp = 10f; //%Increased movement speed

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(suitDef, energyCap, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terra Gravity Suit");
			/* Tooltip.SetDefault("+19 defense\n" +
				"+30 overheat capacity\n" +
				"10% decreased overheat use\n" +
				"10% decreased Missile Charge Combo cost\n" +
				"10% increased hunter damage\n" +
				"8% increased hunter critical strike chance\n" +
				"10% increased movement speed\n" +
				"30% increased energy barrier efficiency\n" + // Provisional name
				"17.5% increased energy barrier resilience\n" + // Provisional name
				"Infinite breath underwater\n" +
				"Immune to knockback\n" +
				"Free movement in liquid\n" +
				"Grants 14 seconds of lava immunity\n" +
				"Default gravity in space\n" +
				"Immune to Distorted and Amplified Gravity effects"); */
			ItemID.Sets.ShimmerTransformToItem[ItemType] = SuitAddonLoader.GetAddon<PhazonSuitAddon>().ItemType;
			AddonSlot = SuitAddonSlotID.Suit_Primary;
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
			player.statDefense += suitDef;
			player.noKnockback = true;
			player.ignoreWater = true;
			if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
			{
				player.gills = true;
			}
			player.moveSpeed += speedUp / 100;
			player.lavaMax += 840;
			player.gravity = Player.defaultGravity;
			player.buffImmune[BuffID.VortexDebuff] = true;
			player.buffImmune[Terraria.ModLoader.ModContent.BuffType<Buffs.GravityDebuff>()] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.tankCapacity += energyCap;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			mp.EnergyDefenseEfficiency += energyEff / 100;
			mp.EnergyExpenseEfficiency += energyRes / 100;
			mp.UACost -= 0.1f;
			mp.accessHyperBeam = true;
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
				.AddSuitAddon<GravitySuitAddon>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
