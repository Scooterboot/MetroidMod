using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace MetroidMod.Content.SuitAddons
{
	public class VariaSuitV2Addon : ModSuitUpgrade
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Tile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Helmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Breastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Breastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Greaves_Legs";


		public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid);

		public override double GenerationChance() => 4;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 15; //Added suit defense
		public static int energyCap = 4; //Added E-tank capacity
		public static float energyEff = 20f; //%Increased energy damage absorption
		public static float energyRes = 37.5f; //%Increased energy DR
		public static int overheatCap = 30; //Added maximum overheat
		public static float overheatCost = 15f; //%Decreased overheat cost
		public static float comboCost = 10f; //%Decreased Charge Combo cost
		public static float huntDamage = 10f; //%Increased hunter damage
		public static int huntCrit = 7; //Increased hunter crit
		public static float speedUp = 10f; //%Increased movement speed
		public static float extraBreath = 80f; //%Increased breath meter

		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyCap, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp, extraBreath);

		public override BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Barrier;

		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetDefaults(generatedModItem);
			
			generatedModItem.Item.width = 16;
			generatedModItem.Item.height = 16;
			generatedModItem.Item.value = Item.buyPrice(0, 2, 10, 0);
			generatedModItem.Item.rare = ItemRarityID.Orange;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += suitDef;
			player.nightVision = true;
			player.fireWalk = true;
			player.lavaRose = true;
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
			mp.tankCapacity += energyCap;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			// mp.EnergyDefenseEfficiency += energyEff / 100;
			// mp.EnergyExpenseEfficiency += energyRes / 100;
			mp.canHyper = true;
			mp.UACost -= 0.10f;
		}
		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
				.AddSuitAddon<VariaSuitAddon>(1)
				.AddRecipeGroup(MUtils.CalamityActive() ? MetroidMod.T1HMBarRecipeGroupID : MetroidMod.T3HMBarRecipeGroupID, 5)
				.AddIngredient<Items.Miscellaneous.KraidTissue>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
