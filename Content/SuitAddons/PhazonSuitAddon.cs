using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class PhazonSuitAddon : ModSuitUpgrade
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitBreastplate_Arms_Glow";

		public override string ArmorTextureShouldersGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitBreastplate_Shoulders_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitGreaves_Legs";

		public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || MSystem.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate);

		public override double GenerationChance() => 4;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 15; //Added suit defense
		public static int energyCap = 4; //Added E-tank capacity
		public static float energyEff = 20f; //%Increased energy damage absorption
		public static float energyRes = 17.5f; //%Increased energy DR
		public static int overheatCap = 30; //Added maximum overheat
		public static float overheatCost = 10f; //%Decreased overheat cost
		public static float comboCost = 10f; //%Decreased Charge Combo cost
		public static float huntDamage = 15f; //%Increased hunter damage
		public static int huntCrit = 12; //Increased hunter crit
		public static float speedUp = 10f; //%Increased movement speed

		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyCap, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp);

		public override BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Primary;

		public override void ItemSetStaticDefaults(Items.GeneratedModItem generatedModItem)
		{
			ItemID.Sets.ShimmerTransformToItem[ItemType] = SuitAddonLoader.GetAddon<TerraGravitySuitAddon>().ItemType;
		}
		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
			base.ItemSetDefaults(generatedModItem);

			generatedModItem.Item.width = 16;
			generatedModItem.Item.height = 16;
			generatedModItem.Item.value = Item.buyPrice(0, 11, 70, 0);
			generatedModItem.Item.rare = ItemRarityID.Cyan;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += suitDef;
			//player.noKnockback = true;
			player.ignoreWater = true;
			if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
			{
				player.gills = true;
			}
			player.lavaMax += 420; // blaze it
			player.moveSpeed += speedUp / 100;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.tankCapacity += energyCap;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			// mp.EnergyDefenseEfficiency += energyEff / 100;
			// mp.EnergyExpenseEfficiency += energyRes / 100;
			mp.phazonImmune = true;
			mp.accessPhazonBeam = true;
			mp.UACost -= 0.10f;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 64, 0);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}
		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.PhazonBar>(60)
				.AddIngredient<Items.Miscellaneous.PurePhazon>(1)
				.AddSuitAddon<GravitySuitAddon>(1)
				.AddTile<Tiles.NovaWorkTableTile>()
				.Register();
		}
	}
}
