using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace MetroidMod.Content.SuitAddons
{
	public class HazardShieldAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitBreastplate_Arms_Glow";

		public override string ArmorTextureShouldersGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitBreastplate_Shoulders_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitGreaves_Legs";

		public override string OnShoulderTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitBreastplate_OnShoulder";

		public override string OffShoulderTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitBreastplate_OffShoulder";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue() => false;//WorldGen.drunkWorldGen;

		public override double GenerationChance() => 20;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 25; //Added suit defense
		//public static int energyCap = 4; //Added E-tank capacity, add between the above and below on the tooltip method
		public static float energyEff = 45f; //%Increased energy damage absorption
		public static float energyRes = 47.5f; //%Increased energy DR
		public static int overheatCap = 45; //Added maximum overheat
		public static float overheatCost = 20f; //%Decreased overheat cost
		public static float comboCost = 15f; //%Decreased Charge Combo cost
		public static float huntDamage = 15f; //%Increased hunter damage
		public static int huntCrit = 12; //Increased hunter crit
		public static float speedUp = 20f; //%Increased movement speed
		public static float extraBreath = 80f; //%Increased breath meter

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(suitDef, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp, extraBreath);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hazard Shield");
			/* Tooltip.SetDefault("You shouldn't have this."/*"+25 defense\n" +
				"+45 overheat capacity\n" +
				"20% decreased overheat use\n" +
				"15% decreased Missile Charge Combo cost\n" +
				"15% increased hunter damage\n" +
				"12% increased hunter critical strike chance\n" +
				"80% increased underwater breathing\n" +
				"20% increased movement speed\n" +
				"45% increased energy barrier efficiency\n" + // Provisional name
				"47.5% increased energy barrier resilience\n" + // Provisional name
				"Immunity to fire blocks" + "\n" +
				"Immunity to chill and freeze effects"
				"Debuffs tick down twice as fast");*/
			AddonSlot = SuitAddonSlotID.Suit_Barrier;
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
			player.nightVision = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.OnFire] = true;
			player.buffImmune[BuffID.Burning] = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.moveSpeed += speedUp / 100;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			mp.breathMult = 1f + (extraBreath / 100);
			//mp.tankCapacity += 0;
			mp.EnergyDefenseEfficiency += energyEff / 100;
			mp.EnergyExpenseEfficiency += energyRes / 100;
			mp.hazardShield += 1;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			if (player.TryGetModPlayer(out MPlayer mp))
			{
				mp.visorGlowColor = new Color(0, 228, 255);
				int primaryType = MPlayer.GetPowerSuit(player)[0].Type;
				if (!(primaryType == SuitAddonLoader.GetAddon<VortexAugment>().Type
					|| primaryType == SuitAddonLoader.GetAddon<NebulaAugment>().Type
					|| primaryType == SuitAddonLoader.GetAddon<SolarAugment>().Type))
				{
					ShouldOverrideShoulders = true;
				}
			}
		}
		/* Implement a recipe?
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddSuitAddon<VariaSuitV2Addon>(1)
				.AddRecipeGroup(ItemID.ShroomiteBar, 60)
				.AddTile<NovaWorkTableTile>()
				.Register();
		}
		*/
	}
}
