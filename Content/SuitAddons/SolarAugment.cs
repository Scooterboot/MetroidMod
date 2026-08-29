using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace MetroidMod.Content.SuitAddons
{
	public class SolarAugment : ModSuitUpgrade
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentGreaves_Legs";

		public override bool CanGenerateOnChozoStatue() => false;//WorldGen.drunkWorldGen && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues;

		public override double GenerationChance() => 4;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 10; //Added suit defense
										//public static int energyCap = 1; //Added E-tank capacity, add between the above and below on the tooltip method
		public static float energyEff = 60f; //%Increased energy damage absorption
		public static float energyRes = 32.5f; //%Increased energy DR
		public static int overheatCap = 25; //Added maximum overheat
		public static float overheatCost = 5f; //%Decreased overheat cost
		public static float comboCost = 5f; //%Decreased Charge Combo cost
		public static float huntDamage = 5f; //%Increased hunter damage
		public static int huntCrit = 5; //Increased hunter crit
										//public static float speedUp = 10f; //%Increased movement speed

		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit);

		public override BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Primary;

		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetDefaults(generatedModItem);

			generatedModItem.Item.width = 16;
			generatedModItem.Item.height = 16;
			generatedModItem.Item.value = Item.buyPrice(0, 15, 60, 0);
			generatedModItem.Item.rare = ItemRarityID.Red;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += suitDef;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			mp.EnergyDefenseEfficiency += energyEff / 100;
			mp.EnergyExpenseEfficiency += energyRes / 100;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 254, 204);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
			player.armorEffectDrawOutlines = true;
		}
		/* Commented out because Dark World doesn't exist (yet)
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 36)
				.AddIngredient(ItemID.FragmentSolar, 45)
				.AddSuitAddon<LightSuitAddon>(1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
		*/
	}
}
