using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class VortexAugment : ModSuitUpgrade
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentGreaves_Legs";

		public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedMoonlord;
		public override double GenerationChance() => 1;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 29; //Added suit defense
		public static int energyCap = 6; //Added E-tank capacity
		public static float energyEff = 40f; //%Increased energy damage absorption
		public static float energyRes = 27.5f; //%Increased energy DR
		public static int overheatCap = 55; //Added maximum overheat
		public static float overheatCost = 15f; //%Decreased overheat cost
		public static float comboCost = 15f; //%Decreased Charge Combo cost
		public static float huntDamage = 15f; //%Increased hunter damage
		public static int huntCrit = 13; //Increased hunter crit
		public static float speedUp = 10f; //%Increased movement speed

		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyCap, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp);

		public override BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Primary;

		public override void ItemSetStaticDefaults(Items.GeneratedModItem generatedModItem)
		{
			ItemID.Sets.ShimmerTransformToItem[ItemType] = SuitAddonLoader.GetAddon<NebulaAugment>().ItemType;
		}
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
			// Chromatic cloak ability
			if (!player.controlDownHold)
			{
				player.shimmerImmune = true;
			}

			// Ignore shimmer slowdown ability
			if (player.TryGetModPlayer(out IgnoreShimmerModPlayer shimmerMp))
			{
				shimmerMp.ignoreShimmer = true;
			}

			player.statDefense += suitDef;
			//player.noKnockback = true;
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
			mp.EnergyDefense += suitDef;
			mp.tankCapacity += energyCap;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			// mp.EnergyDefenseEfficiency += energyEff / 100;
			// mp.EnergyExpenseEfficiency += energyRes / 100;
			mp.UACost -= 0.15f;
			mp.accessHyperBeam = true;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(67, 255, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 36)
				.AddIngredient(ItemID.FragmentVortex, 45)
				.AddSuitAddon<TerraGravitySuitAddon>(1)
				.AddTile(TileID.LunarCraftingStation)
				//.AddDecraftCondition(Condition.DownedMoonLord)
				.Register();
		}
	}
}
