using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace MetroidMod.Content.SuitAddons
{
	public class HazardShieldAddon : ModSuitUpgrade
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

		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp, extraBreath);

		public override BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Barrier;

		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetDefaults(generatedModItem);

			generatedModItem.Item.width = 16;
			generatedModItem.Item.height = 16;
			generatedModItem.Item.value = Item.buyPrice(0, 11, 70, 0);
			generatedModItem.Item.rare = ItemRarityID.Lime;
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
			player.breathEffectiveness += extraBreath / 100;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.EnergyDefense += suitDef;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			//mp.tankCapacity += 0;
			// mp.EnergyDefenseEfficiency += energyEff / 100;
			// mp.EnergyExpenseEfficiency += energyRes / 100;
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
		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
				.AddSuitAddon<VariaSuitV2Addon>(1)
				.AddRecipeGroup(ItemID.ShroomiteBar, 60)
				.AddTile<NovaWorkTableTile>()
				.Register();
		}
		*/
	}
}
