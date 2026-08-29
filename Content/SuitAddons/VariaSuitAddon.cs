using MetroidMod.Common.Players;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace MetroidMod.Content.SuitAddons
{
	public class VariaSuitAddon : ModSuitUpgrade
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitGreaves_Legs";

		public override bool CanGenerateOnChozoStatue() => NPC.downedBoss2;//Main.UnderworldLayer;

		public override double GenerationChance() => 4;//20;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 6; //Added suit defense
		public static int energyCap = 2; //Added E-tank capacity
		public static float energyEff = 10f; //%Increased energy damage absorption
		public static float energyRes = 20f; //%Increased energy DR
		public static int overheatCap = 15; //Added maximum overheat
		public static float overheatCost = 10f; //%Decreased overheat cost
		public static float comboCost = 5f; //%Decreased Charge Combo cost
		public static float huntDamage = 5f; //%Increased hunter damage
		public static int huntCrit = 5; //Increased hunter crit
		public static float speedUp = 10f; //%Increased movement speed
		public static float extraBreath = 55f; //%Increased breath meter

		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyCap, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp, extraBreath);

		public override BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Barrier;


		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetDefaults(generatedModItem);

			GeneratedModItem.Item.width = 16;
			GeneratedModItem.Item.height = 16;
			GeneratedModItem.Item.value = Item.buyPrice(0, 2, 10, 0);
			GeneratedModItem.Item.rare = ItemRarityID.Orange;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += suitDef;
			player.nightVision = true;
			player.fireWalk = true;
			player.lavaRose = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.moveSpeed += speedUp / 100;
			player.breathEffectiveness += extraBreath / 100;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.tankCapacity += energyCap;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			mp.EnergyDefenseEfficiency += energyEff / 100;
			mp.EnergyExpenseEfficiency += energyRes / 100;
			mp.UACost -= 0.05f;
		}
		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
				.AddIngredient(ItemID.HellstoneBar, 45)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
